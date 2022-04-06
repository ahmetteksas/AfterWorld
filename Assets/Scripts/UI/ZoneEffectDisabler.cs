using UnityEngine;

public class ZoneEffectDisabler : MonoBehaviour
{
    public void EndEffect()
    {
        gameObject.SetActive(false);
    }
}