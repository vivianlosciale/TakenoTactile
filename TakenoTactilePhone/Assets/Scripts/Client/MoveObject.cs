using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    public string sceneToLoadName;
    public string sceneToUnloadName;
    private bool _load;

    public void MoveToAnotherScene()
    {
        if (_load) return;
        _load = true;
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Additive);
        var nextScene = SceneManager.GetSceneByName(sceneToLoadName);
        SceneManager.MoveGameObjectToScene(gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(sceneToUnloadName);
    }

}
