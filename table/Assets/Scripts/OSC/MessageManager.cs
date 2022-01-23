using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public OSC osc;
    public TextMeshPro text;

    List<TuioCursor> tuioCur = new List<TuioCursor>();
    List<TuioObject> tuioObj = new List<TuioObject>();
    List<TuioCursor> deadTouches = new List<TuioCursor>();
    List<TuioObject> deadTouchesObj = new List<TuioObject>();

    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler("/tuio/2Dcur", generate2DMouseEvent);
        osc.SetAddressHandler("/tuio/2Dobj", generate2DObjectEvent);
    }

    private void generate2DObjectEvent(OscMessage oscM)
    {
        string message = getMessage(oscM);
        manageTuioObjectEvent(message);
    }

    //generate 3 line on clic down and update/drag, 5 lines for drag/update
    private void generate2DMouseEvent(OscMessage oscM)
    {
        string message = getMessage(oscM);
        manageTuioCursorEvent(message);

    }

    private void manageTuioObjectEvent(string message)
    {
        string[] messageTab = message.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        List<string> tmp = new List<string>(messageTab);
        switch (tmp[0])
        {
            case "alive":
                tmp.RemoveAt(0);
                updateCollectionObj(tmp);
                break;
            case "set":
                tmp.RemoveAt(0);
                checkObjectObj(tmp);
                break;
            case "fseq":
                string str = "Voici les detections:\n";
                foreach (TuioObject t in tuioObj)
                {
                    str = str + t;
                }
                text.SetText(str);
                tuioObj = tuioObj.Except(deadTouchesObj).ToList();
                break;
        }
    }

    private void manageTuioCursorEvent(string message)
    {
        string[] messageTab = message.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        List<string> tmp = new List<string>(messageTab);
        switch (tmp[0])
        {
            case "alive":
                tmp.RemoveAt(0);
                updateCollection(tmp);
                break;
            case "set":
                tmp.RemoveAt(0);
                checkObject(tmp);
                break;
            case "fseq":
                string str = "Voici les detections:\n";
                foreach (TuioCursor t in tuioCur)
                {
                    str = str + t;
                    if (t.isClick())
                    {
                        //server.Broadcast("Hello World !");
                    }
                }
                text.SetText(str);
                tuioCur = tuioCur.Except(deadTouches).ToList();
                break;
        }
    }

    private void checkObject(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        float xCoord = float.Parse(tmp[1]);
        float yCoord = float.Parse(tmp[2]);
        TuioCursor tuioEvent = tuioCur.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioCursor(id, xCoord, yCoord);
            tuioCur.Add(tuioEvent);
            StartCoroutine(instantiateType(tuioEvent));
        }
        else
        {
            Position p = new Position(xCoord, yCoord);
            tuioEvent.updateCoordinates(p.TUIOPosition);
        }

    }

    private void checkObjectObj(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        int value = int.Parse(tmp[1]);
        float xCoord = float.Parse(tmp[2]);
        float yCoord = float.Parse(tmp[3]);
        float angle = float.Parse(tmp[4]);
        TuioObject tuioEvent = tuioObj.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioObject(id, xCoord, yCoord, angle, value);
            tuioObj.Add(tuioEvent);
            StartCoroutine(instantiateTypeObj(tuioEvent));
        }
        else
        {
            Position p = new Position(xCoord, yCoord);
            tuioEvent.updateCoordinates(p.TUIOPosition);
            tuioEvent.updateAngle(angle);
        }

    }
    private IEnumerator instantiateTypeObj(TuioObject tuioEvent)
    {
        yield return new WaitForSeconds(1.0f);
        if (tuioObj.Contains(tuioEvent) && tuioEvent.State == TuioState.CLICK_DOWN)
            tuioEvent.State = TuioState.LONG_CLICK;
    }

    private IEnumerator instantiateType(TuioCursor tuioEvent)
    {
        yield return new WaitForSeconds(1.0f);
        if (tuioCur.Contains(tuioEvent) && tuioEvent.State == TuioState.CLICK_DOWN)
            tuioEvent.State = TuioState.LONG_CLICK;
    }

    private void updateCollection(List<string> idAlive)
    {
        deadTouches = tuioCur.FindAll(e => (!idAlive.Contains(e.Id.ToString())));
        foreach (TuioCursor t in deadTouches)
            t.State = TuioState.CLICK_UP;
    }
    private void updateCollectionObj(List<string> idAlive)
    {
        deadTouchesObj = tuioObj.FindAll(e => (!idAlive.Contains(e.Id.ToString())));
        foreach (TuioObject t in deadTouchesObj)
            t.State = TuioState.CLICK_UP;
    }

    private string getMessage(OscMessage message)
    {
        string res = "";
        foreach (object obj in message.values)
        {
            res += obj + " ";
        }
        return res;
    }
}