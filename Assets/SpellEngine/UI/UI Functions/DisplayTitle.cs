using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTitle : MonoBehaviour
{

    public Title title;

    public TMP_Text nameText; // Reference to the TMP Text component for the name
    public TMP_Text descriptionText; // Reference to the TMP Text component for the description

    void OnEnable()
    {
        SetFirstActiveTitle();
    }

    void SetFirstActiveTitle()
    {
        nameText.text = title.titleStatsLists[0].name;
        descriptionText.text = title.titleStatsLists[0].description;
    }
}
