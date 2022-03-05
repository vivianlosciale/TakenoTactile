using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    private string _serverAddress;

    public void SetAddressServer(string address)
    {
        _serverAddress = address;
    }

    public string GetAddressServer()
    {
        return _serverAddress;
    }
}