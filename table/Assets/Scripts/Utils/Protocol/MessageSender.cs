using System;
using UnityEngine;
using WebSocketSharp;

public class MessageSender
{

    public const char Separator = '#';

    private readonly WebSocket _room;

    public MessageSender(WebSocket room)
    {
        _room = room;
    }

    public void Send(MessageQuery query)
    {
        Send(QueryMethods.ToString(query));
    }

    public void Send(MessageQuery query, string message)
    {
        Send(QueryMethods.ToString(query) + Separator + message);
    }

    public void Send(MessageQuery query, string dest, string message)
    {
        Send(QueryMethods.ToString(query) + Separator + dest + Separator + message);
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
                Debug.Log("Message sent try " + tries + ": " + message);
            }
        }
        Debug.LogError("Message sent failed!");
    }
}