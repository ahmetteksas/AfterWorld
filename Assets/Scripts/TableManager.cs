using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class TableManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> purchaseZones2 = new List<GameObject>();
    [SerializeField] private List<GameObject> purchaseZones3 = new List<GameObject>();

    [SerializeField] IntVariable Var_Int_MapLevel;

    public void UpdateVisibility()
    {
        bool mediumOpen = Var_Int_MapLevel > 0;
        for (int i = 0; i < purchaseZones2.Count; i++)
        {
            purchaseZones2[i].SetActive(mediumOpen);
        }

        FindObjectOfType<UpgradeCenter>().ToggleDisplay(mediumOpen);
        bool bigOpen = Var_Int_MapLevel > 1;
        for (int i = 0; i < purchaseZones3.Count; i++)
        {
            purchaseZones3[i].SetActive(bigOpen);
        }
    }
}