using UnityEngine;

public class Locater : MonoBehaviour
{
    [SerializeField]
    private Color color = Color.magenta;

    [SerializeField]
    private float radius = .5f;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}