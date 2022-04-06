using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;

public class MoneyTextDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI tmpMoney;

    [SerializeField]
    private IntVariable moneyVariable;

    private void Start() => FindObjectOfType<MoneyManager>().CallMoneyUpdate();

    public void UpdateMoneyUI()=> tmpMoney.text = moneyVariable >= 0 ? moneyVariable.ToString() : "0";
}