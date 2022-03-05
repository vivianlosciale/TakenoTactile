using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    private bool _load;

    public void MoveToAnotherScene(string sceneToLoad, string sceneToUnload)
    {
        StartCoroutine(ChangeScene(sceneToLoad, sceneToUnload, gameObject));
    }

    public void MoveToAnotherSceneWithSave(string sceneToLoad, string sceneToUnload, GameObject saveObject)
    {
        StartCoroutine(ChangeScene(sceneToLoad, sceneToUnload, saveObject));
    }

    private IEnumerator ChangeScene(string sceneToLoad, string sceneToUnload, GameObject gameObject)
    {
        Debug.Log("HEY ! : " + sceneToLoad);
        Debug.Log("HEY 2 ! : " + sceneToUnload);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        var nextScene = SceneManager.GetSceneByName(sceneToLoad);
        SceneManager.MoveGameObjectToScene(gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}

