using UnityEngine;
using ScriptableObjectArchitecture;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    //Variable References
    [Header("Game Variables")]
    [SerializeField] private IntVariable Var_Int_Money;

    [SerializeField] private IntVariable Var_Int_StackCapacity;
    [SerializeField] private FloatVariable Var_Float_PlayerSpeed;

    //Level References
    [Header("Level Variables")]
    [SerializeField] private IntVariable Level_Capacity;
    [SerializeField] private IntVariable Level_Speed;

    //Value References
    [Header("Value References")]
    [SerializeField] private IntCollection Values_Capacity;
    [SerializeField] private FloatCollection Values_Speed;

    //Price References
    [Header("Price References")]
    [SerializeField] private IntCollection Prices_Capacity;
    [SerializeField] private IntCollection Prices_Speed;

    //Default Price References
    [Header("Default Price References")]
    [SerializeField] private IntCollection Prices_Default_Capacity;
    [SerializeField] private IntCollection Prices_Default_Speed;


    //UI References
    [Header("UI References")]
    [SerializeField] private TextMeshPro tmpSpeed;
    [SerializeField] private TextMeshPro tmpCapacity;
    [SerializeField] private GameObject goSpeedUI;
    [SerializeField] private GameObject goCapacityUI;

    [SerializeField] private UpgradeTrigger speedTrigger;
    [SerializeField] private UpgradeTrigger capacityTrigger;


    ////UI Level Slots
    //[Header("UI Level Slots")]
    //[SerializeField] private GameObject[] capacitySlots = new GameObject[4];
    //[SerializeField] private GameObject[] playerSpeedSlots = new GameObject[4];
    //[SerializeField] private GameObject[] cashierSpeedSlots = new GameObject[4];
    //[SerializeField] private GameObject[] workerSpeedSlots = new GameObject[4];

    ////UI Buttons
    //[Header("UI Buttons")]
    //[SerializeField] private Button buttonUpgradeCapacity;
    //[SerializeField] private Button buttonUpgradeSpeed;
    //[SerializeField] private Button buttonUpgradeWorkerSpeed;
    //[SerializeField] private Button buttonUpgradeCashierSpeed;
    //[SerializeField] private Button buttonPurchaseCashier;

    ////UI Texts
    //[Header("UI Texts")]
    //[SerializeField] private TextMeshProUGUI tmpButtonCapacity;
    //[SerializeField] private TextMeshProUGUI tmpButtonSpeed;
    //[SerializeField] private TextMeshProUGUI tmpButtonCashierSpeed;
    //[SerializeField] private TextMeshProUGUI tmpButtonWorkerSpeed;
    //[SerializeField] private TextMeshProUGUI tmpButtonPurchaseCashier;

    //GameEvent
    [Header("Event References")]
    [SerializeField] private GameEvent eventMoneyUpdate;
    [SerializeField] private GameEvent eventWingStatusUpdate;

    //Setters
    public int _LevelCapacity
    {
        get => Level_Capacity;
        set
        {
            print("level capacity was : " + Level_Capacity);
            Level_Capacity.Value = value;
            Var_Int_StackCapacity.Value = Values_Capacity[_LevelCapacity];
            print("level capacity is now : " + Level_Capacity + "  and stack capacity is: " + Var_Int_StackCapacity);
            RefreshUI();
        }
    }

    public int _LevelSpeed
    {
        get => Level_Speed;
        set
        {   
            Level_Speed.Value=value;
            Var_Float_PlayerSpeed.Value = Values_Speed[_LevelSpeed];
            eventWingStatusUpdate.Raise();
            RefreshUI();
        }
    }

    private void OnEnable() => RefreshUI();

    //private bool CanPurchase(int amount) => Var_Int_Money >= amount;

    public void ResetUpgradeData()
    {
        _LevelCapacity = 0;
        _LevelSpeed = 0;
        for (int i = 0; i < Prices_Capacity.Count; i++)
        {
            Prices_Capacity[i] = Prices_Default_Capacity[i];
        }
        for (int i = 0; i < Prices_Speed.Count; i++)
        {
            Prices_Speed[i] = Prices_Default_Speed[i];
        }
        RefreshUI();
    }

    public void RefreshUI()
    {
        //TODO ADJUST UPGRADE ZONES 
        bool IsMaxedOut(int _level,BaseCollection _values) => _level >= _values.Count-1;

        //Remove maxed out buttons
        bool capacityMaxed = IsMaxedOut(_LevelCapacity, Values_Capacity);
        bool speedMaxed = IsMaxedOut(_LevelSpeed, Values_Speed);
        goCapacityUI.SetActive(!capacityMaxed);
        goSpeedUI.SetActive(!speedMaxed);
        if (!capacityMaxed) capacityTrigger.UpdateCostTextUI(capacityTrigger.UpgradePrice);
        if (!speedMaxed) speedTrigger.UpdateCostTextUI(speedTrigger.UpgradePrice);
        //Update button price texts

        //if (!IsMaxedOut(_LevelCapacity)) tmpButtonCapacity.text = "$ " + Prices_Capacity[_LevelCapacity + 1];
        //if (!IsMaxedOut(_LevelSpeed)) tmpButtonSpeed.text = "$ " + Prices_Speed[_LevelSpeed + 1];
        //if (!IsMaxedOut(_LevelCashierSpeed)) tmpButtonCashierSpeed.text = "$ " + Prices_CashierSpeed[_LevelCashierSpeed + 1];
        //if (!IsMaxedOut(_LevelWorkerSpeed)) tmpButtonWorkerSpeed.text = "$ " + Prices_WorkerSpeed[_LevelWorkerSpeed + 1];
        //if (!Var_Bool_CashierPurchased) tmpButtonPurchaseCashier.text = "$ " + Price_Cashier;

        //Update slots
        //for (int i = 0; i < 4; i++)
        //{
        //    capacitySlots[i].SetActive(i < _LevelCapacity);
        //    playerSpeedSlots[i].SetActive(i < _LevelSpeed);
        //    cashierSpeedSlots[i].SetActive(i < _LevelCashierSpeed);
        //    workerSpeedSlots[i].SetActive(i < _LevelWorkerSpeed);
        //}
    }

    private void SpendMoney(int _amount)
    {
        Var_Int_Money.Value =Var_Int_Money - _amount;
        eventMoneyUpdate.Raise();
    }

    public void TryUpgradeCapacity()
    {
        //SpendMoney(Prices_Capacity[Level_Capacity + 1]);
        _LevelCapacity++;
    }

    public void TryUpgradeSpeed()
    {
        //SpendMoney(Prices_Speed[Level_Speed + 1]);
        _LevelSpeed++;
    }
}