using System.Net.Sockets;
using System.Text.Json;
namespace MVC_trail;
public class Client {
    public async Task Start(string host, int port) {
        var gameView = new GameView();
        var client = new TcpClient();
        await client.ConnectAsync(host, port);

        Console.WriteLine("✅ Połączono z serwerem.");
        var stream = client.GetStream();
        var writer = new StreamWriter(stream) { AutoFlush = true };
        var reader = new StreamReader(stream);

        while (true) {
            Console.Write("👉 Podaj komendę: ");
            var cmd = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(cmd)) continue;

            // Wyślij komendę jako string JSON
            await writer.WriteLineAsync(JsonSerializer.Serialize(cmd));

            // Odbierz zaktualizowany stan gry
            var response = await reader.ReadLineAsync();
            if (response == null) {
                Console.WriteLine("❌ Serwer zakończył połączenie.");
                break;
            }

            // Próbujemy sparsować GameState
            var state = JsonSerializer.Deserialize<GameState>(response);
            if (state?.Players != null && state.Players.Count > 0) {
                gameView.Render(state);
            } else {
                Console.WriteLine("⚠️ Otrzymano pusty stan gry lub brak graczy.");
            }
        }
    }
}
