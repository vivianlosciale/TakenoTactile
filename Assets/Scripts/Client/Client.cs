using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Client : MonoBehaviour
{

    public GameObject unityCamera;
    public Text text;

    private string _socketAddress;
    private WebSocket _ws;
    private string _id;
    
    public void Connect(string address)
    {
        unityCamera.SetActive(false);
        
        _socketAddress = address;
        _id = Device.GetIPv4();
        _ws = new WebSocket(_socketAddress+"/login");
        _ws.Connect();
        _ws.Send("REQUEST_LOG " + _id);
        _ws.OnMessage += MessageHandling;
    }
    
    private void MessageHandling(object sender, MessageEventArgs e)
    {
        string id = e.Data.Split(' ')[0];
        if (id.Equals(_id))
        {
            string path = e.Data.Substring(id.Length + 1);
            Debug.Log(path);
            text.text = path;
            _ws.Close();
            _ws = new WebSocket(_socketAddress+path);
            _ws.Connect();
            _ws.Send("Hello privately in '" + path + "'");
        }
        
    }
}