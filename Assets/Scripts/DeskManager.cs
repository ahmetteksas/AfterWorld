using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ScriptableObjectArchitecture;
using DG.Tweening;
using System.Collections;

public class DeskManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> linePoints = new List<Transform>();

    private Queue<Customer> line = new Queue<Customer>(); //This line is used for position allocation

    private HashSet<Customer> actualLine = new HashSet<Customer>(); //This line is checked to collect money

    [SerializeField]
    private IntVariable Var_Int_LineCapacity;

    [SerializeField]
    private Camera mainCamera;

    public bool CanSendNewCustomer => line.Count < Var_Int_LineCapacity;

    public bool IsActualLineEmpty => actualLine.Count == 0;

    [SerializeField]
    private NPCManager npcManager;

    [SerializeField]
    private int minStartCount = 3;

    [SerializeField]
    private int maxStartCount = 7;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        if (npcManager == null) npcManager = FindObjectOfType<NPCManager>();
    }

    private void Start()
    {
        StartCoroutine(PopulateLine());
    }

    private void InitializeLine()
    {
        int _count = Random.Range(minStartCount, maxStartCount);
        for (int i = 0; i < _count; i++)
        {
            TryCallDeadToLine();
        }
    }

    WaitForSeconds _wait = new WaitForSeconds(1f);
    private IEnumerator PopulateLine()
    {
        InitializeLine();
        while (true)
        {
            yield return _wait;
            TryCallDeadToLine();
        }
    }

    private void TryCallDeadToLine()
    {
        if (line.Count >= Var_Int_LineCapacity) return;
        Customer _customer = npcManager.RequestDead();
        Transform _pointInLine = GetInLine(_customer);
        _customer.Move(_pointInLine, () =>
        {
            transform.DORotateQuaternion(_pointInLine.rotation, .3f);
            GetInActualLine(_customer);
            _customer.animator.SetTrigger("Idle");
        }
        );
    }

    public Transform GetInLine(Customer _customer)
    {
        line.Enqueue(_customer);
        int _index = line.ToList().IndexOf(_customer); 
        return linePoints[_index];
    }

    public void GetInActualLine(Customer _customerToAdd) //Called when NPC gets in line
    {
        if (actualLine.Contains(_customerToAdd)) return;
        actualLine.Add(_customerToAdd);
    }

    public void LeaveActualLine(Customer _customerToRemove)
    {
        if (!actualLine.Contains(_customerToRemove)) return;
        actualLine.Remove(_customerToRemove);
    }

    public void ResetLineData()
    {
        actualLine.Clear();
        line.Clear();
    }

    public Customer PopQueue()
    {
        Customer _poppedCustomer = line.Dequeue();
        LeaveActualLine(_poppedCustomer);
        List<Customer> customerList = line.ToList();
        for (int i = 0; i < customerList.Count; i++)
        {
            Customer _c = customerList[i];
            if (_c.paid) continue;
            _c.Move(linePoints[i],()=>_c.Stop(()=>_c.animator.SetTrigger("Idle")));
        }
        return _poppedCustomer;
    }
}