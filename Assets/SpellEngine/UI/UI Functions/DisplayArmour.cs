using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayArmour : MonoBehaviour
{
    public ArmourPiece armour;

    public List<TextMeshProUGUI> armourTexts;

    void OnEnable()
    {
        DisplayArmourPieces();
    }

    void DisplayArmourPieces()
    {
        for (int i = 0; i < armourTexts.Count; i++)
        {
            armourTexts[i].text = armour.equipment[i].name;
        }
    }
}
