using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using ScriptableObjectArchitecture;
using UnityEditor;

public abstract class PurchaseZone : MonoBehaviour
{
    public TextMeshPro costText;

    public bool purchased;
    [SerializeField] GameObject goUI;
    [SerializeField] ScalableUI scalableUI;

    public GameObject goPurchaseVFX;

    [SerializeField] private IntCollection Values_PurchaseZonePrices;

    public int zoneIndex = -1;

    public int defaultPrice=100;

    public virtual int upgradePrice
    {
        get=> Values_PurchaseZonePrices[zoneIndex];
        set => Values_PurchaseZonePrices[zoneIndex] = value;
    }    

    public virtual int UpgradePrice
    {
        get => upgradePrice;
        set
        {
            if (value < 0) return;
            upgradePrice = value;
            UpdateCostTextUI(value);
            if (upgradePrice == 0 && !purchased) Purchase();
        }
    }

    public virtual void OnValidate()
    {
        if (PrefabUtility.GetPrefabInstanceStatus(gameObject)!=PrefabInstanceStatus.NotAPrefab) return;
        if (zoneIndex == -1)
        {
            PurchaseZone[] purchaseZones = FindObjectsOfType<PurchaseZone>();
            int highestIndex = 0;
            foreach (PurchaseZone v in purchaseZones)
            {
                if (v.zoneIndex > highestIndex) highestIndex = v.zoneIndex;
            }
            zoneIndex = highestIndex + 1;
            if (Values_PurchaseZonePrices.Count < zoneIndex+1) Values_PurchaseZonePrices.Add(-1);
        }
    }

    public virtual void Purchase()
    {
        purchased = true;
        goUI.SetActive(false);
        TryEndReceivingMoney();
        if (coroutineCheckIfStops!=null) StopCoroutine(coroutineCheckIfStops);
        coroutineCheckIfStops = null;
        FindObjectOfType<AstarPath>().Scan();
    }

    public virtual void Purchase(bool isLoad = false,int _stackCount=0)
    {
        Purchase();
        if (!isLoad && goPurchaseVFX!=null) goPurchaseVFX.SetActive(true);
    }

    public void UpdateCostTextUI(int value) => costText.text = "$ " + (value >= 0 ? value.ToString() : "0");

    [SerializeField]
    private SpriteRenderer outline;
    private Color defaultColor;
    [SerializeField]
    private Color focusColor = new Color(0.9882353f, 0.4039216f, 1);

    public virtual void Awake()
    {
        defaultColor = outline.color;
        if (scalableUI == null) scalableUI = goUI?.GetComponent<ScalableUI>();
    }

    public void TryResetPriceData()
    {
        if (UpgradePrice < 0) UpgradePrice = defaultPrice;
    }

    private void Start()
    {
        
        if (UpgradePrice == 0) Purchase(true);
        TryResetPriceData();
        UpdateCostTextUI(upgradePrice);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        ToggleColor(true);
        coroutineCheckIfStops = StartCoroutine(CheckIfStops(other.GetComponent<PlayerController>()));
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        print("debug");
        StopTransactions();
        if (purchased)
        {   
            GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
    }

    public void StopTransactions()
    {
        ToggleColor(false);
        StopAllCoroutines();
        coroutineCheckIfStops = null;
        TryEndReceivingMoney();
        MMVibrationManager.StopContinuousHaptic(true);
    }

    public void ToggleColor(bool on)
    {
        outline.DOKill();
        scalableUI?.ToggleScale(on);
        outline.DOColor(on ? focusColor : defaultColor, .3f);
    }


    WaitForFixedUpdate _wait = new WaitForFixedUpdate();

    public Coroutine coroutineCheckIfStops;
    private IEnumerator CheckIfStops(PlayerController playerController)
    {
        while (true)
        {
            yield return _wait;
            if (!playerController.IsMoving)
            {
                if (tweenReceiveMoney != null) continue;
                MoneyManager moneyManager = playerController.GetComponent<MoneyManager>();
                if (moneyManager.CurrentMoney <= 0) continue;

                int _difference = moneyManager.CurrentMoney - UpgradePrice;
                if (_difference >= 0)
                {
                    float _duration = Mathf.Min((float)UpgradePrice / 100f, .8f);
                    MMVibrationManager.ContinuousHaptic(0.05f, 0.21f, _duration, HapticTypes.None, this);
                    tweenReceiveMoney = DOTween.To(() => moneyManager.CurrentMoney, x => moneyManager.CurrentMoney = x, _difference, _duration).OnKill(() => tweenReceiveMoney = null);
                    tweenReducePrice = DOTween.To(() => UpgradePrice, x => UpgradePrice = x, 0, _duration).OnKill(() => tweenReducePrice = null);
                }
                else
                {
                    _difference = Mathf.Abs(_difference);
                    float _duration = Mathf.Min((float)_difference / 100f, .5f);
                    MMVibrationManager.ContinuousHaptic(0.05f, 0.21f, _difference, HapticTypes.None, this);
                    tweenReceiveMoney = DOTween.To(() => moneyManager.CurrentMoney, x => moneyManager.CurrentMoney = x, 0, _duration).OnKill(()=>tweenReceiveMoney=null);
                    tweenReducePrice = DOTween.To(() => UpgradePrice, x => UpgradePrice = x, _difference, _duration).OnKill(() => tweenReducePrice = null);
                }
            }
            else
                TryEndReceivingMoney();
        }
    }

    private Tweener tweenReceiveMoney;
    private Tweener tweenReducePrice;

    public void TryEndReceivingMoney()
    {
        if (tweenReceiveMoney != null) tweenReceiveMoney.Kill();
        if (tweenReducePrice != null) tweenReducePrice.Kill();
    }
}