using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private IntVariable Var_Int_Money;

    [SerializeField] private IntVariable Var_DollarPrice;
    [SerializeField] private IntVariable Var_DollarPriceVIP;

    [SerializeField] private GameEvent eventMoneyUpdate;

    [SerializeField] private Transform dollarUI,dollarCanvas;

    private const float dollarTweenDuration = .7f;

    public int CurrentMoney
    {
        get => Var_Int_Money;
        set
        {
            Var_Int_Money.Value =  value;
            eventMoneyUpdate.Raise();
        }
    }

    public void CallMoneyUpdate()=> eventMoneyUpdate.Raise();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dollar"))
        {
            GameObject goDollar = other.gameObject;
            ProcessCollectMoney(Var_DollarPrice,goDollar.transform.position,goDollar.transform, true);
            goDollar.SetActive(false);
        }
        if (other.gameObject.CompareTag("DollarVIP"))
        {
            GameObject goDollar = other.gameObject;
            ProcessCollectMoney(Var_DollarPriceVIP, goDollar.transform.position, goDollar.transform, true);
            goDollar.SetActive(false);
        }
    }

    public void ProcessCollectMoney(int amount, Vector3 origin, Vector3 posSpawnPoint, bool instant = false)
    {

        if (instant)
            CurrentMoney += amount;
        else
            DOTween.To(() => CurrentMoney, x => CurrentMoney = x, CurrentMoney + amount, .3f);

        ObjectPool.instance.SpawnFromPool("DollarText", posSpawnPoint, Quaternion.identity).GetComponent<DollarTextUI>().Process(amount);

        Transform tDollarSprite = ObjectPool.instance.SpawnFromPool("DollarSprite", Camera.main.WorldToScreenPoint(origin), Quaternion.Euler(Vector3.zero)).transform;
        tDollarSprite.SetParent(dollarCanvas);
        //tDollarSprite.localScale = Vector3.one * 10;
        tDollarSprite.DOMove(dollarUI.transform.position, dollarTweenDuration).OnComplete(() => tDollarSprite.gameObject.SetActive(false)).SetEase(Ease.OutSine);
    }

    public void ProcessCollectMoney(int amount, Vector3 origin, Transform textSpawnPoint,bool instant = false)
    => ProcessCollectMoney(amount, origin, textSpawnPoint.position, instant);

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) CurrentMoney += 100;
    }
#endif
}