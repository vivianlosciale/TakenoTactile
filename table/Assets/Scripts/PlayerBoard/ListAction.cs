using System;
using System.Collections.Generic;
using UnityEngine;

public class ListAction : MonoBehaviour
{
    List<string> icons = new List<string>();
    public void addIcon(string icon)
    {
        icons.Add(icon);
        updateCollection();
    }

    public void removeIcon(string icon)
    {
        icons.Remove(icon);
        updateCollection();
    }

    private void updateCollection()
    {

        int max = 2;

        for (int i = 0; i < max; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().removeIcon();
        }
        for (int i = 0; i < icons.Count; i++)
        {
            transform.GetChild(i).GetComponent<ActionMaterial>().addIcon(icons[i]);
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
            transform.GetChild(i).GetComponent<ActionMaterial>().removeIcon();
        }
    }
}
