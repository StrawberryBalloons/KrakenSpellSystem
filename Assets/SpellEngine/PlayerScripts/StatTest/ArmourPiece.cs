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


}
