using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour
{
    public bool isUI = true;

    [HideInInspector] public Level targetLevel;
    [HideInInspector] public CanvasType canvasType;

    CanvasManager canvasManager;
    LevelLoader levelLoader;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
        canvasManager = CanvasManager.GetInstance();
        levelLoader = LevelLoader.GetInstance();
    }

    void OnButtonClicked()
    {
        if (isUI)
            canvasManager.SwitchCanvas(canvasType);
        else
            levelLoader.LoadLevel(targetLevel);
    }
}