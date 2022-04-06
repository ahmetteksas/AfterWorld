using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEditor;
using UnityEngine;

public abstract class StackTable : TableBase
{
    public TextMeshProUGUI tmpStackCount;

    public GameObject goStackCountUI;

    [SerializeField]
    private IntCollection Values_Int_TableStackCounts;

    public Dictionary<int, StackDisplay> stacks = new Dictionary<int, StackDisplay>();

    public List<Transform> stackPoints = new List<Transform>();

    public Transform stackSpendPoint;
    private Vector3 defaultStackCountUIScale;

    [SerializeField]
    private int stackCapacity = 4;

    [SerializeField]
    private FloatVariable stackSizeModifier;

    public int tableIndex = -1;

    private bool IsFirstTable => tableIndex == 0;

    [SerializeField]
    private GameObject goActive;

    [SerializeField]
    private GameObject goPassive;

    public override void Awake()
    {
        base.Awake();
        if (tmpStackCount == null) tmpStackCount = GetComponentInChildren<TextMeshProUGUI>();
        defaultStackCountUIScale = goStackCountUI.transform.localScale;
    }

    public virtual void OnValidate()
    {
        if (PrefabUtility.GetPrefabInstanceStatus(gameObject) != PrefabInstanceStatus.NotAPrefab) return;
        if (tableIndex == -1)
        {
            PurchaseZone[] purchaseZones = FindObjectsOfType<PurchaseZone>();
            int highestIndex = 0;
            foreach (PurchaseZone v in purchaseZones)
            {
                if (v.zoneIndex > highestIndex) highestIndex = v.zoneIndex;
            }
            tableIndex = highestIndex + 1;
            if (Values_Int_TableStackCounts.Count < tableIndex + 1) Values_Int_TableStackCounts.Add(-1);
        }
    }

