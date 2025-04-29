using System.Net.Sockets;

namespace MVC_trail;
public interface IGameView
{
    void ShowPlayerMoved( Player player, string direction);
    void ShowPlayerPositions(IEnumerable<Player> players);
}
