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
        if (equipmentInfo.equipmentStats.Count < 1 || equipmentInfo.equipmentStats.Count == null)
        {
            initDefaultArmourPiece();
            Debug.Log("ArmourPiece issue");
        }
    }

    void initDefaultArmourPiece()
    {
        EquipmentType type = EquipmentType.Helm;
        Rarity rarity = Rarity.Legendary;
        equipmentInfo = new EquipmentPiece("Developer's " + type, "An armor forged from the blood and bones of testers.", rarity, type);


        // Initialize equipmentStats with default values
        equipmentInfo.equipmentStats.Add(new StatValue((StatType.Health), 1f));
    }
}
