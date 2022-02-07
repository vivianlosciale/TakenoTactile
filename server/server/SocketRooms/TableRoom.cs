using server.Utils.Game;
using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class TableRoom : SocketRoom
{
    private readonly Server _server;
    private PositionDto? _tilePosition;
    private bool _pickCard;
    private bool _pickTiles;
    private PlayerRoom? _currentPlayer;
    private readonly List<Actions> _chosenPowers = new();

    public TableRoom(Server server)
    {
        _server = server;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        MessageParser message = new MessageParser(e.Data);
        switch (message.GetQuery())
        {
            case MessageQuery.StartGame:
                _server.StartGame();
                break;
            case MessageQuery.Ping:
                Console.WriteLine("Table said: " + message.GetBody());
                break;
            case MessageQuery.Error:
                int playerNumber = int.Parse(message.GetDest());
                _server.SendError(playerNumber, message.GetBody());
                break;
            case MessageQuery.PickCard:
                _pickCard = true;
                break;
            case MessageQuery.PickTiles:
                _pickTiles = true;
                break;
            case MessageQuery.ChoseAction:
                Actions action = PowersMethods.ToPowers(message.GetBody());
                if (_currentPlayer == null || _currentPlayer.ValidateChoice()) break;
                if (!_currentPlayer.CanPlayPowerTwice() && _chosenPowers.Contains(action)) break;
                _chosenPowers.Add(action);
                Console.WriteLine("Power '"+message.GetBody()+"' selected!");
                if (_chosenPowers.Count >= _currentPlayer.GetPowerUses())
                {
                    Console.WriteLine("The player chose '"+MultiNames.ToMessage(_chosenPowers)+"'");
                    _currentPlayer.SendEvent(MessageQuery.ValidateChoice, "true");
                }
                break;
            case MessageQuery.RemoveAction:
                if (_currentPlayer == null || _currentPlayer.ValidateChoice()) break;
                _chosenPowers.Remove(PowersMethods.ToPowers(message.GetBody()));
                _currentPlayer.SendEvent(MessageQuery.ValidateChoice, "false");
                Console.WriteLine("Power '"+message.GetBody()+"' removed!");
                break;
            case MessageQuery.ChosenTile:
                _tilePosition = PositionDto.ToPosition(message.GetBody());
                break;
        }
    }

    public void WaitForCardPick()
    {
        _pickCard = false;
        SendEvent(MessageQuery.WaitingPickCard);
        while (!_pickCard) WaitSeconds(1);
    }

    public void WaitForTilesPick()
    {
        _pickTiles = false;
        SendEvent(MessageQuery.WaitingPickTiles);
        while (!_pickTiles) WaitSeconds(1);
    }

    public List<Actions> ChosePowers(PlayerRoom player)
    {
        _currentPlayer = player;
        _chosenPowers.Clear();
        Console.WriteLine("Waiting for the current player to chose " + _currentPlayer.GetPowerUses() + " powers...");
        Sender.Send(MessageQuery.ChoseAction);
        player.SendEvent(MessageQuery.ChoseAction);
        player.SetValidate(false);
        while (_chosenPowers.Count < _currentPlayer.GetPowerUses() || !player.ValidateChoice()) WaitSeconds(1);
        player.ResetPowerUses();
        Sender.Send(MessageQuery.ValidateChoice);
        return new List<Actions>(_chosenPowers);
    }

    public PositionDto WaitForSelectPosition()
    {
        _tilePosition = null;
        while (_tilePosition == null) WaitSeconds(1);
        return _tilePosition;
    }
}