using server.Game;
using server.SocketRooms;
using server.Utils.Devices;
using server.Utils.Protocol;
using WebSocketSharp.Server;

namespace server;

public class Server
{
    private const string LoginPath = "/login";
    private const string PlayerPath = "/player";
    private const string TablePath = "/table";
    private const string ReconnectionPath = "/reconnection";

    private readonly string _socketAddress;
    private readonly WebSocketServer _ws;
    private readonly List<PlayerRoom> _players;
    private TableRoom _table;
    private Takenoko _game;
    private Thread? _gameThread;


    public Server()
    {
        _socketAddress = "ws://"+Device.GetIPv4()+":8080";
        _ws = new WebSocketServer(_socketAddress) { Log={ Output=(_,_)=>{} } };
        _players = new();
        _table = new TableRoom(this);
        _game = new Takenoko(_table, _players, this);
    }
    
    
    /*
     * Create and start the login websocket.
     */
    public void StartServer()
    {
        _ws.AddWebSocketService(LoginPath, () => new LoginRoom(this));
        _ws.AddWebSocketService(ReconnectionPath, () => new ReconnectionRoom(this));
        _ws.Start();
        Console.WriteLine("Server started on " + _socketAddress + LoginPath);
    }

    
    /*
     * Creat the table room to handle the communication with the table device.
     */
    public string SetTable()
    {
        if (_ws.WebSocketServices.Paths.Contains(TablePath)) return string.Empty;
        _ws.AddWebSocketService(TablePath, () => _table);
        return _socketAddress + TablePath;
    }

    
    /*
     * Create a new player room to handle the communication with the player device.
     * And add the new player to the game.
     */
    public string? AddPlayer(int roomNumber)
    {
        if (_players.Count < 4)
        {
            string privatePlayerPath = PlayerPath + roomNumber;
            PlayerRoom playerRoom = new PlayerRoom(_game, roomNumber, _socketAddress + ReconnectionPath);
            _ws.AddWebSocketService(privatePlayerPath, () => playerRoom);
            _players.Add(playerRoom);
            _table.SendEvent(MessageQuery.APlayerJoined, playerRoom.GetNumber().ToString()); // TODO
            return _socketAddress + privatePlayerPath;
        }
        return null;
    }


    public string GetPlayerRoot(int playerRoom)
    {
        foreach (PlayerRoom player in _players)
        {
            if (player.GetNumber().Equals(playerRoom)) return _socketAddress + PlayerPath + player.GetNumber();
        }
        throw new Exception("Player room not found: "+playerRoom);
    }
    
    
    public void RemovePlayer(PlayerRoom player)
    {
        string servicePath = PlayerPath + player.GetNumber();
        _ws.RemoveWebSocketService(servicePath);
        if (player.IsDisconnected())
        {
            PlayerRoom resetPlayer = new(player);
            _players[_players.IndexOf(player)] = resetPlayer;
            _ws.AddWebSocketService(servicePath, () => resetPlayer);
        }
        else
        {
            _players.Remove(player);
        }
    }

    
    /*
     * Remove the login room.
     * Start the game.
     */
    public void StartGame()
    {
        if (_players.Count >= 1)
        {
            _ws.RemoveWebSocketService(LoginPath);
            _gameThread = new Thread(_game.StartGame);
            _gameThread.Start();
        }
    }

    
    /*
     * Reset all server parameters
     */
    public void EndGame()
    {
        _players.Clear();
        _table = new TableRoom(this);
        _game = new Takenoko(_table, _players, this);
        foreach (string path in _ws.WebSocketServices.Paths)
        {
            _ws.RemoveWebSocketService(path);
        }
        _ws.AddWebSocketService(LoginPath, () => new LoginRoom(this));
    }

    
    public void SendError(int playerNumber, string body)
    {
        foreach (PlayerRoom player in _players)
        {
            if (player.GetNumber().Equals(playerNumber))
            {
                player.SendEvent(MessageQuery.Error, body);
            }
        }
    }


    private void DisplayServices()
    {
        Console.WriteLine("\nServices are:");
        foreach (string path in _ws.WebSocketServices.Paths)
        {
            Console.WriteLine("\t"+path);
        }
        Console.WriteLine();
    }
}