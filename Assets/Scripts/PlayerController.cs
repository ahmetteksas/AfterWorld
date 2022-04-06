using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private UltimateJoystick joystick;

    [SerializeField]
    private GameObject goJoystick;

    public Animator animator;

    [SerializeField] private Rigidbody rb;

    private void Start()
    {
        
    }

    public void Cripple(bool on)
    {
        rb.isKinematic = on;
        if (!on) animator.SetTrigger("Idle");
        if (on) joystick.DisableJoystick(); else joystick.EnableJoystick();
        goJoystick.SetActive(!on);
    }

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }
    public bool IsMoving => rb.velocity!=Vector3.zero;

}