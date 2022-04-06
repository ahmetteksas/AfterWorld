using UnityEngine;
using Pathfinding;
using System.Collections;
using System;

public abstract class NPCBase : MonoBehaviour
{
    public Animator animator;

    public AIPath aiPath;

    public AIDestinationSetter aiDestinationSetter;

    public const float sitAnimDuration = 3f;

    public void CallToTable(TableBase table) => StartCoroutine(CallToTableCoroutine(table));

    public abstract IEnumerator CallToTableCoroutine(TableBase table);

    private void OnValidate()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (aiPath== null) aiPath= GetComponent<AIPath>();
        if (aiDestinationSetter== null) aiDestinationSetter = GetComponent<AIDestinationSetter>();
    }

    public void ToggleAIPath(bool on) => aiPath.enabled = on;
    public void Stop(Action action=null)
    {
        ToggleAIPath(false);
        aiDestinationSetter.target = null;
        action?.Invoke();
    }

    public void Move(Transform _target)
    {
        animator.SetTrigger("Walk");
        aiDestinationSetter.target = _target;
        
        ToggleAIPath(true);
    }
     
    public void Move(Transform _target,Action onComplete=null )
    {
        animator.SetTrigger("Walk");
        aiDestinationSetter.target = _target;

        ToggleAIPath(true);
        StartCoroutine(ProcessOnComplete(onComplete));
        
    }
    private IEnumerator ProcessOnComplete(Action onComplete)
    {
        while (!aiPath.reachedEndOfPath)
        {
            yield return null;
        }
        onComplete?.Invoke();
    }
}