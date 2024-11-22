using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentPiece equipmentInfo;
    public SkinnedMeshRenderer mesh;
    PlayerStats player = null;

    public override void Use()
    {
        player = this.itemOwner.GetComponent<PlayerStats>();
        base.Use();

        this.name = equipmentInfo.name;
        Debug.Log(equipmentInfo.name);
        //Equip Item
        player.Equip(this);
        //Remove Item from Inventory
        RemoveFromInventory();
    }


    void Awake()
    {
        if (equipmentInfo.armourStats.Count < 1 || equipmentInfo.armourStats.Count == null)
        {
            initDefaultArmourPiece();
            Debug.Log("ArmourPiece issue");
        }
    }

    void initDefaultArmourPiece()
    {
        ArmourPiece.EquipmentType type = ArmourPiece.EquipmentType.Helm;
        ArmourPiece.Rarity rarity = ArmourPiece.Rarity.Legendary;
        equipmentInfo = new EquipmentPiece("Developer's " + type, "An armor forge from the blood and bones of testers.", rarity, type);


        // Initialize armourStats with default values
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.Health), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.HealthRegen), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.Mana), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.ManaRegen), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.Stamina), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.StaminaRegen), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.Armour), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.CastDelay), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.CastStepDelay), 1f));
        equipmentInfo.armourStats.Add(new ArmourPiece.ArmourStat((PlayerStats.StatType.SpellDuration), 1f));
    }
}
