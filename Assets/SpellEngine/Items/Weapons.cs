using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapons : Item
{
    public GameObject weaponPrefab;
    public EquipmentPiece equipmentInfo;
    public WeaponType weaponType; // Reference to weapon type
    public WeaponType.WeaponStanceEnum customStance; // Optional custom stance

    public WeaponType.WeaponTypeEnum customType; // Optional custom stance
    // public AnimationClip[] customAttacks = new AnimationClip[8]; // Optional custom attack animations

    PlayerStats playerStats = null;
    PlayerCombat playerCombat = null;
    Inventory playerInven = null;
    GameObject weaponInstance = null;
    public bool equipped = false;

    public override void Use()
    {
        if (playerStats == null)
        {
            playerStats = this.itemOwner.GetComponent<PlayerStats>();
            playerCombat = this.itemOwner.GetComponent<PlayerCombat>();
            playerInven = this.itemOwner.GetComponent<Inventory>();
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
        playerStats = this.itemOwner.GetComponent<PlayerStats>();
        playerCombat = this.itemOwner.GetComponent<PlayerCombat>();
        playerInven = this.itemOwner.GetComponent<Inventory>();

        if (weaponPrefab == null || playerInven == null || playerInven.rightHand == null)
        {
            Debug.LogError("Weapon data missing.");
            return;
        }

        // Instantiate the weapon without affecting its original rotation or scale
        weaponInstance = Instantiate(weaponPrefab, playerInven.rightHand.position, playerInven.rightHand.rotation, playerInven.rightHand);

        // Adjust rotation: Set X-axis to 90 degrees while keeping the rightHand's rotation
        Quaternion newRotation = playerInven.rightHand.rotation * Quaternion.Euler(90f, 0f, 0f);
        weaponInstance.transform.rotation = newRotation;

        // Scale the weapon to 1/100th of its original size
        weaponInstance.transform.localScale = weaponPrefab.transform.localScale * 0.01f;

        //Add the wielded values to players stats
        foreach (var stat in equipmentInfo.equipmentStats)
        {
            playerStats.ModifyStats(stat.type, stat.value);
        }
        equipped = true;
        playerStats.GetComponent<PlayerCombat>().EquipWeapon(this);

    }

    public void Sheathe()
    {
        //Remove the wielded values from player stats
        foreach (var stat in equipmentInfo.equipmentStats)
        {
            playerStats.ModifyStats(stat.type, -stat.value);
        }

        //Delete weapon game object
        Destroy(weaponInstance);
        equipped = false;
        playerStats.GetComponent<PlayerCombat>().UnEquipWeapon();
    }
    public int GetStance()
    {
        if (customStance == null)
        {
            return (int)weaponType.weaponStance;
        }
        return (int)customStance;
    }
    public int GetType()
    {
        if (customType == null)
        {
            return (int)weaponType.weaponType;
        }
        return (int)customType;
    }

    // public int GetAttackAnimation(int directionIndex)
    // {
    //     // return weaponType.defaultAttacks[directionIndex];
    //     return customAttacks[directionIndex] != null ? customAttacks[directionIndex] : weaponType.defaultAttacks[directionIndex];
    // }
}
