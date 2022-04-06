using UnityEngine;
using ScriptableObjectArchitecture;

public class ExpandZone : PurchaseZone
{
    [SerializeField] IntVariable Var_Int_MapLevel;

    [SerializeField] GameEvent Event_MapUpdate;

    public override void Purchase()
    {
        base.Purchase();
        Var_Int_MapLevel.Value = Var_Int_MapLevel + 1;
        Event_MapUpdate.Raise();
    }
}