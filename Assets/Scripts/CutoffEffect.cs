using System;
using System.Collections;
using UnityEngine;

public class CutoffEffect : MonoBehaviour
{
    [SerializeField] Renderer stickfighterRenderer;
    //[SerializeField] Material killMaterial;
    [SerializeField] float cutoffStartValue = 0f;
    [SerializeField] float cutoffEndValue = -5f;

    [SerializeField]
    private Material matDissolve;

    void Start()
    {
        if (stickfighterRenderer == null) stickfighterRenderer = GetComponentInChildren<Renderer>();
    }

    public IEnumerator Dissolve(float delay, float duration, Action onComplete = null)
    {
        yield return new WaitForSeconds(delay);
        //stickfighterRenderer.material = killMaterial;
        Material cachedMat = stickfighterRenderer.sharedMaterial;
        stickfighterRenderer.material = matDissolve;
        Material mat = stickfighterRenderer.material;
        float t = 0;
        float initialValue = cutoffStartValue == 0 ? mat.GetFloat("_Cutoff_Height") : cutoffStartValue;
        while (t < duration)
        {
            t += Time.fixedDeltaTime;
            mat.SetFloat("_Cutoff_Height", Mathf.Lerp(initialValue, cutoffEndValue, t / duration));
            yield return new WaitForFixedUpdate();
        }
        mat.SetFloat("_Cutoff_Height", Mathf.Lerp(initialValue, cutoffEndValue, 1));
        stickfighterRenderer.material = cachedMat;
        onComplete?.Invoke();
    }
}
