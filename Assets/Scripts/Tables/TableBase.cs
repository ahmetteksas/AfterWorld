using System.Collections;
using UnityEngine;

public abstract class TableBase : MonoBehaviour
{
    public float frequencyMin = 3f, frequencyMax = 7f;

    public bool occupied;

    public float nailWorkDuration = 3f;

    public Transform employeePos;
    public Transform npcPos;

    public Employee employee;

    public Transform employeeCallPoint;

    public Transform customerCallPoint;

    public Transform moneyPlacePoint;

    public DeskManager deskManager;

    [SerializeField]
    private GameObject premadeEmployee;

    [HideInInspector]
    public bool employeePresent;


    public virtual void Awake()
    {
        if (deskManager==null) deskManager = FindObjectOfType<DeskManager>();
        employeePresent = premadeEmployee != null;
    }

    public virtual void Start()
    {   
        ReadyForCustomers = true;
        if (employeePresent)
        {
            employee = premadeEmployee.GetComponent<Employee>();
            employee.animator.SetTrigger("PedicureIdle");
        }
    }

    private bool readyForCustomers = false;
    public bool ReadyForCustomers
    {
        get=> readyForCustomers;

        set
        {
            readyForCustomers = value;
            if (value && callCustomersCoroutine == null)
            {
                callCustomersCoroutine = StartCoroutine(CallCustomersCoroutine());
            }
        }
    }

    public Coroutine callCustomersCoroutine;
    public abstract IEnumerator CallCustomersCoroutine();
}