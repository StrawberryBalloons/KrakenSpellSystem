using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InventoryTooltip : MonoBehaviour
{
    public InventorySlot slot;  // Reference to the inventory slot
    public Image rarityBorder;
    public Text itemText;       // UI Text component

    void FixedUpdate()
    {
        UpdateSlotUI();
    }

    public void UpdateSlotUI()
    {
        if (slot != null && slot.item != null)
        {
            if (slot.item.GetType() == typeof(Equipment))
            {
                Equipment equip = slot.item as Equipment;
                itemText.text = equip.equipmentInfo.name
                + "\n"
                + equip.equipmentInfo.rarity
                + "\n"
                + equip.equipmentInfo.equipmentType
                + "\n"
                + equip.equipmentInfo.description
                + "\n"
                + string.Join("\n", equip.equipmentInfo.equipmentStats.Select(stat => $"{stat.type}: {stat.value}"));

                Color rarityColor = GetRarityColor(equip.equipmentInfo.rarity);
                rarityColor.a = 1f; // Ensure full opacity
                rarityBorder.color = rarityColor;

            }
            else if (slot.item.GetType() == typeof(Weapons))
            {
                Weapons weapon = slot.item as Weapons;
                itemText.text = weapon.equipmentInfo.name
                + "\n"
                + weapon.equipmentInfo.rarity
                + "\n"
                + weapon.equipmentInfo.equipmentType
                + "\n"
                + weapon.equipmentInfo.description
                + "\n"
                + string.Join("\n", weapon.equipmentInfo.equipmentStats.Select(stat => $"{stat.type}: {stat.value}"));

                Color rarityColor = GetRarityColor(weapon.equipmentInfo.rarity);
                rarityColor.a = 1f; // Ensure full opacity
                rarityBorder.color = rarityColor;

            }
            else
            {
                itemText.text = slot.item.name;
                Color clearColor = rarityBorder.color;
                clearColor.a = 0f; // Hide the border
                rarityBorder.color = clearColor;
            }
        }
        else
        {
            itemText.text = "";
        }
    }
    Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.gray;
            case Rarity.Uncommon: return Color.green;
            case Rarity.Rare: return Color.blue;
            case Rarity.Epic: return new Color(0.6f, 0f, 1f); // Purple
            case Rarity.Legendary: return new Color(1f, 0.5f, 0f); // Orange
            case Rarity.Mythic: return Color.red;
            case Rarity.Unique: return Color.black;
            default: return Color.white;
        }
    }

}
