using System;
using System.Collections;
using UnityEngine;

class CardMovement : MonoBehaviour
{
    private readonly float speed = 9.5f;
    private bool _isCenter;
    private Vector3 pos;
    private int child;
    private void Start()
    {
        child = transform.parent.childCount;
        pos = new Vector3(0, 13, 0);
        _isCenter = false;
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        while (true)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
            if (Vector3.Distance(transform.position, pos) < 0.01f)
            {
                if (_isCenter)
                {
                    transform.localPosition += new Vector3(0, 0, child * 0.01f);
                    Destroy(this);
                }

                _isCenter = true;
                yield return new WaitForSeconds(1.0f);
                pos = transform.root.TransformPoint(new Vector3(-0.0124f, -0.00051f, 0));
            }
            yield return new WaitForSeconds(0.001f);
        }
    }
}