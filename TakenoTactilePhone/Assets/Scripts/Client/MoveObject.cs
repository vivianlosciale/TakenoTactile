using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{
    private const string _joinGameScene = "JoinGameScene";
    private const string _inGameScene = "InGameScene";
    private const string _endGameScene = "EndGameScene";
    public static string saved_result;
    private bool _load;

    public void MoveToInGameScene()
    {
        if (_load) return;
        _load = true;
        StartCoroutine(ChangeSceneWithMobileClient(_joinGameScene, _inGameScene));
    }

    public void MoveToEndGameScene(string result)
    {
        if (_load) return;
        _load = true;
        saved_result = result;
        StartCoroutine(ChangeToEndGameScene(_inGameScene, _endGameScene));
        //Debug.Log("result received " + result);
    }

    public void RestartGame()
    {
        if (_load) return;
        _load = true;
        StartCoroutine(ChangeScene(_endGameScene, _joinGameScene));
    }
    

    private IEnumerator ChangeSceneWithMobileClient(string from, string to)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        var nextScene = SceneManager.GetSceneByName(to);
        SceneManager.MoveGameObjectToScene(gameObject, nextScene);
        yield return null;
        SceneManager.UnloadSceneAsync(from);
        _load = false;
    }
    
    private IEnumerator ChangeToEndGameScene(string from, string to)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        yield return null;
        SceneManager.UnloadSceneAsync(from);
    }
    
    private IEnumerator ChangeScene(string from, string to)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        yield return null;
        SceneManager.UnloadSceneAsync(from);
    }

    public string GetResult()
    {
        return "La partie est terminée ! Vous avez eu " + saved_result + " points. Félicitations !";
    }
}
