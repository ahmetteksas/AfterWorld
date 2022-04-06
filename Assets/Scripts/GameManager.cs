using UnityEngine;
using UnityEngine.SceneManagement;
using MoreMountains.NiceVibrations;
using Lean.Touch;
using ScriptableObjectArchitecture;

[System.Serializable]
public class GameManager : Singleton<GameManager>
{
    //In Game Variables
    public int score;
    public int level = 1;
    public bool firstLaunch = true;

    //Level States
    public bool isGamePaused;
    bool GamePaused
    {
        set
        {
            isGamePaused = value;
            //FindObjectOfType<DrawManager>().paused = value;
        }
    }
    

    //UI
    [SerializeField] CanvasManager canvasManager;

    [SerializeField] private GameObject goGesture, goPauseButton, goDollarUI;

    public GameObject goUpgradeCenter;

    private void OnEnable() => LeanTouch.OnFingerDown += HandleFingerDown;
    private void OnDisable() => LeanTouch.OnFingerDown -= HandleFingerDown;

    private void HandleFingerDown(LeanFinger finger)
    {
        StartLevel();
        LeanTouch.OnFingerDown -= HandleFingerDown;
    }

    protected override void Awake()
    {
        foreach (var item in FindObjectsOfType<AudioListener>())
        {
            print(item.gameObject.name);
        }
        SRDebug.Init();
        base.Awake();
        if (canvasManager == null) canvasManager = CanvasManager.GetInstance();
    }


    public void StartLevel()
    {
        if (SceneManager.sceneCount > 1) SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
        goGesture.SetActive(false);
        goDollarUI.SetActive(true);
        goPauseButton.SetActive(true);
        score = 0;
        canvasManager.SwitchCanvas(CanvasType.GameUI);

        //TinySauce.OnGameStarted();
    }

    #region PauseResumeSystem
    public void Pause()
    {
        Time.timeScale = 0f;
        canvasManager.SwitchCanvas(CanvasType.PauseMenu);
        GamePaused = true;
    }

    public void PauseWithoutCanvasChange()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        canvasManager.SwitchCanvas(CanvasType.GameUI);
        GamePaused = false;
    }
    #endregion


    #region SaveLoadSystemMethods
    //public void SaveGame() => SaveSystem.Save(this);

    [SerializeField] private IntCollection Values_Int_TableStackCounts;
    [SerializeField] private IntCollection Values_Int_PurchaseZonePrices;
    [SerializeField] private IntVariable Var_Int_Money;
    [SerializeField] private GameObjectCollection StackList;
    [SerializeField] private GameEvent Event_MoneyUpdate;
    


    [Header("Initial Data")]
    [SerializeField] private int startMoney = 50;

    #endregion
    [SerializeField] Transform playerStartPoint;

    public void RestartLevel()
    {
        foreach (Customer c in FindObjectsOfType<Customer>())
        {
            c.StopAllCoroutines();
            c.paid = false;
            c.aiDestinationSetter.target = null;
            c.gameObject.SetActive(false);
        }
        foreach (var item in GameObject.FindGameObjectsWithTag("Stack"))
        {
            item?.SetActive(false);
        }
        for (int i = 0; i < Values_Int_TableStackCounts.Count; i++) //reset table stack count data
        {
            Values_Int_TableStackCounts[i] = i == 0 ? 2 : i==10 ? 0 : -1;
        }

        for (int i = 0; i < Values_Int_PurchaseZonePrices.Count; i++) //reset table stack count data
        {
            Values_Int_PurchaseZonePrices[i] = -1;
        }


        if (StackList.Count > 0)
        {
            for (int i = 0; i < StackList.Count; i++)
            {
                if (StackList[i].GetType() != typeof(GameObject)) continue;
                GameObject _goStack = StackList[i];
                if (_goStack == null) continue;
                _goStack.transform.SetParent(null);
                _goStack.transform.localScale = Vector3.one;
                _goStack.SetActive(false);
            }
        }

        StackList.Clear();


        Resume();
        FindObjectOfType<MapManager>().ResetMapData();
        FindObjectOfType<OnboardingManager>().ResetOnboarding();
        //FindObjectOfType<TableNail>().GiveInitialStacks(2);
        GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().SetTrigger("Idle");
        //.OnGameFinished(0);
        GameObject.FindGameObjectWithTag("Player").transform.position = playerStartPoint.position;
        FindObjectOfType<PlayerController>().Cripple(false);
        FindObjectOfType<UpgradeSystem>().ResetUpgradeData();
        //FindObjectOfType<DoorManager>().ResetDoor();
        FindObjectOfType<DeskManager>().ResetLineData();

        

        foreach (var item in FindObjectsOfType<TableBase>())
        {
            item.occupied = false;
        }
        Var_Int_Money.Value = startMoney;
        Event_MoneyUpdate.Raise();
        Debug.Log("Player data reset complete.");
    }

    
    public bool hapticOn;
    
    [SerializeField] GameObject hapticOnPause, hapticOffPause;

    public void ToggleHapticOnOff()
    {
        hapticOn = !hapticOn;
        MMVibrationManager.SetHapticsActive(hapticOn);Var_Int_Money.Value = startMoney;
        Event_MoneyUpdate.Raise();
        hapticOnPause.SetActive(hapticOn);
        hapticOffPause.SetActive(!hapticOn);
    }
}