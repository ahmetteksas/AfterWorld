using UnityEngine;

public class NewLevelStart : MonoBehaviour
{
    private void Update()
    {
        //if (Input.touchCount > 0 || Input.GetMouseButton(0))
            //StartLevel();
    }

    private void StartLevel()
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ActivateControls(true);
        CanvasManager.GetInstance().SwitchCanvas(CanvasType.GameUI);
    }
}
