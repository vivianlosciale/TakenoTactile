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
        StartCoroutine(ChangeScene(sceneToLoad, sceneToUnload));
    }

    private IEnumerator ChangeScene(string sceneToLoad, string sceneToUnload)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Additive);
        var nextScene = SceneManager.GetSceneByName(sceneToLoad);
        SceneManager.MoveGameObjectToScene(gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(sceneToUnload);
    }

}

