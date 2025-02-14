using System;
using System.Collections.Generic;
public enum EquipmentType
{
    Helm,
    Chest,
    Gloves,
    Legs,
    Boots,
    Cape,
    Amulet,
    Ring,
    Wielded
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
public struct StatValue
{
    public StatType type;
    public float value;

    public StatValue(StatType type, float value)
    {
        this.type = type;
        this.value = value;
    }
}

[Serializable]
public struct EquipmentPiece
{
    public string name;
    public string description;
    public Rarity rarity;
    public EquipmentType equipmentType;
    public List<StatValue> equipmentStats;

    public EquipmentPiece(string name, string description, Rarity rarity, EquipmentType type)
    {
        this.name = name;
        this.description = description;
        this.rarity = rarity;
        this.equipmentType = type;
        this.equipmentStats = new List<StatValue>();
    }
}
