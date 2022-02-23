using System;
using System.Collections.Generic;
using UnityEngine;

public class ListAction : MonoBehaviour
{
    List<string> icons = new List<string>();
    public void AddIcon(string icon)
    {
        icons.Add(icon);
        UpdateCollection();
    }

    public void RemoveIcon(string icon)
    {
        icons.Remove(icon);
        UpdateCollection();
    }

    private void UpdateCollection()
    {

        int max = 2;

        for (int i = 0; i < max; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().RemoveIcon();
        }
        for (int i = 0; i < icons.Count; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().AddIcon(icons[i]);
        }
    }

    public void useAction()
    {
        for (int i = 0; i < icons.Count; i++)
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
        for(int i = 0; i < icons.Count; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().RemoveIcon();
        }
        icons = new List<string>();
    }
}
