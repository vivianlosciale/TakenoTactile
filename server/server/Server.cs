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

    private readonly string _socketAddress;
    private readonly WebSocketServer _ws;
    private readonly List<PlayerRoom> _players;
    private readonly TableRoom _table;


    public Server()
    {
        _socketAddress = "ws://"+Device.GetIPv4()+":8080";
        _ws = new WebSocketServer(_socketAddress);
        _players = new();
        _table = new TableRoom(this);
    }
    
    
    /*
     * Create and start the login websocket.
     */
    public void StartServer()
    {
        _ws.AddWebSocketService(LoginPath, () => new LoginRoom(this));
        _ws.Start();
        Console.WriteLine("Server started on " + _socketAddress + LoginPath);
    }

    
    /*
     * Creat the table room to handle the communication with the table device.
     */
    public string SetTable()
    {
        _ws.AddWebSocketService(TablePath, () => _table);
        return _socketAddress + TablePath;
    }

    
    /*
     * Create a new player room to handle the communication with the player device.
     * And add the new player to the game.
     */
    public string? AddPlayer()
    {
        if (_players.Count < 4)
        {
            string privatePlayerPath = PlayerPath + _players.Count;
            PlayerRoom playerRoom = new PlayerRoom(this, _players.Count);
            _ws.AddWebSocketService(privatePlayerPath, () => playerRoom);
            _players.Add(playerRoom);
            _table.SendEvent(MessageQuery.APlayerJoined);
            return _socketAddress + privatePlayerPath;
        }
        return null;
    }

    
    /*
     * Remove the login room.
     * Start the game.
     */
    public void StartGame()
    {
        if (_players.Count >= 2)
        {
            _ws.RemoveWebSocketService(LoginPath);
            new Takenoko(_table, _players).StartGame();
        }
    }
}