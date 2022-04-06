using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    Transform tCam;

    private void Awake()
    {
        tCam = Camera.main.transform;
    }
    
    void Update()
    {
        transform.LookAt(tCam);
    }
}