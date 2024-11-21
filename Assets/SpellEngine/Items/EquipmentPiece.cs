using System;
using System.Collections.Generic;

[Serializable]
public struct EquipmentPiece
{
    public string name;
    public string description;
    public ArmourPiece.Rarity rarity;
    public ArmourPiece.EquipmentType equipmentType;
    public List<ArmourPiece.ArmourStat> armourStats;

    public EquipmentPiece(string name, string description, ArmourPiece.Rarity rarity, ArmourPiece.EquipmentType type)
    {
        this.name = name;
        this.description = description;
        this.rarity = rarity;
        this.equipmentType = type;
        this.armourStats = new List<ArmourPiece.ArmourStat>();
    }
}
