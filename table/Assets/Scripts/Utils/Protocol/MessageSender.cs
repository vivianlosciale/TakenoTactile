using System;
using UnityEngine;
using WebSocketSharp;

public class MessageSender
{

    private readonly WebSocket _room;

    public MessageSender(WebSocket room)
    {
        _room = room;
    }

    public void Send(MessageQuery query, string dest, string message)
    {
        Send(QueryMethods.ToString(query)+"-"+dest+"-"+message);
    }

    public void Send(MessageQuery query, string message)
    {
        Send(QueryMethods.ToString(query)+"-"+message);
    }

    public void Send(string message)
    {
        for (int tries = 0; tries < 5;)
        {
            try
            {
                _room.Send(message);
                return;
            }
            catch (Exception)
            {
                tries++;
                Debug.Log("Message sent try "+tries+": "+message);
            }
        }
        Debug.Log("Message sent fail!");
    }
}