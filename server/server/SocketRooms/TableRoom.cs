using server.Utils.Game;
using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class TableRoom : SocketRoom
{
    private readonly Server _server;
    private PositionDto? _tilePosition;
    private CardTypes _pickCard;
    private UpgradeType _pickUpgrade;
    private bool _pickTiles;
    private bool _objectMoved;
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
                _pickCard = CardTypesMethods.ToPowers(message.GetBody());
                break;
            case MessageQuery.PickUpgrade:
                _pickUpgrade = UpgradeTypeMethods.ToUpgradeType(message.GetBody());
                break;
            case MessageQuery.PickTiles:
                _pickTiles = true;
                break;
            case MessageQuery.BambooEaten:
            case MessageQuery.BambooPlaced:
                _objectMoved = true;
                break;
            case MessageQuery.ChoseAction:
                Actions action = PowersMethods.ToPowers(message.GetBody());
                if (_currentPlayer == null || _currentPlayer.Validate) break;
                if (!_currentPlayer.CanPlayPowerTwice && _chosenPowers.Contains(action)) break;
                _chosenPowers.Add(action);
                Console.WriteLine("Power '"+message.GetBody()+"' selected!");
                if (_chosenPowers.Count >= _currentPlayer.PowerUses)
                {
                    Console.WriteLine("The player chose '"+MultiNames.ToMessage(_chosenPowers)+"'");
                    _currentPlayer.SendEvent(MessageQuery.ValidateChoice, "true");
                }
                break;
            case MessageQuery.RemoveAction:
                if (_currentPlayer == null || _currentPlayer.Validate) break;
                _chosenPowers.Remove(PowersMethods.ToPowers(message.GetBody()));
                _currentPlayer.SendEvent(MessageQuery.ValidateChoice, "false");
                Console.WriteLine("Power '"+message.GetBody()+"' removed!");
                break;
            case MessageQuery.ChosenPosition:
                _tilePosition = PositionDto.ToPosition(message.GetBody());
                break;
        }
    }

    public CardTypes WaitForCardPick(PlayerRoom player)
    {
        _pickCard = CardTypes.None;
        SendEvent(MessageQuery.WaitingPickCard);
        Console.WriteLine("Waiting for the current player to pick a card...");
        while (_pickCard.Equals(CardTypes.None) && !player.IsDisconnected()) WaitSeconds(1);
        return _pickCard;
    }

    public UpgradeType WaitForUpgradePick(PlayerRoom player)
    {
        _pickUpgrade = UpgradeType.None;
        SendEvent(MessageQuery.WaitingPickUpgrade);
        Console.WriteLine("Waiting for the current player to pick an upgrade...");
        while (_pickUpgrade.Equals(UpgradeType.None) && !player.IsDisconnected()) WaitSeconds(1);
        return _pickUpgrade;
    }

    public void WaitForTilesPick(PlayerRoom player)
    {
        _pickTiles = false;
        SendEvent(MessageQuery.WaitingPickTiles);
        Console.WriteLine("Waiting for the current player to pick tiles...");
        while (!_pickTiles && !player.IsDisconnected()) WaitSeconds(1);
    }

    public List<Actions>? ChosePowers(PlayerRoom player)
    {
        _currentPlayer = player;
        _chosenPowers.Clear();
        Sender.Send(MessageQuery.WaitingChoseAction);
        player.SendEvent(MessageQuery.WaitingChoseAction);
        Console.WriteLine("Waiting for the current player to chose " + _currentPlayer.PowerUses + " powers...");
        while (_chosenPowers.Count < _currentPlayer.PowerUses)
        {
            _currentPlayer.Validate = false;
            _chosenPowers.Clear();
            // TODO send message to mobile to notify a miss validation
            WaitValidation();
            if (_currentPlayer.IsDisconnected()) return default;
        }
        Console.WriteLine("Choices are made!");
        Sender.Send(MessageQuery.ValidateChoice);
        return new List<Actions>(_chosenPowers);
    }

    private void WaitValidation()
    {
        if(_currentPlayer == null) return;
        while (!_currentPlayer.Validate && !_currentPlayer.IsDisconnected()) WaitSeconds(1);
    }

    public PositionDto? WaitForSelectPosition(PlayerRoom player)
    {
        _tilePosition = null;
        Console.WriteLine("Waiting for the current player to chose a position!");
        while (_tilePosition == null && !player.IsDisconnected()) WaitSeconds(1);
        return _tilePosition;
    }

    public void WaitForObjectMoved(PlayerRoom player)
    {
        _objectMoved = false;
        while (!_objectMoved && !player.IsDisconnected()) WaitSeconds(1);
    }
}