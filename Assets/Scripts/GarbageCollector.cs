using UnityEngine;

public class GarbageCollector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Npc")) return;
        other.GetComponent<Customer>()?.Dispose();
    }
}