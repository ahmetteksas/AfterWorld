using UnityEngine;
using ScriptableObjectArchitecture;

public class MapManager : MonoBehaviour
{
    [SerializeField] GameObject[] maps = new GameObject[3];

    [SerializeField] private IntVariable Var_Int_MapLevel;

    [SerializeField] private GameEvent Event_MapUpdate;

    private void Start()
    {
        Event_MapUpdate.Raise();
    }

    public void UpdateMap()
    {
        for (int i = 0; i < maps.Length; i++)
            maps[i].SetActive(i == Var_Int_MapLevel);
    }

    public void ResetMapData()
    {
        Var_Int_MapLevel.Value = 0;
        Event_MapUpdate.Raise();
    }
}