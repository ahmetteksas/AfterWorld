using DG.Tweening;
using UnityEngine;

public class ScalableUI : MonoBehaviour
{
    private bool isMagnified = false;
    Tweener tweenerScaleX;
    Tweener tweenerScaleZ;
    public Vector3 defaultScale = Vector3.zero;

    [SerializeField] float scaleMultiplier = 1.5f;

    public virtual void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void ToggleScale(bool magnify)
    {
        if (magnify == isMagnified) return;
        isMagnified = magnify;
        TryCancelScaling();
        
        //if (magnify) ToggleHovering(false);
        tweenerScaleX = transform.DOScaleX(defaultScale.x * (magnify ? scaleMultiplier : 1f), .3f).SetEase(Ease.InOutSine).OnComplete(() => {
            tweenerScaleX = null;
        });
        tweenerScaleZ = transform.DOScaleZ(defaultScale.z * (magnify ? scaleMultiplier : 1f), .3f).SetEase(Ease.InOutSine).OnComplete(() => {
            tweenerScaleZ = null;
        });
    }

    private void TryCancelScaling()
    {
        if (tweenerScaleX != null)
        {
            tweenerScaleX.Kill();
            tweenerScaleX = null;
        }
        if (tweenerScaleZ != null)
        {
            tweenerScaleZ.Kill();
            tweenerScaleZ = null;
        }
    }
}