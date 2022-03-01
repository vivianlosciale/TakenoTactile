using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveObject : MonoBehaviour
{
    private const string _joinGameScene = "JoinGameScene";
    private const string _inGameScene = "InGameScene";
    private const string _endGameScene = "EndGameScene";
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
        StartCoroutine(ChangeToEndGameScene(_inGameScene, _endGameScene, result));
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
    
    private IEnumerator ChangeToEndGameScene(string from, string to, string result)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        yield return null;
        SceneManager.UnloadSceneAsync(from);
        GameObject.FindWithTag(TagManager.EndGameResult.ToString()).GetComponent<TextMeshPro>().text = 
            "La partie est terminée ! Vous avez eu " + result + " points. Félicitations !";
    }
    
    private IEnumerator ChangeScene(string from, string to)
    {
        SceneManager.LoadScene(to, LoadSceneMode.Additive);
        yield return null;
        SceneManager.UnloadSceneAsync(from);
    }

}
