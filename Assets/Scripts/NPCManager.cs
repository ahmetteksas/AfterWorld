using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public Transform spawnPoint;

    [SerializeField]
    private DeskManager deskManager;

    private void Start()
    {
        if (deskManager == null) deskManager = FindObjectOfType<DeskManager>();
    }

    public Customer RequestDead()
        => deskManager.CanSendNewCustomer ? ObjectPool.instance.SpawnFromPool("NPC", spawnPoint.position, Quaternion.identity).GetComponent<Customer>() : null;
}