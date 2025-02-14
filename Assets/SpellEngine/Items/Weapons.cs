using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapons : Item
{
    public GameObject weaponPrefab;
    public EquipmentPiece equipmentInfo;
    PlayerStats player = null;
    Inventory inven = null;
    GameObject weaponInstance = null;
    public bool equipped = false;

    public override void Use()
    {
        if (player == null)
        {
            player = this.itemOwner.GetComponent<PlayerStats>();
        }

        if (equipped)
        {
            base.Use();

            // this.name = equipmentInfo.name;
            // Debug.Log(equipmentInfo.name);

            //Equip Item
            // player.Equip(this);

            //Remove Item from Inventory
            // RemoveFromInventory();
        }

    }

    void WhatAWeaponDo()
    {
        //swing
    }

    public void Wield()
    {
        inven = this.itemOwner.GetComponent<Inventory>();
        if (weaponPrefab == null)
        {
            Debug.LogError("Weapon prefab is not assigned.");
            return;
        }

        if (inven == null)
        {
            Debug.LogError("Inventory reference (inven) is null.");
            return;
        }

        if (inven.rightHand == null)
        {
            Debug.LogError("Right hand transform is not assigned in Inventory.");
            return;
        }

        // Instantiate the weapon without affecting its original rotation or scale
        weaponInstance = Instantiate(weaponPrefab, inven.rightHand.position, inven.rightHand.rotation, inven.rightHand);

        // Adjust rotation: Set X-axis to 90 degrees while keeping the rightHand's rotation
        Quaternion newRotation = inven.rightHand.rotation * Quaternion.Euler(90f, 0f, 0f);
        weaponInstance.transform.rotation = newRotation;

        // Scale the weapon to 1/100th of its original size
        weaponInstance.transform.localScale = weaponPrefab.transform.localScale * 0.01f;

        //Add the wielded values to players stats
        foreach (var stat in equipmentInfo.equipmentStats)
        {
            player.ModifyStats(stat.type, stat.value);
        }
        equipped = true;
    }

    public void Sheathe()
    {
        //Remove the wielded values from player stats
        foreach (var stat in equipmentInfo.equipmentStats)
        {
            player.ModifyStats(stat.type, -stat.value);
        }

        //Delete weapon game object
        Destroy(weaponInstance);
        equipped = false;
    }

}
