//using UnityEngine;

public class CameraSingleton : Singleton<CameraSingleton>
{
    /*
    public Transform target;
    public float swapSpeed = 1f;

    Quaternion newRot;
    Vector3 relPos;

    PlayerController playerController;

    private void Start() {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update() {
        if (target==null){
         target = playerController.coasters[0].transform;
        }else{
            SmoothFollow();
            SmoothRotate();
        }
    }

    private void SmoothRotate(){
            relPos = target.position - transform.position;
            newRot = Quaternion.LookRotation(relPos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRot, Time.time * swapSpeed);
    }

    private void SmoothFollow(){
        Vector3 toPos = target.position + (target.rotation * defaultDistance);
        Vector3 curPos = Vector3.SmoothDamp(transform.position,toPos,)
    }
*/
}