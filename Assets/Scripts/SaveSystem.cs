using ScriptableObjectArchitecture;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField]
    private GameObjectCollection StackList;

    [SerializeField]
    private IntCollection Values_PurchaseZonePrices;

    [SerializeField]
    private IntCollection Values_TableStackCounts;

    [SerializeField]
    private IntVariable Var_Int_MapLevel;

    [SerializeField]
    private IntVariable Var_Int_Money;

    [SerializeField]
    private IntVariable Var_Int_OnboardingState;

    [SerializeField]
    private FloatVariable Var_Float_PlayerSpeed;

    [SerializeField]
    private IntVariable Var_Int_StackCapacity;

    [SerializeField]
    private IntVariable Level_Capacity;

    [SerializeField]
    private IntVariable Level_Speed;

    [SerializeField]
    private GameEvent Event_MoneyUpdate;
    [SerializeField]
    private GameEventBase Event_StackCountUpdate;
    [SerializeField]
    private GameEvent Event_MapUpdate;

    private void Awake()
    {
        Load();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }

    private void OnApplicationQuit()
    {
        Save();
    }



    private void Load()
    {
        SaveData loadData = ES3.Load("Data", new SaveData()) ;
        bool isFirstLaunch = loadData._Var_Int_OnboardingState == 1;
        if (!isFirstLaunch) ApplyLoadData(loadData);
        Debug.Log(isFirstLaunch ? "First Launch!" : "Loaded");
    }

    private void Save()
    {
        ES3.Save("Data", CreateSaveData());
        Debug.Log("Saved");
    }

    private SaveData CreateSaveData()
    {
        foreach (var item in FindObjectsOfType<StackTable>())
        {
            Values_TableStackCounts[item.tableIndex] = item.FullStackCount;
        }


        SaveData saveData = new SaveData();
        saveData.stackCount = StackList.Count;
        saveData._Values_PurchaseZonePrices = Values_PurchaseZonePrices.ToArray();
        saveData._Values_TableStackCounts = Values_TableStackCounts.ToArray();
        saveData._Var_Int_MapLevel = Var_Int_MapLevel.Value;
        saveData._Var_Int_Money = Var_Int_Money.Value;
        saveData._Var_Int_OnboardingState = Var_Int_OnboardingState.Value;
        saveData._Var_Int_StackCapacity = Var_Int_StackCapacity.Value;
        saveData._Level_Capacity = Level_Capacity.Value;
        saveData._Level_Speed = Level_Speed;
        return saveData;
    }

    private void ApplyLoadData(SaveData saveData)
    {
        StackList.Clear();
        FindObjectOfType<StackManager>().LoadFromSave(saveData.stackCount);
        for (int i = 0; i < saveData._Values_PurchaseZonePrices.Length; i++)
        {
            Values_PurchaseZonePrices[i]=(saveData._Values_PurchaseZonePrices[i]);
        }
        for (int i = 0; i < saveData._Values_TableStackCounts.Length; i++)
        {
            Values_TableStackCounts[i]=(saveData._Values_TableStackCounts[i]);
        }
        Var_Int_MapLevel.Value = saveData._Var_Int_MapLevel;
        Var_Int_Money.Value = saveData._Var_Int_Money;
        Var_Int_OnboardingState.Value = saveData._Var_Int_OnboardingState;
        FindObjectOfType<OnboardingManager>().LoadOnboardedGameobjects();
        Var_Float_PlayerSpeed.Value = saveData._Var_Float_PlayerSpeed;
        Var_Int_StackCapacity.Value = saveData._Var_Int_StackCapacity;
        Level_Capacity.Value = saveData._Level_Capacity;
        Level_Speed.Value = saveData._Level_Speed;
        Event_MoneyUpdate.Raise();
        //Event_MapUpdate.Raise();
        Event_StackCountUpdate.Raise();
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Load();
            print(Values_PurchaseZonePrices.ToArray().Length);
            print(Values_PurchaseZonePrices.ToArray()[0]);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < 5; i++)
            {
                StackList.Add(new GameObject());
            }
        }

        if (Input.GetKeyDown(KeyCode.J)) Save();
        if (Input.GetKeyDown(KeyCode.K)) Load();
    }
#endif
}

[ES3Serializable]
public class SaveData
{
    public int stackCount=0;
    public int[] _Values_PurchaseZonePrices;
    public int[] _Values_TableStackCounts;
    public int _Var_Int_MapLevel;
    public int _Var_Int_Money;
    public int _Var_Int_OnboardingState;
    public float _Var_Float_PlayerSpeed;
    public int _Var_Int_StackCapacity;
    public int _Level_Capacity;
    public int _Level_Speed;
}