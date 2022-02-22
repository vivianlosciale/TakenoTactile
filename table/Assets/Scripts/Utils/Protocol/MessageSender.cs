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

    public Exception Send(MessageQuery query)
    {
        return Send(QueryMethods.ToString(query));
    }

    public Exception Send(MessageQuery query, string message)
    {
        return Send(QueryMethods.ToString(query) + Separator + message);
    }

    public Exception Send(MessageQuery query, string dest, string message)
    {
        return Send(QueryMethods.ToString(query) + Separator + dest + Separator + message);
    }

    public Exception Send(string message)
    {
        for (int tries = 0; tries < 5;)
        {
            try
            {
                _room.Send(message);
                return null;
            }
            catch (Exception)
            {
                tries++;
                Debug.Log("Message sent try " + tries + ": " + message);
            }
        }
        Debug.LogError("Message sent failed!");
        return new Exception();
    }
}