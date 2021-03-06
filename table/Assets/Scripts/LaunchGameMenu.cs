using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchGameMenu : MonoBehaviour
{
    public AudioClip music;
    public int level;

    public void Start()
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.clip = music;
        audioSource.Play();
        DontDestroyOnLoad(soundGameObject);
    }

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
