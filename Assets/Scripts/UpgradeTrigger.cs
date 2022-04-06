using ScriptableObjectArchitecture;
using UnityEngine;

public class UpgradeTrigger : PurchaseZone
{
    [SerializeField]
    private bool isSpeed = false;

    [SerializeField]
    private UpgradeSystem upgradeSystem;

    [SerializeField]
    private IntCollection Prices_Upgrade;

    [SerializeField]
    private GameEvent Event_MoneyUpdate;

    public override void Purchase()
    {
        StopTransactions();
        if (isSpeed)
        {
            upgradeSystem.TryUpgradeSpeed();
        }
        else
        {
            upgradeSystem.TryUpgradeCapacity();
        }
        if (goPurchaseVFX!=null) goPurchaseVFX.SetActive(true);
    }


    public override int upgradePrice
    {
        get => Prices_Upgrade[isSpeed ? upgradeSystem._LevelSpeed +1 : upgradeSystem._LevelCapacity + 1];
        set => Prices_Upgrade[isSpeed ? upgradeSystem._LevelSpeed + 1 : upgradeSystem._LevelCapacity + 1] = value;
    }

    public override int UpgradePrice
    {
        get => upgradePrice;
        set
        {
            Event_MoneyUpdate.Raise();
            if (value < 0) return;
            upgradePrice = value;
            if (upgradePrice == 0) Purchase();
            UpdateCostTextUI(UpgradePrice);
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        StopTransactions();
    }

    public override void OnValidate()
    {
        
    }
}