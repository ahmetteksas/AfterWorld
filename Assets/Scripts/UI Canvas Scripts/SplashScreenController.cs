using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenController : MonoBehaviour
{
    bool isFirstLaunch = true;
    GameManager gameManager;
    
    void OnEnable()
    {
        if (!isFirstLaunch) return;
        isFirstLaunch = false;
        gameManager = GameManager.GetInstance();
        //gameManager.LoadGame();

        StartCoroutine(LoadLastScene(gameManager.level));
    }
    

    IEnumerator LoadLastScene(int sceneIndex)
    {
        if (sceneIndex > SceneManager.sceneCountInBuildSettings - 1)
	        sceneIndex = 3; //3. level olacak. normalde: Random.Range(5, SceneManager.sceneCountInBuildSettings-1)
        if (sceneIndex == 0) sceneIndex = 1;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        /*
        while (!operation.isDone)
        {
            yield return null;
        }
        */
        
        yield return null;
        //yield return new WaitForSecondsRealtime(1f);
        //SceneManager.SetActiveScene(SceneManager.GetSceneAt(sceneName));
        //Debug.Log("Active scene is: " + SceneManager.GetActiveScene());

        CanvasManager.GetInstance().SwitchCanvas(CanvasType.GameUI);
    }
}