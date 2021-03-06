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

    List<TuioEntity> tuioEvents = new List<TuioEntity>();
    List<TuioEntity> deadTouches = new List<TuioEntity>();

    private const string cursor = "/tuio/2Dcur";
    private const string obj = "/tuio/2Dobj";

    // Start is called before the first frame update
    void Start()
    {
        osc.SetAddressHandler(cursor, Generate2DTUIOEvent);
        osc.SetAddressHandler(obj, Generate2DTUIOEvent);
    }

    private void Generate2DTUIOEvent(OscMessage oscM)
    {
        string message = GetMessage(oscM);
        ManageTuioObjectEvent(message, oscM.address);
    }

    private void ManageTuioObjectEvent(string message, string adress)
    {
        string[] messageTab = message.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
        List<string> tmp = new List<string>(messageTab);
        switch (tmp[0])
        {
            case "alive":
                tmp.RemoveAt(0);
                UpdateCollection(tmp, adress);
                break;
            case "set":
                tmp.RemoveAt(0);
                if (adress == obj)
                    CheckObjectObj(tmp);
                else
                    CheckObject(tmp);
                break;
            case "fseq":
                string str = "Voici les detections:\n";
                foreach (TuioEntity t in tuioEvents)
                {
                    str += t;
                    //cast an invisible ray that will collide with the first object
                    Ray ray = Camera.main.ScreenPointToRay(new Vector3(t.position.TUIOPosition.x * Screen.width, t.position.TUIOPosition.y * Screen.height, 0));
                    if (Physics.Raycast(ray, out RaycastHit hit))
                        if (hit.transform.GetComponent<OSCEvent>() != null)
                            hit.transform.GetComponent<OSCEvent>().RunFunction(t); //will run specific function based on the state of the TUIOEvent
                }
                text.SetText(str);
                tuioEvents = tuioEvents.Except(deadTouches).ToList();
                break;
        }
    }

    private void CheckObject(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        float xCoord = float.Parse(tmp[1]);
        float yCoord = float.Parse(tmp[2]);
        TuioCursor tuioEvent = (TuioCursor)tuioEvents.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioCursor(id, xCoord, 1.0f - yCoord);
            tuioEvents.Add(tuioEvent);
            StartCoroutine(InstantiateType(tuioEvent));
        }
        else
        {
            Vector2 p = new Vector2(xCoord, 1.0f - yCoord);
            tuioEvent.UpdateCoordinates(p);
        }

    }

    private void CheckObjectObj(List<string> tmp)
    {
        int id = int.Parse(tmp[0]);
        string value = tmp[1];
        float xCoord = float.Parse(tmp[2]);
        float yCoord = float.Parse(tmp[3]);
        TuioObject tuioEvent = (TuioObject)tuioEvents.Find(e => e.Id == id);
        if (tuioEvent == null)
        {
            tuioEvent = new TuioObject(id, xCoord, 1.0f - yCoord, value);
            tuioEvents.Add(tuioEvent);
            StartCoroutine(InstantiateType(tuioEvent));
        }
        else
        {
            Vector2 p = new Vector2(xCoord, 1.0f - yCoord);
            tuioEvent.UpdateCoordinates(p);
        }

    }

    private IEnumerator InstantiateType(TuioEntity tuioEvent)
    {
        yield return new WaitForSeconds(1.0f);
        if (tuioEvents.Contains(tuioEvent) && tuioEvent.State == TuioState.MAINTAIN_DOWN)
            tuioEvent.State = TuioState.LONG_CLICK;
    }

    private void UpdateCollection(List<string> idAlive, string adress)
    {
        if (adress == obj)
            deadTouches = tuioEvents.FindAll(e => !(e is TuioCursor || idAlive.Contains(e.Id.ToString())));
        else
            deadTouches = tuioEvents.FindAll(e => !(e is TuioObject || idAlive.Contains(e.Id.ToString())));
        foreach (TuioEntity t in deadTouches)
            t.State = TuioState.CLICK_UP;
    }

    private string GetMessage(OscMessage message)
    {
        string res = "";
        foreach (object obj in message.values)
        {
            res += obj + " ";
        }
        return res;
    }

    private void Update()
    {
        foreach (TuioEntity t in tuioEvents)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(t.position.TUIOPosition.x * Screen.width, t.position.TUIOPosition.y * Screen.height, 0));
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100, Color.green);
        }
    }
}