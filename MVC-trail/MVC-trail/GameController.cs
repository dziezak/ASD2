namespace MVC_trail;

public class GameController
{
    private readonly GameState _state;
    private readonly IGameView _view;
    public GameController(GameState state, GameView view)
    {
        _state = state;
        _view = view;
    }

    public void HandleInput(string command, int playerId)
    {
       Player player = _state.Players.Find(p => p.Id == playerId);
       string direction = null;
       switch (command.ToLower())
       {
           case "w": 
               player.Y++;
               direction = "Gora"; 
               break;
           case "s": 
               player.Y--; 
               direction = "Dol";
               break;
           case "a": 
               player.X--;
               direction = "Lewo";
               break;
           case "d": 
               player.X++;
               direction = "Prawo";
               break;
           default: return;
       }
       if (direction != null)
       {
           //_view.ShowPlayerMoved(player, direction);
           //_view.ShowPlayerPositions(_state.Players);
       }
    }
}