using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using ScriptableObjectArchitecture;

public class StackCabinetDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmpStackCount;
    
    [SerializeField]
    private GameObject goUI;

    [SerializeField]
    private Transform stackSpawnPos;

    [SerializeField]
    private Image circularProgressBar;

    [SerializeField]
    private Image imgOutline;

    [SerializeField]
    private GameObject goCircularProgressBar;

    [SerializeField]
    private Color colorFullText=Color.red;
    private Color colorDefaultText;

    private void Awake()
    {
        colorDefaultText = tmpStackCount.color;
    }

    public void ToggleUI(bool show) => goUI.SetActive(show);


    public void UpdateStackCountUI(Object _stackCountData)
    {
        StackCountData stackCountData = _stackCountData as StackCountData;
        int _count = stackCountData.count;
        int _capacity = stackCountData.capacity;
        tmpStackCount.color = _count == _capacity ? colorFullText: colorDefaultText;
        tmpStackCount.text = stackCountData .count + " / " + stackCountData.capacity;
    }

    public ObjectGameEvent eventStackCountUpdate;

    private void OnEnable()=> eventStackCountUpdate.AddListener(UpdateStackCountUI);
    private void OnDisable()=> eventStackCountUpdate.RemoveListener(UpdateStackCountUI);

    [SerializeField]
    private float reloadDuration = .7f;

    public async Task<GameObject> RequestSingleStack()
    {
        goCircularProgressBar.SetActive(true);

        imgOutline.fillAmount = 0f;
        circularProgressBar.fillAmount = 0f;
        imgOutline.DOFillAmount(1f, reloadDuration);
        await circularProgressBar.DOFillAmount(1f, reloadDuration).OnComplete(() =>
        {
            goCircularProgressBar.SetActive(false);
        }).AsyncWaitForCompletion();
        GameObject _goStack = ObjectPool.instance.SpawnFromPool("Stack", stackSpawnPos.position, Quaternion.identity);
        _goStack.transform.localScale = Vector3.one;
        return _goStack;
    }
}