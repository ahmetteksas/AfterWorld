using UnityEngine;
using TMPro;
using DG.Tweening;

public class DollarTextUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmpDollarText;

    [SerializeField]
    private float moveDistanceY = 3f;

    [SerializeField]
    private float moveDuration = 1f;

    [SerializeField]
    private float fadeDuration = .4f;

    [SerializeField]
    private float fadeAmount = .2f;

    private Vector3 defaultScale;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void Process(int amount)
    {
        transform.DOKill();
        transform.localScale = defaultScale;
        tmpDollarText.DOFade(1, 0);
        tmpDollarText.text = "$" + amount.ToString();
        transform.DOScale(transform.localScale * 1.1f, .1f).SetLoops(2, LoopType.Yoyo);
        transform.DOMoveY(transform.position.y + moveDistanceY, moveDuration).SetEase(Ease.OutSine).OnComplete(() => gameObject.SetActive(false));
        tmpDollarText.DOFade(fadeAmount, fadeDuration).SetDelay(moveDuration - fadeDuration);
    }
}