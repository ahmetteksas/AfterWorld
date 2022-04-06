using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Employee : NPCBase
{
    public override IEnumerator CallToTableCoroutine(TableBase table)
    {
        Move(table.employeeCallPoint);
        while (!aiPath.reachedEndOfPath)
        {
            yield return null;
        }
        Stop();
        Transform sitTarget = table.employeePos;
        GetComponent<Collider>().enabled = false;
        animator.SetTrigger("PedicureIdle");
        transform.DORotateQuaternion(sitTarget.rotation, sitAnimDuration / 6);
        yield return transform.DOMove(sitTarget.position, sitAnimDuration / 3).WaitForCompletion(); //sit
        table.ReadyForCustomers = true;
    }
}