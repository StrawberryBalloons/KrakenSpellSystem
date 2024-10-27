using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    public PlayerStats playerStats;
    public List<TextMeshProUGUI> statTexts;
    private void OnEnable()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats script is not assigned!");
            return;
        }

        if (statTexts == null || statTexts.Count < 20)
        {
            Debug.LogError("Not enough TextMeshProUGUI elements assigned in the inspector!");
            return;
        }

        UpdateStats();
    }

    private void UpdateStats()
    {
        for (int i = 0; i < 10; i++)
        {
            statTexts[i].text = playerStats.baseStats[i].ToString("F2");
        }

        for (int i = 0; i < 10; i++)
        {
            statTexts[i + 10].text = playerStats.modifiedStats[i].ToString("F2");
        }
    }
}
