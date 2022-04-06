using UnityEngine;

public class RotationFixer : MonoBehaviour
{
    private void OnEnable()=> transform.LookAt(transform.position + Vector3.forward);
}