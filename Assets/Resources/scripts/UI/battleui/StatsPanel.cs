using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour, IObserver<Stats>
{
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI velocityText;
    public GameObject informationTextPanel;
    public GameObject statsVariablePanel;

    private IDisposable currentSubscription;

    public void OnNext(Stats stats)
    {
        UpdateUI(stats);
    }

    public void OnError(Exception error)
    {
        Debug.LogError($"StatsPanel error: {error.Message}");
    }

    public void OnCompleted()
    {
        Debug.Log("StatsPanel observation completed.");
    }

    public void SubscribeTo(Stats stats)
    {
        Unsubscribe();

        if (stats != null)
        {
            currentSubscription = stats.Subscribe(this);
            UpdateUI(stats);
        }
    }

    public void Unsubscribe()
    {
        currentSubscription?.Dispose();
        currentSubscription = null;
    }

    private void UpdateUI(Stats stats)
    {
        if (stats.life <= 0)
        {
            RemoveStatsPanel();
        } else
        {
            UpdatePanelTextInformation();
            lifeText.text = stats.life.ToString();
            attackText.text = stats.attack.ToString();
            defenseText.text = stats.deffense.ToString();
        }
    }

    private void UpdatePanelTextInformation()
    {
        if (informationTextPanel.activeSelf)
        {
            informationTextPanel.SetActive(false);
            statsVariablePanel.SetActive(true);
        }
    }

    private void RemoveStatsPanel()
    {
        if (statsVariablePanel.activeSelf)
        {
            informationTextPanel.SetActive(true);
            statsVariablePanel.SetActive(false);
        }
    }
}
