using UnityEngine;

public class ZoneEffect : MonoBehaviour
{
    [SerializeField]
    private GameObject goEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        goEffect.SetActive(true);
    }
}