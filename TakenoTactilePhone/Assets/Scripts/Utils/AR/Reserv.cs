using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reserv : MonoBehaviour
{
    public int nbBambooG;
    public int nbBambooP;
    public int nbBambooY;

    public GameObject bambooG;
    public GameObject bambooP;
    public GameObject bambooY;

    public GameObject imageTarget;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < nbBambooG; i++)
        {
            var bambooGtmp = Instantiate(bambooG, new Vector3(transform.position.x, transform.position.y + i * 0.035f, transform.position.z), transform.rotation);
            bambooGtmp.transform.parent = imageTarget.transform;
            bambooGtmp.transform.Rotate(new Vector3(-90, 0, 0));
        }
        for (int i = 0; i < nbBambooP; i++)
        {
            var bambooPtmp = Instantiate(bambooP, new Vector3(transform.position.x + 0.1f, transform.position.y + i * 0.035f, transform.position.z - 0.1f), transform.rotation);
            bambooPtmp.transform.parent = imageTarget.transform;
            bambooPtmp.transform.Rotate(new Vector3(-90, 0, 0));
        }
        for (int i = 0; i < nbBambooY; i++)
        {
            var bambooYtmp = Instantiate(bambooY, new Vector3(transform.position.x - 0.1f, transform.position.y + i * 0.035f, transform.position.z - 0.1f), transform.rotation);
            bambooYtmp.transform.parent = imageTarget.transform;
            bambooYtmp.transform.Rotate(new Vector3(-90, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
