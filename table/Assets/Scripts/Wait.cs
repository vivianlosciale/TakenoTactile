using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{

    private float waitTime = 5.0f;
    void Start()
    {
        StartCoroutine(WaitForIntro());
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(1);
    }
}
