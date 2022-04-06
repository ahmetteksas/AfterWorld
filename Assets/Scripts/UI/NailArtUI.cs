using UnityEngine;

public class NailArtUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] children = new GameObject[4];

    public bool isSuccess;

    public void Toggle(bool on)
    {
        foreach (GameObject child in children)
        {
            child.SetActive(on);
        }
    }

    public bool IsUIActive => children[0].activeSelf;
}