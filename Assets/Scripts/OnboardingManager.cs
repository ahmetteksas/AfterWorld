using System.Collections;
using UnityEngine;
using ScriptableObjectArchitecture;

public class OnboardingManager : MonoBehaviour
{
    [SerializeField] private Transform tObjectiveStack;
    [SerializeField] private Transform tObjectiveUpgrade;
    [SerializeField] private Transform tObjectiveDesk;
    [SerializeField] private Transform tFirstTable;

    [SerializeField] private GameObject goStackCabinet;

    [SerializeField] private IntVariable Var_Int_OnboardingState;

    public int OnboardingState
    {
        get => Var_Int_OnboardingState;
        set
        {
            Var_Int_OnboardingState.Value = value;
            switch (value)
            {
                case 1:
                    onboardingArrow.Target = null;
                    break;
                case 2:
                    onboardingArrow.Target = tObjectiveDesk;
                    break;
                case 3:
                    onboardingArrow.Target = null;
                    break;
                case 4:
                    goStackCabinet.SetActive(true);
                    onboardingArrow.Target = tObjectiveStack;
                    break;
                case 5:
                    onboardingArrow.Target = tFirstTable;
                    break;
                case 6:
                    onboardingArrow.Target = null;
                    break;
                case 7:
                    break;
                case 8:
                    onboardingArrow.Target = null;
                    break;
                case 9:
                    //onboardingArrow.Target = tableVIP.transform;
                    break;
                case 10:
                    onboardingArrow.Target = null;
                    goExpandZone.SetActive(true);
                    break;
                case 11:
                    onboardingArrow.Target = tObjectiveUpgrade;
                    break;
                case 12:
                    onboardingArrow.Target = null;
                    StopAllCoroutines();
                    break;
                default:
                    break;
            }
        }
    }

    [SerializeField] private OnboardingArrow onboardingArrow;

    [SerializeField] private IntVariable Var_Int_Money;

    [SerializeField] private StackTable firstStackTable;

    [SerializeField] private GameObjectCollection stackList;

    [SerializeField] private int purchaseZonePrice = 80;
    [SerializeField] private PurchaseZone purchaseZone;

    [SerializeField] private IntVariable Var_Int_MapLevel;

    [SerializeField] private UpgradeCenter upgradeCenter;

    [SerializeField] private GameObject goExpandZone;

    private void Awake()
    {
        StartCoroutine(Onboard());
    }

    public void ResetOnboarding()
    {
        OnboardingState = 1;
        StopAllCoroutines();
        StartCoroutine(Onboard());
    }

    public void LoadOnboardedGameobjects()
    {
        return; //todo
        goStackCabinet.SetActive(OnboardingState > 4);
        goExpandZone.SetActive(OnboardingState > 9);
    }

    private IEnumerator Onboard()
    {
        OnboardingState = OnboardingState;
        LoadOnboardedGameobjects();
        while (true)
        {
            yield return null;
            switch (OnboardingState)
            {
                case 1: //1 - Wait for a customer to get in line
                    break;
                case 2: //2 - ONBOARD->Take first money
                    if (Var_Int_Money > 0) OnboardingState++;
                    break;
                case 3: //3 - Wait for the stack to finish
                    if (Var_Int_Money>30) OnboardingState++;
                    break;
                case 4://4 - ONBOARD->Spawn stack cabinet, onboard the player there and wait until player has at least one stack
                    if (stackList.Count>0) OnboardingState++;
                    break;
                case 5://5 - ONBOARD->Direct the player to first table, and wait for stack placement
                    if (!firstStackTable.IsStackEmpty) OnboardingState++;
                    break;
                case 6://6 - Wait for enough money to purchase first zone
                    if (Var_Int_Money>=purchaseZonePrice) OnboardingState++;
                    break;
                case 7://7 - ONBOARD->Spawn purchase zone, onboard the player there and wait? until zone is purchased???
                    if (purchaseZone.purchased) OnboardingState++;
                    break;
                case 8://8 - Wait for VIP customer to sit
                    //if (tableVIP.vipCustomer!=null) OnboardingState++;
                    break;
                case 9://9 - ONBOARD the player to vip table and wait until nail paint screen comes up
                    //if (tableVIP.coroutineNailBrushSequence != null) OnboardingState++;
                    break;
                case 10://10 - Wait for expansion
                    if (Var_Int_MapLevel > 0) OnboardingState++;
                    break;
                case 11://11 - ONBOARD the player to upgrade center, wait until upgrade screen comes up
                    if (!upgradeCenter.canLaunchUpgradeUI) OnboardingState++;
                    break;
                default:
                    break;
            }
        }
    }
}