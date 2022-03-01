using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    private bool _load;

    public void MoveToAnotherScene(string sceneToLoad, string sceneToUnload)
    {
        if (_load) return;
        _load = true;
        StartCoroutine(ChangeScene(sceneToLoad, sceneToUnload, gameObject));
    }

    public void MoveToAnotherSceneWithSave(string sceneToLoad, string sceneToUnload, GameObject saveObject)
    {
        if (_load) return;
        _load = true;
        StartCoroutine(ChangeScene(sceneToLoad, sceneToUnload, saveObject));
    }

    private IEnumerator ChangeScene(string sceneToLoad, string sceneToUnload, GameObject gameObject)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        var nextScene = SceneManager.GetSceneByName(sceneToLoad);
        SceneManager.MoveGameObjectToScene(gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }
}

