using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    public PlayerStats playerStats; // Reference to the player's stats
    public GameObject statPrefab; // The UI prefab for each stat
    public Transform statsContainer; // Parent object to hold the instantiated stat UI elements

    private void OnEnable()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats script is not assigned!");
            return;
        }

        if (statPrefab == null || statsContainer == null)
        {
            Debug.LogError("StatPrefab or StatsContainer is not assigned!");
            return;
        }

        UpdateStats();
    }

    private void UpdateStats()
    {
        // Clear previous UI elements
        foreach (Transform child in statsContainer)
        {
            Destroy(child.gameObject);
        }

        // Ensure baseStats and modifiedStats are the same length
        int statCount = Mathf.Min(playerStats.baseStats.Count, playerStats.modifiedStats.Count);

        for (int i = 0; i < statCount; i++)
        {
            GameObject statInstance = Instantiate(statPrefab, statsContainer);

            // Assign text values
            TextMeshProUGUI[] textComponents = statInstance.GetComponentsInChildren<TextMeshProUGUI>();
            if (textComponents.Length >= 3)
            {
                textComponents[0].text = ((StatType)i).ToString(); // Assuming Enum order matches list index
                textComponents[1].text = playerStats.baseStats[i].ToString("F2");
                textComponents[2].text = playerStats.modifiedStats[i].ToString("F2");
            }
        }
    }
}