    public override void Start()
    {
        base.Start();
        TryInitializeStacks();
        ToggleStackCountUI(false, true);
        ToggleExclamation();
        GiveInitialStacks(Values_Int_TableStackCounts[tableIndex]);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (debug) print(other.tag + " " + ReadyForCustomers);
        if (!other.CompareTag("Player") ||  !ReadyForCustomers) return;
        TryToggleReceiveStacks(other.GetComponent<StackManager>());
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || !ReadyForCustomers) return;
        TryToggleReceiveStacks();
    }

    [SerializeField] bool debug;

    private void TryToggleReceiveStacks(StackManager _stackManager) //starts
    {
        if (receiveStacksCoroutine != null) return;
        ToggleStackCountUI(true);
        receiveStacksCoroutine = StartCoroutine(ReceiveStacksCoroutine(_stackManager));
    }

    private void TryToggleReceiveStacks() //stops
    {
        ToggleStackCountUI(false);
        if (receiveStacksCoroutine == null) return;
        StopCoroutine(receiveStacksCoroutine);
        receiveStacksCoroutine = null;
    }

    WaitForSeconds _receiveFq = new WaitForSeconds(.05f);

    private bool IsStackFull => FullStackCount >= stacks.Keys.Count;

    public bool IsStackEmpty => !stacks.Values.Any(e => e != null);

    public int FullStackCount => stacks.Values.Where(e => e != null).Count();

    private int NextStackIndex => stacks.First(e => e.Value == null).Key;

    Coroutine receiveStacksCoroutine;
    private IEnumerator ReceiveStacksCoroutine(StackManager _stackManager)
    {
        while (!IsStackFull)
        {
            ReceiveStack(_stackManager.SendStack());
            yield return _receiveFq;
        }
    }

    //private void OnApplicationPause(bool pause)
    //{
    //    Values_Int_TableStackCounts[tableIndex]=FullStackCount;
    //    print("paused with stack count : " + stacks.Count + "  and so data: " + Values_Int_TableStackCounts[tableIndex]);
    //}

    //private void OnApplicationQuit(bool pause)
    //{
    //    Values_Int_TableStackCounts[tableIndex] = FullStackCount;
    //    print("quit with stack count : " + stacks.Count + "  and so data: " + Values_Int_TableStackCounts[tableIndex]);
    //}

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (!focus) Values_Int_TableStackCounts[tableIndex] = FullStackCount;
    //    print("focus with stack count : " + stacks.Count + "  and so data: " + Values_Int_TableStackCounts[tableIndex]);
    //}

    //private void OnDisable()
    //{
    //    Values_Int_TableStackCounts[tableIndex] = FullStackCount;
    //    Debug.Log("OnDisable on StackTable");
    //}



    public void ReceiveStack(StackDisplay _stackToReceive,bool instant=false)
    {
        if (_stackToReceive == null) return;
        Transform _t = _stackToReceive.transform;
        int _index = NextStackIndex;
        _t.SetParent(stackPoints[_index]);
        if (instant)
        {
            _t.localScale = Vector3.one * stackSizeModifier;
            _t.localPosition = Vector3.zero;
            _t.localRotation = Quaternion.identity;
        }
        else
        {
            _t.DOScale(Vector3.one * stackSizeModifier, .3f);
            _t.DOLocalJump(Vector3.zero, 1, 1, .3f).SetEase(Ease.InOutSine);
            _t.DOLocalRotate(Vector3.zero, .3f);
        }
        stacks[_index] = _stackToReceive;
        ToggleExclamation();
        UpdateStackCountUI();
    }

    public GameObject SpendStack()
    {
        KeyValuePair<int, StackDisplay> kp = stacks.Last(e => e.Value != null);

        int _index = kp.Key;
        Transform _t = stacks[_index].transform;
        stacks[_index] = null;
        _t.DOScale(Vector3.zero, .7f).SetEase(Ease.OutSine).OnComplete(() => { _t.localScale = Vector3.one;_t.SetParent(null); _t.gameObject.SetActive(false); });
        _t.DOMove(stackSpendPoint.position, .7f).SetEase(Ease.InOutSine);
        _t.DORotateQuaternion(stackSpendPoint.rotation, .7f).SetEase(Ease.InOutSine);
        UpdateStackCountUI();
        return _t.gameObject;
    }

    public GameObject goExclamation;

    public void ToggleExclamation()
    {
        bool _show = IsStackEmpty;
        goActive.SetActive(!_show);
        goPassive.SetActive(_show);
        goExclamation.SetActive(_show);

        if (_show) goExclamation.transform.DOShakeScale(.2f);
    }

    private void TryInitializeStacks(bool force = false)
    {
        if (!force && stacks.Count >= stackCapacity) return;
        for (int i = 0; i < stackCapacity; i++)
        {
            stacks[i] = null;
            if (stackPoints[i].childCount > 0)
            {
                Transform _t = stackPoints[i].GetChild(0);
                _t.SetParent(null);
                _t.gameObject.SetActive(false);
            }
        }
    }

    public void GiveInitialStacks(int amount)
    {
        TryInitializeStacks(force: true);
        if (debug) print(amount);
        int _cachedAmount = Values_Int_TableStackCounts[tableIndex]; //SO Collection holds values 0 to 4 if purchased, else -1
        amount = _cachedAmount >= 0 ? _cachedAmount : amount;
        if (_cachedAmount == -1) Values_Int_TableStackCounts[tableIndex] = 0;
        for (int i = 0; i < amount; i++)
        {
            if (debug) print(i);
            StackDisplay _stack = ObjectPool.instance.SpawnFromPool("Stack", stackPoints[i].position, Quaternion.identity).GetComponent<StackDisplay>();
            ReceiveStack(_stack, true);
        }
        if (callCustomersCoroutine != null)
        {
            StopCoroutine(callCustomersCoroutine);
            callCustomersCoroutine = null;
        }
        ReadyForCustomers = true;
    }

    private void ToggleStackCountUI(bool on,bool instant=false)
    {
        if (disableStackCountUI != null)
        {
            StopCoroutine(disableStackCountUI);
            disableStackCountUI = null;
        }
        if (on)
        {
            //goStackCountUI.SetActive(on);
            Transform _t = goStackCountUI.transform;
            _t.DOKill();
            _t.localScale = defaultStackCountUIScale;
            UpdateStackCountUI();
        }
        else
        {
            if (instant)
                goStackCountUI.transform.localScale = Vector3.zero;
            else
                disableStackCountUI = StartCoroutine(DisableStackCountUI());
        }
    }

    Coroutine disableStackCountUI;
    private IEnumerator DisableStackCountUI()
    {
        yield return new WaitForSeconds(1.5f);
        Transform _t = goStackCountUI.transform;
        _t.DOKill();
        _t.DOScale(defaultStackCountUIScale * 1.1f, .1f).OnComplete(() => _t.DOScale(Vector3.zero, .2f));
        //_t.DOScale(defaultStackCountUIScale * 1.1f, .1f).OnComplete(() => _t.DOScale(Vector3.zero, .2f).OnComplete(() => goStackCountUI.SetActive(false)));
    }

    private void UpdateStackCountUI()
    => tmpStackCount.text = FullStackCount + "/" + stacks.Keys.Count;
}