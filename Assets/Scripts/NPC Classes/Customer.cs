using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using ScriptableObjectArchitecture;

public class Customer : NPCBase
{
    public List<GameObject> models = new List<GameObject>();
    [SerializeField]
    private FloatVariable Var_Float_WorkerSpeed;

    [SerializeField]
    private GameObject goImps;

    public bool paid = false; 

    private int modelType;

    [SerializeField]
    private float riseUpDistanceY = 5f;

    [SerializeField]
    private float moneyPlaceRandomizationFactor = 2f;

    [SerializeField]
    private CutoffEffect cutoffEffect;

    private void OnEnable()
    {
        modelType = Random.Range(0, models.Count);
        paid = false;
    }

    private void ToggleImps(bool on)=> goImps.SetActive(on);

    void Start()
    {
        for (int i = 0; i < models.Count; i++)
            models[i].SetActive(i==modelType);
    }

    [SerializeField]
    private float sadReactionDuration = 1f;
    
    public override IEnumerator CallToTableCoroutine(TableBase table)
    {
        animator.SetTrigger("Sad Reaction");
        yield return new WaitForSeconds(sadReactionDuration);
        ToggleImps(true);
        animator.SetTrigger("Fly");

        Move(table.customerCallPoint);
        while (!aiPath.reachedDestination)
        {
            ToggleAIPath(true);
            yield return null;
        }
        Stop();
        Transform _npcPoint = table.npcPos;
        GetComponent<Collider>().enabled = false;

        transform.DORotateQuaternion(_npcPoint.rotation, sitAnimDuration / 6);
        bool _employeePresent = table.employeePresent;
        Animator _animEmployee = _employeePresent? table.employee.animator :null;

        ToggleImps(false);
        TableObject _hellObject = table as TableObject;
        yield return transform.DOMove(_npcPoint.position, sitAnimDuration / 3).WaitForCompletion(); //sit
        animator.SetTrigger(_hellObject.animName);
        _animEmployee?.SetTrigger("Work"); 
        _hellObject.SpendStack();
        float punishmentDuration = _hellObject.nailWorkDuration;
        float t = 0;
        _hellObject.ToggleProgressBar(true);
        while (t < punishmentDuration)
        {
            t += Time.deltaTime;
            _hellObject.UpdateProgressBar(t / punishmentDuration);
            yield return null;
        }
        _hellObject.ToggleExclamation();
        _hellObject.UpdateProgressBar(1);
        _hellObject.ToggleProgressBar(false, 2);
        

        _animEmployee?.SetTrigger("WorkIdle");//??
        //if (table.GetType() == typeof(StackTable)) (table as StackTable).ToggleExclamation();
        table.occupied = false;
        animator.SetTrigger("Fly");
        transform.DOMoveY(transform.position.y + riseUpDistanceY, 3.2f).SetEase(Ease.InSine);
        yield return cutoffEffect.Dissolve(1f, 2f, ()=>{
            paid = true;
            PlaceMoney(table);
            Dispose();
        });
        
    }




    private void PlaceMoney(TableBase table)
    => ObjectPool.instance.SpawnFromPool("Dollar", table.moneyPlacePoint.position + new Vector3(Random.Range(-moneyPlaceRandomizationFactor, moneyPlaceRandomizationFactor), 0, Random.Range(-moneyPlaceRandomizationFactor, moneyPlaceRandomizationFactor)), Quaternion.identity);

    public void Dispose()
    {
        if (!paid) return;
        paid = false;
        animator.SetTrigger("Idle");
        gameObject.SetActive(false);
    }
}