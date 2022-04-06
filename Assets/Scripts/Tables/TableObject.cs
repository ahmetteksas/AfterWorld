using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TableObject : StackTable
{
    [SerializeField]
    private Image progressBar;

    private GameObject goProgressBar;

    [SerializeField]
    bool showProgressBar = false;

    public string animName = "";

    [SerializeField]
    private float callStartDelay = 5f;

    public override void Start()
    {
        base.Start();
        if (!showProgressBar) return;
        goProgressBar = progressBar.transform.parent.gameObject;
        ToggleProgressBar(false);
    }

    public override IEnumerator CallCustomersCoroutine()
    {
        yield return new WaitForSeconds(callStartDelay);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(frequencyMin, frequencyMax));
            if (occupied || IsStackEmpty) continue;
            Customer _customer = deskManager.PopQueue();
            if (_customer == null) continue;
            occupied = true;
            _customer.CallToTable(this);
        }
    }

    public void UpdateProgressBar(float fillAmount)
    {
        if (!showProgressBar) return;
        progressBar.fillAmount = fillAmount;
    }

    public void ToggleProgressBar(bool show)
    {
        if (!showProgressBar) return;
        progressBar.fillAmount = 0;
        goProgressBar.SetActive(show);
    }

    public void ToggleProgressBar(bool show, float delay)
    => StartCoroutine(ToggleProgressBarDelayed(show, delay));

    private IEnumerator ToggleProgressBarDelayed(bool show, float delay)
    {
        yield return new WaitForSeconds(delay);
        ToggleProgressBar(show);
    }
}