namespace MVC_trail;

public class GameView:IGameView
{
    public void Render(GameState model)
    {
        Console.Clear();
        foreach (Player p in model.Players)
        {
            Console.WriteLine("Player" + p.Id + ": " + p.Name + " (" + p.X + ")");
        }
    }
    public void ShowPlayerMoved(Player player, string direction)
    {
        Console.WriteLine($" Gracz {player.Id} poruszył się w stronę: {direction}");
    }
    public void ShowPlayerPositions(IEnumerable<Player> players)
    {
        Console.WriteLine(" Aktualne pozycje graczy:");
        foreach (var p in players)
        {
            Console.WriteLine($" - Gracz {p.Id}: ({p.X}, {p.Y})");
        }
    }

}