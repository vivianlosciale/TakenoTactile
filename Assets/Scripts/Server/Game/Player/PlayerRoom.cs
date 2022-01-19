using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class PlayerRoom
{

    public PlayerRoom(WebSocketServer ws, string socketServicePath)
    {
        ws.AddWebSocketService<PlayerBehavior>(socketServicePath);
    }
    
    private class PlayerBehavior : WebSocketBehavior {
        
        protected override void OnMessage(MessageEventArgs e) {
            Debug.Log("Private message received from client: " + e.Data);
            Send("OK thx!");
        }
        
    }
}