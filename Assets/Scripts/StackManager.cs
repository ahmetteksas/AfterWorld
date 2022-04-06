using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class StackManager : MonoBehaviour
{
    public GameObjectCollection stackList;

    public List<Transform> stackListTransforms = new List<Transform>();

    public float stackJumpTime = .5f;

    public IntVariable stackCapacity;

    public ObjectGameEvent eventStackCountUpdate;

    [SerializeField] Rig leftHandIk,rightHandIk;

    public void LoadFromSave(int count)
    {
        StartCoroutine(LoadFromSaveDelayed(count));
    }

    private IEnumerator LoadFromSaveDelayed(int count)
    {
        print("Loading with initial stack: " + count.ToString());
        yield return null;
        if (count > 0)
        {
            leftHandIk.weight = 1;
            rightHandIk.weight = 1;
            for (int i = 0; i < count; i++)
            {
                print(i);
                yield return null;
                GameObject _spawnedStack = SpawnStack(i);
                if (stackList.Count <= i)
                    stackList.Add(_spawnedStack);
                else
                    stackList[i] = SpawnStack(i);
            }
        }
        else
        {
            leftHandIk.weight = 0;
            rightHandIk.weight = 0;
        }
        eventStackCountUpdate.Raise(new StackCountData(stackList.Count, stackCapacity));

        GameObject SpawnStack(int i)
        {
            GameObject _stack = ObjectPool.instance.SpawnFromPool("Stack", transform.position, Quaternion.identity);
            Transform _t = _stack.transform;
            Transform _target = stackListTransforms[i].transform;
            _t.SetParent(_target);
            _t.localScale = _t.localScale * stackSizeModifier;
            //eventStackCountUpdate.Raise(new StackCountData(stackList.Count, stackCapacity));
            _t.localRotation = Quaternion.identity;
            _t.localPosition = Vector3.zero;
            return _stack;
        }
    }

    public void ResetStack()
    {
        stackList.Clear();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StackCabinet")) ToggleStackSequence(true,other.GetComponent<StackCabinetDisplay>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("StackCabinet")) ToggleStackSequence(false, other.GetComponent<StackCabinetDisplay>());
    }

    private void ToggleStackSequence(bool start,StackCabinetDisplay stackCabinet)
    {
        stackCabinet.ToggleUI(start);
        if (start)
        {
            if (requestStackCoroutine != null) return;
            requestStackCoroutine = StartCoroutine(RequestStackCoroutine(stackCabinet));
        }
        else
        {
            if (requestStackCoroutine == null) return;
            StopCoroutine(requestStackCoroutine);
            requestStackCoroutine = null;
        }
    }

    Coroutine requestStackCoroutine;
    private IEnumerator RequestStackCoroutine(StackCabinetDisplay stackCabinet)
    {
        eventStackCountUpdate.Raise(new StackCountData(stackList.Count, stackCapacity));
        while (stackList.Count < stackCapacity)
        {
            if (stackList.Count != 0)
            {
                leftHandIk.weight = 1;
                rightHandIk.weight = 1;
            }

            Task<GameObject> task = stackCabinet.RequestSingleStack();
            yield return new WaitUntil(() => task.IsCompleted);
            GameObject _stack = task.Result;
            Transform _t = _stack.transform;
            Transform _target = stackListTransforms[stackList.Count].transform;
            _t.SetParent(_target);
            _t.DOScale(_t.localScale * stackSizeModifier, stackJumpTime * .5f).SetEase(Ease.InOutSine);
            stackList.Add(_stack); //todo change to immutable struct types for so architecture
            eventStackCountUpdate.Raise(new StackCountData(stackList.Count,stackCapacity));
            _t.DOLocalRotateQuaternion(Quaternion.identity, stackJumpTime);
            yield return _t.DOLocalJump(Vector3.zero, 1f, 1, stackJumpTime).SetEase(Ease.InOutSine).WaitForCompletion();
        }
    }

    [SerializeField]
    private FloatVariable stackSizeModifier;

    public StackDisplay SendStack()
    {
        if (stackList.Count <= 0) return null;
        GameObject _stackToSend = stackList[stackList.Count - 1];
        stackList.Remove(_stackToSend);
        if (stackList.Count == 0)
        {
            leftHandIk.weight = 0;
            rightHandIk.weight = 0;
        }
        return _stackToSend.GetComponent<StackDisplay>();
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GameObject _stack = ObjectPool.instance.SpawnFromPool("Stack", transform.position, Quaternion.identity); ;
            Transform _t = _stack.transform;
            Transform _target = stackListTransforms[stackList.Count].transform;
            _t.SetParent(_target);
            _t.DOScale(_t.localScale * stackSizeModifier, stackJumpTime * .5f).SetEase(Ease.InOutSine);
            stackList.Add(_stack); //todo change to immutable struct types for so architecture
            eventStackCountUpdate.Raise(new StackCountData(stackList.Count, stackCapacity));
            _t.DOLocalRotateQuaternion(Quaternion.identity, stackJumpTime);
            _t.DOLocalJump(Vector3.zero, 1f, 1, stackJumpTime).SetEase(Ease.InOutSine);
        }
    }

#endif
}

public class StackCountData:Object
{
    public StackCountData(int _count, int _capacity)
    {
        count = _count;
        capacity = _capacity;
    }
    public int count;
    public int capacity;
}
