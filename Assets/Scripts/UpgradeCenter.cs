using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UpgradeCenter : MonoBehaviour
{
    [SerializeField]
    private UpgradeSystem upgradeSystem;

    [SerializeField]
    private Transform tUpgradeUI;

    [SerializeField]
    private PlayerController playerController;

    public bool canLaunchUpgradeUI = true;

    [SerializeField]
    private GameObject goObjectiveUI;

    public void ToggleDisplay(bool on)
    {
        goObjectiveUI.SetActive(on);
        foreach (var item in GetComponentsInChildren<MeshRenderer>())
        {
            item.enabled = on;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ToggleUpgradeUI(true);
    }

    public void ToggleUpgradeUI(bool on)
    {
        if (on && !canLaunchUpgradeUI) return;
        playerController.Cripple(on);
        if (on)
        {
            //upgradeSystem.ToggleDisplay(on);
            tUpgradeUI.localScale = Vector3.zero;
            tUpgradeUI.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);
        }
        else
        {
            canLaunchUpgradeUI = false;
            StartCoroutine(Cooldown(1));
            tUpgradeUI.DOScale(Vector3.zero, .3f).SetEase(Ease.InBack);//.OnComplete(() => upgradeSystem.ToggleDisplay(false));
        }
    }

    private IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        canLaunchUpgradeUI = true;
    }
}