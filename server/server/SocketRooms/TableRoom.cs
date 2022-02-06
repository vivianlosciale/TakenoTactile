using server.Utils.Game;
using server.Utils.Protocol;
using WebSocketSharp;

namespace server.SocketRooms;

public class TableRoom : SocketRoom
{
    private readonly Server _server;
    private PositionDto? _tilePosition;
    private bool _pickCard;
    private bool _canPlayPowerTwice;
    private int _powerUses;
    private readonly List<Powers> _chosenPowers = new();

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
            case MessageQuery.PickCard:
                //Console.WriteLine("Table asked for a card pick.");
                _pickCard = true;
                break;
            case MessageQuery.ChosePower:
                Powers power = PowersMethods.ToPowers(message.GetBody());
                if (!_canPlayPowerTwice && _chosenPowers.Contains(power)) break;
                _chosenPowers.Add(power);
                Console.WriteLine("Power '"+message.GetBody()+"' selected!");
                if (_chosenPowers.Count < _powerUses) Sender.Send(MessageQuery.ChosePower);
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

    public List<Powers> ChosePowers()
    {
        _canPlayPowerTwice = false;
        _powerUses = 2;
        _chosenPowers.Clear();
        _chosenPowers.Add(Powers.PickCard); // TODO remove
        
        /* TODO add
        
        Console.WriteLine("Waiting for the current player to chose " + _powerUses + " powers...");
        Sender.Send(MessageQuery.ChosePower);
        while (_chosenPowers.Count < _powerUses) WaitSeconds(1);
        
         */
        
        return new List<Powers>(_chosenPowers);
    }

    public PositionDto WaitForSelectTile()
    {
        _tilePosition = null;
        while (_tilePosition == null) WaitSeconds(1);
        return _tilePosition;
    }

    public void AddPowerUses(int supUses)
    {
        _powerUses += supUses;
    }

    public void CanPlayPowerTwice(bool value)
    {
        _canPlayPowerTwice = value;
    }
}