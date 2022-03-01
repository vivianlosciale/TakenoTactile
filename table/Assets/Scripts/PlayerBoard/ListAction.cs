using System;
using System.Collections.Generic;
using UnityEngine;

public class ListAction : MonoBehaviour
{
    List<string> actionIcons = new List<string>();
    public void AddActionIcon(string icon)
    {
        actionIcons.Add(icon);
        UpdateCollection();
    }

    public void RemoveActionIcon(string icon)
    {
        actionIcons.Remove(icon);
        UpdateCollection();
    }

    private void UpdateCollection()
    {
        int max = 3;

        for (int i = 0; i < max; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().RemoveIcon();
        }
        for (int i = 0; i < actionIcons.Count; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().AddIcon(actionIcons[i]);
        }
    }

    public void useAction()
    {
        for (int i = 0; i < actionIcons.Count; i++)
        {
            ActionMaterial actionMaterial = transform.GetChild(i).GetComponent<ActionMaterial>();
            if (!actionMaterial.used)
            {
                actionMaterial.used = true;
                break;
            }

        }
    }

    public void removeAllIcon()
    {
        for(int i = 0; i < actionIcons.Count; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().RemoveIcon();
        }
        actionIcons = new List<string>();
    }
}
