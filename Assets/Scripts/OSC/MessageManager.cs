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

    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler("/tuio/2Dcur", generate2DMouseEvent);
        osc.SetAddressHandler("/tuio/2Dobj", generate2DObjectEvent);
    }

    private void generate2DObjectEvent(OscMessage oscM)
    {
        //check in list if everyone is still alive
        //if not, remove the one
        //create/update the object and put it in the list
    }

    //generate 3 line on clic down and update/drag, 5 lines for drag/update
    private void generate2DMouseEvent(OscMessage oscM)
    {
        string message = getMessage(oscM);
        manageTuioCursorObject(message);

    }

    private void manageTuioCursorObject(string message)
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
                    str = str + "detection numero " + t.Id + ": clic:" + t.isClick() + " drag:" + t.isDrag() + " longclic:" + t.isLongClick() + "\n";
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
            tuioEvent.updateCoordinates(xCoord, yCoord);
        }

    }

    private IEnumerator instantiateType(TuioCursor tuioEvent)
    {
        yield return new WaitForSeconds(1.0f);
        if (tuioCur.Contains(tuioEvent))
            if (!tuioEvent.AlreadyUpdate)
                tuioEvent.State = CursorState.LONG_CLICK;
    }

    private void updateCollection(List<string> idAlive)
    {
        deadTouches = tuioCur.FindAll(e => (!idAlive.Contains(e.Id.ToString())));
        foreach (TuioCursor t in deadTouches)
            t.State = CursorState.CLICK_UP;
        //tuioCur.RemoveAll(e => (!idAlive.Contains(e.Id.ToString())));
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