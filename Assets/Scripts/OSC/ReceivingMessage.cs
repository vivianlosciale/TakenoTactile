using UnityEngine;

public class ReceivingMessage : MonoBehaviour
{
    public OSC osc;

    // Start is called before the first frame update
    void Start()
    {
        osc.SetAllMessageHandler(OnReceiveX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnReceiveX(OscMessage message)
    {
        Debug.Log(message);
        string res = "";
        foreach(object obj in message.values)
        {
            res += obj + " ";
        }
        //Debug.Log(res);
    }
}