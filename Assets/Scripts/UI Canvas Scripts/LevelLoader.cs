using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Level //write level names here. These will be parsed to string from enum while loading scenes
{
    Level1,
}

public class LevelLoader : Singleton<LevelLoader>
{
    CanvasManager canvasManager;

    protected override void Awake()
    {
        base.Awake();
        canvasManager = FindObjectOfType<CanvasManager>();
    }

    public void LoadLevel(Level sceneName) => StartCoroutine(LoadAsynchronously(sceneName.ToString()));

    IEnumerator LoadAsynchronously(string sceneName)
    {
        print(canvasManager);
        canvasManager.SwitchCanvas(CanvasType.LoadingScreen);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
            yield return null;

        canvasManager.SwitchCanvas(CanvasType.GameUI);
    }
}