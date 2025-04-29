using System.Net.Sockets;
using System.Text.Json;
namespace MVC_trail;

public class Client {
    public async Task Start(string host, int port) {
        var gameView = new GameView();
        var client = new TcpClient();
        await client.ConnectAsync(host, port);

        Console.WriteLine("Connected to server.");
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        var reader = new StreamReader(stream);

        int myPlayerId = -1;
        

        while (true) {
            var cmd = Console.ReadLine();
            await writer.WriteLineAsync(JsonSerializer.Serialize(cmd));
            var json = await reader.ReadLineAsync();
            GameState state = null;
            if (json != null)
            {
                state = JsonSerializer.Deserialize<GameState>(json);
                if (state != null)
                {
                    if (state.Players != null && state.Players.Count > 0)
                    {
                        gameView.Render(state);
                    }
                    else
                    {
                        //Console.WriteLine("Ale przypał");
                        Console.WriteLine("Blad -> pusta lista graczy");
                    }
                }
                else {
                    Console.WriteLine("Failed to deserialize the game state.");
                }
            }
        }
    }
}
