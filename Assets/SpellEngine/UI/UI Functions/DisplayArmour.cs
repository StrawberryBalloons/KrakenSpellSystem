using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayArmour : MonoBehaviour
{
    public PlayerStats player;

    public List<TextMeshProUGUI> armourTexts;

    void OnEnable()
    {
        DisplayArmourPieces();
    }

    void DisplayArmourPieces()
    {
        for (int i = 0; i < armourTexts.Count; i++)
        {
            if (player.currentEquipment[i] != null)
            {
                armourTexts[i].text = player.currentEquipment[i].name;
            }
            else
            {
                armourTexts[i].text = "";
            }
        }
    }
}
