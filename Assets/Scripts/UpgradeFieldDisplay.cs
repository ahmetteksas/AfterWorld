using UnityEngine;
using DG.Tweening;

public class UpgradeFieldDisplay : PurchaseZone
{
    [SerializeField]
    private GameObject goHellObject;


    public override int UpgradePrice
    {
        get => upgradePrice;
        set
        {
            if (value < 0) return;
            upgradePrice = value;
            UpdateCostTextUI(value);
            if (upgradePrice <= 0 && !purchased) Purchase(false);
        }
    }

    public override void Awake()
    {
        base.Awake();
        goHellObject.SetActive(false);
    }

    public override void OnValidate()
    {
        if (goHellObject == null) return;
        base.OnValidate();
    }

    public override void Purchase(bool isLoad=false,int _stackCount=0)
    {
        base.Purchase(isLoad);
        goHellObject.SetActive(true);
        if (!isLoad)
        {
            Transform _t = goHellObject.transform;
            _t.DOScale(_t.localScale * 1.2f, .15f).SetLoops(2, LoopType.Yoyo);
            
            TableBase _table = goHellObject.GetComponent<TableBase>();
            if (_table.GetType() == typeof(TableObject))
                (_table as StackTable).GiveInitialStacks(2);
        }
        else
        {   
            goHellObject.GetComponent<StackTable>()?.GiveInitialStacks(_stackCount);
            TurnOnTableCollider();
            FindObjectOfType<AstarPath>().Scan();
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        if (!other.CompareTag("Player")) return;
        if (purchased)
        {
            TurnOnTableCollider();
        }
    }

    private void TurnOnTableCollider()=> goHellObject.GetComponent<Collider>().enabled = true;
}