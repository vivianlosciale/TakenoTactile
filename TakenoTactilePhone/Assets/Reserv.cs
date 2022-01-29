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
            var bambooGtmp = Instantiate(bambooG, new Vector3(transform.position.x, transform.position.y + i * 0.1f, transform.position.z), Quaternion.identity);
            bambooGtmp.transform.parent = imageTarget.transform;
        }
        for (int i = 0; i < nbBambooP; i++)
        {
            var bambooPtmp = Instantiate(bambooP, new Vector3(transform.position.x + 0.1f, transform.position.y + i * 0.1f, transform.position.z - 0.1f), Quaternion.identity);
            bambooPtmp.transform.parent = imageTarget.transform;
        }
        for (int i = 0; i < nbBambooY; i++)
        {
            var bambooYtmp = Instantiate(bambooY, new Vector3(transform.position.x - 0.1f, transform.position.y + i * 0.1f, transform.position.z - 0.1f), Quaternion.identity);
            bambooYtmp.transform.parent = imageTarget.transform;
        }
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
