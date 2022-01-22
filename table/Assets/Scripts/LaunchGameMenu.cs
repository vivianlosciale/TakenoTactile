using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchGameMenu : MonoBehaviour
{
    public int level = 0;

    private void OnMouseDown()
    {
        SceneManager.LoadScene(level);
    }

    void enableCollider()
    {
        Debug.Log(name);
        GetComponent<MeshCollider>().enabled = true;
    }
}
