using System.Net.Sockets;

namespace MVC_trail;
public class ServerConsoleView : IGameView
{
    public void ShowPlayerMoved(Player player, string direction)
    {
        Console.WriteLine($"🎮 Gracz {player.Id} poruszył się w kierunku: {direction}");
    }

    public void ShowPlayerPositions(IEnumerable<Player> players)
    {
        Console.WriteLine("📍 Aktualne pozycje graczy:");
        foreach (var p in players)
        {
            Console.WriteLine($" - Gracz {p.Id}: ({p.X}, {p.Y})");
        }
    }
}
