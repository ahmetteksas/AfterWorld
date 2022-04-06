using UnityEngine;

public class OnboardingArrow : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] GameObject goArrow;

    public Transform target;
    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            goArrow.SetActive(target != null);
        }
    }

    [SerializeField] private float closeRange = 3f;
    [SerializeField] private float offsetY = 2f;
    [SerializeField] private float lerpSpeed = 1f;

    private void Update()
    {
        if (Target != null)
        {
            bool inRange = (player.position - Target.position).sqrMagnitude < closeRange;
            Vector3 supposedPos =  inRange ? Target.position+Vector3.up*offsetY : player.position;
            transform.position = Vector3.Lerp(transform.position, supposedPos, inRange ? Time.deltaTime * lerpSpeed : 1);
            transform.LookAt(Target);
        }
        else
        {
            transform.position = player.position;
        }
    }
}