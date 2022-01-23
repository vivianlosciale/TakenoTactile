using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    public OSC osc;

    List<TuioEntity> tuioEvents = new List<TuioEntity>();
    List<TuioEntity> deadTouches = new List<TuioEntity>();

    private const string cursor = "/tuio/2Dcur";
    private const string obj = "/tuio/2Dobj";

    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler(cursor, generate2DTUIOEvent);
        osc.SetAddressHandler(obj, generate2DTUIOEvent);
    }

    private void generate2DTUIOEvent(OscMessage oscM)
    {
        string message = getMessage(oscM);
        manageTuioObjectEvent(message, oscM.address);
    }

    private void manageTuioObjectEvent(string message, string adress)
    {
        string[] messageTab = message.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        List<string> tmp = new List<string>(messageTab);
        switch (tmp[0])
        {
            case "alive":
                tmp.RemoveAt(0);
                updateCollection(tmp, adress);
                break;
            case "set":
                tmp.RemoveAt(0);
                if (adress == obj)
                    checkObjectObj(tmp);
                else
                    checkObject(tmp);
                break;
            case "fseq":
                foreach (TuioEntity t in tuioEvents)
                {
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(t.position.TUIOPosition.x * Screen.width, t.position.TUIOPosition.y * Screen.height, 0));
                    if (Physics.Raycast(ray, out RaycastHit hit))
                        if (hit.transform.GetComponent<OSCEvent>() != null)
                            hit.transform.GetComponent<OSCEvent>().RunFunction(t);
                }
                tuioEvents = tuioEvents.Except(deadTouches).ToList();
                break;
        }
    }

    private void checkObject(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        float xCoord = float.Parse(tmp[1]);
        float yCoord = float.Parse(tmp[2]);
        TuioCursor tuioEvent = (TuioCursor)tuioEvents.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioCursor(id, xCoord, yCoord);
            tuioEvents.Add(tuioEvent);
            StartCoroutine(instantiateType(tuioEvent));
        }
        else
        {
            Vector2 p = new Vector2(xCoord, yCoord);
            tuioEvent.updateCoordinates(p);
        }

    }

    private void checkObjectObj(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        int value = int.Parse(tmp[1]);
        float xCoord = float.Parse(tmp[2]);
        float yCoord = float.Parse(tmp[3]);
        float angle = float.Parse(tmp[4]);
        TuioObject tuioEvent = (TuioObject)tuioEvents.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioObject(id, xCoord, yCoord, angle, value);
            tuioEvents.Add(tuioEvent);
            StartCoroutine(instantiateType(tuioEvent));
        }
        else
        {
            Vector2 p = new Vector2(xCoord, yCoord);
            tuioEvent.updateCoordinates(p);
            tuioEvent.updateAngle(angle);
        }

    }

    private IEnumerator instantiateType(TuioEntity tuioEvent)
    {
        yield return new WaitForSeconds(1.0f);
        if (tuioEvents.Contains(tuioEvent) && tuioEvent.State == TuioState.CLICK_DOWN)
            tuioEvent.State = TuioState.LONG_CLICK;
    }

    private void updateCollection(List<string> idAlive, string adress)
    {
        if (adress == obj)
            deadTouches = tuioEvents.FindAll(e => !(idAlive.Contains(e.Id.ToString()) && !(e is TuioCursor)));
        else
            deadTouches = tuioEvents.FindAll(e => !idAlive.Contains(e.Id.ToString()) && !(e is TuioObject));
        foreach (TuioEntity t in deadTouches)
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