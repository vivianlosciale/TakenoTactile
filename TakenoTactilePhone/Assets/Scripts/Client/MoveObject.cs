using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    public string sceneToLoadName;
    public string sceneToUnloadName;
    private bool _load;

    GameObject mobileClientObject;

    private void Start()
    {
        this.mobileClientObject = GameObject.FindWithTag(TagManager.MobileClient.ToString());
    }

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

    public void MoveToSceneWithMobileClientObject()
    {
        if (_load) return;
        _load = true;

        StartCoroutine(ChangeScene(this.mobileClientObject));
    }

    private IEnumerator ChangeScene(GameObject gameObject)
    {
        SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Additive);
        var nextScene = SceneManager.GetSceneByName(sceneToLoadName);
        SceneManager.MoveGameObjectToScene(gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(sceneToUnloadName);
    }

}
