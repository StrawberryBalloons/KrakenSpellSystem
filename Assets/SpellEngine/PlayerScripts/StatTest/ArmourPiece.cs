using System;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPiece : MonoBehaviour
{
    public enum ArmourType
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
    // Define the Rarity enum
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

    [Serializable]
    public struct ArmourPieceData
    {
        public string name;
        public string description;
        public Rarity rarity;
        public ArmourType armourType;
        public List<ArmourStat> armourStats;

        public ArmourPieceData(string name, string description, Rarity rarity, ArmourType type)
        {
            this.name = name;
            this.description = description;
            this.rarity = rarity;
            this.armourType = type;
            this.armourStats = new List<ArmourStat>();
        }
    }

    public List<ArmourPieceData> armourPieces = new List<ArmourPieceData>();

    private void Awake()
    {
        InitializeDefaultArmourStatsList();
    }

    private void InitializeDefaultArmourStatsList()
    {
        foreach (ArmourType type in Enum.GetValues(typeof(ArmourType)))
        {
            ArmourPieceData pieceData = new ArmourPieceData("Dragon Scale " + type, "An armor made from the scales of a dragon.", Rarity.Legendary, type);

            // Initialize armourStats with default values
            for (int i = 0; i < 10; i++)
            {
                pieceData.armourStats.Add(new ArmourStat((PlayerStats.StatType)i, 1f));
            }

            armourPieces.Add(pieceData);
        }
    }
}
