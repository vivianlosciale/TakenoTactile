using UnityEngine;
using WebSocketSharp;

public class Client : MonoBehaviour
{

    WebSocket ws;
    private void Start()
    {
    }

    private void Update()
    {
        if(ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Hello");
        }
        if (Input.touchCount > 0)
        {
            ws.Send("Touch");
        }
    }

    public void Connect(string address)
    {
        ws = new WebSocket(address);
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
        };
    }
}