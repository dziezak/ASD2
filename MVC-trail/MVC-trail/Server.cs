using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MVC_trail;
// Server.cs
public class Server {
    private TcpListener _listener;
    private GameState _state = new();
    private List<TcpClient> _clients = new();
    private List<GameController> _controllers = new();

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

            _ = Task.Run(() => HandleClient(client, playerId));
            //BroadcastState();
            //BroadcastPlayerPositions();
        }
    }

    private async Task HandleClient(TcpClient client, int playerId) {
        using var stream = client.GetStream();
        using var writer = new StreamWriter(stream) { AutoFlush = true }; // nwm o co chodzi
        using var reader = new StreamReader(stream);
        
        //linijka inicjalizacj :>
        var init = JsonSerializer.Serialize(new { Type = "init", PlayerId = playerId });
        await writer.WriteLineAsync(init); 
        
        while (true) {
            var line = await reader.ReadLineAsync();
            if (line != null) {
                var command = JsonSerializer.Deserialize<string>(line);
                _controllers.Add(new GameController(_state, new GameView()));
                _controllers.Last().HandleInput(command, playerId);
                
                BroadcastState();
            }
        }
    }

    // works on server 
    private void BroadcastState() {
        var json = JsonSerializer.Serialize(_state);
        var message = json + "\n";
        var bytes = Encoding.UTF8.GetBytes(message);

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
    }
}
