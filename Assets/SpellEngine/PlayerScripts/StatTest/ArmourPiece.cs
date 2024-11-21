using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPiece : MonoBehaviour
{
    public enum EquipmentType
    {
        Helm,
        Chest,
        Gloves,
        Legs,
        Boots,
        Cape,
        Amulet,
        Ring
    }

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Unique
    }

    [Serializable]
    public struct ArmourStat
    {
        public PlayerStats.StatType type;
        public float value;

        public ArmourStat(PlayerStats.StatType type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }

    public List<EquipmentPiece> equipment = new List<EquipmentPiece>();

    private void Awake()
    {
        InitializeDefaultArmourStatsList();
    }

    private void InitializeDefaultArmourStatsList()
    {
        foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
        {
            EquipmentPiece pieceData = new EquipmentPiece("Dragon Scale " + type, "An armor made from the scales of a dragon.", Rarity.Legendary, type);

            // Initialize armourStats with default values
            for (int i = 0; i < Enum.GetValues(typeof(PlayerStats.StatType)).Length; i++)
            {
                pieceData.armourStats.Add(new ArmourStat((PlayerStats.StatType)i, 1f));
            }


            equipment.Add(pieceData);
        }
    }
}
