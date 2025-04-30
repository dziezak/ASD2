using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace MVC_trail;
// Server.cs
public class Server {
    private TcpListener _listener;
    private GameState _state = new();
    private List<TcpClient> _clients = new();
    private Dictionary<int, GameController> _controllers = new(); // po ID gracza
    public GameView _gameView = new GameView();

    public async Task Start(int port = 5555) {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();

        Console.WriteLine("Server started...");

        while (true) {
            var client = await _listener.AcceptTcpClientAsync();
            _clients.Add(client);
            var playerId = _clients.Count;
            _state.Players.Add(new Player { Id = playerId, Name = $"Player{playerId}", X = 0, Y = 0 });
            
            Console.WriteLine($"Player {playerId} joined");

            _controllers[playerId] = new GameController(_state, _gameView);
            _ = Task.Run(() => HandleClient(client, playerId));
        }
    }

    /*
    private async Task HandleClient(TcpClient client, int playerId) {
        using var stream = client.GetStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true }; // ogarnac o co chodzi z AutoFlush
        using var reader = new StreamReader(stream);
        
        //linijka inicjalizacj :>
        var init = JsonSerializer.Serialize(new { Type = "init", PlayerId = playerId });
        await writer.WriteLineAsync(init); 
        
        while (true) {
            var line = await reader.ReadLineAsync();
            if (line != null) {
                var command = JsonSerializer.Deserialize<string>(line);
                if (_controllers.TryGetValue(playerId, out var controller)) {
                    controller.HandleInput(command, playerId);
                }
                BroadcastState();
            }
        }
    }
    */
    private async Task HandleClient(TcpClient client, int playerId) {
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        var reader = new StreamReader(stream);

        Console.WriteLine($"🎮 Gracz {playerId} podłączony.");

        while (true) {
            var line = await reader.ReadLineAsync();
            if (line == null) {
                Console.WriteLine($"❌ Gracz {playerId} rozłączony.");
                break;
            }

            var command = JsonSerializer.Deserialize<string>(line);

            // Obsługa komendy
            var controller = new GameController(_state, null); // nie potrzebujemy GameView
            controller.HandleInput(command, playerId);

            // Odesłanie aktualnego stanu gry do TYLKO tego klienta
            /*
            var responseJson = JsonSerializer.Serialize(_state);
            await writer.WriteLineAsync(responseJson);
            */
            BroadcastState();
        }
    }

    
    private void BroadcastState() {
        var json = JsonSerializer.Serialize(_state);
        var bytes = Encoding.UTF8.GetBytes(json + "\n");

        Console.WriteLine($"Liczba graczy {_clients.Count}");
        foreach (var client in _clients) {
            try
            {
                var stream = client.GetStream();
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Blad przy wyslaniu do klienta: {ex.Message}");
            }
        }
        _gameView.Render(_state);
    }
}
