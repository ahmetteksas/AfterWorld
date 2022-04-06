using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum CanvasType
{
    GameUI,
    PauseMenu,
    LoadingScreen,
    SplashScreen,
    PreviousCanvas,
}

public class CanvasManager : Singleton<CanvasManager>
{
    List<CanvasController> canvasControllerList;
    CanvasController lastActiveCanvas;


    [HideInInspector] public CanvasType previousCanvasType;
    bool isFirstLaunch=true;

    protected override void Awake()
    {
        base.Awake();
        canvasControllerList = GetComponentsInChildren<CanvasController>().ToList();
        canvasControllerList.ForEach(x => x.gameObject.SetActive(false));
        SwitchCanvas(CanvasType.GameUI);
    }

    public void SwitchCanvas(CanvasType _type)
    {
        CanvasController previousCanvas;

        if (_type == CanvasType.PreviousCanvas)
        {
            SwitchToPreviousCanvas(previousCanvasType); //if button asked for previous canvas.. this had to be hardcoded here
            return;
        }

        if (!isFirstLaunch)
        {
            previousCanvas = canvasControllerList.Find(x => x.gameObject.activeSelf); //store previous canvas type
            previousCanvasType = previousCanvas.canvasType;
        }

        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);
        if (desiredCanvas != null)
        {
            //Debug.Log("Switching to : " + desiredCanvas);
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }

        isFirstLaunch = false;
    }

    private void SwitchToPreviousCanvas(CanvasType _type)
    {
        if (lastActiveCanvas != null)
        {
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = canvasControllerList.Find(x => x.canvasType == _type);
        if (desiredCanvas != null)
        {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;
        }
        else { Debug.LogWarning("The desired canvas was not found!"); }
    }
}