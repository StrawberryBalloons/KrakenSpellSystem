using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Type", menuName = "Inventory/WeaponType")]
public class WeaponType : ScriptableObject
{
    public enum WeaponTypeEnum
    {
        SWORD
    }

    public enum WeaponStanceEnum
    {
        SWORD
    }

    public string typeName;
    public WeaponTypeEnum weaponType;
    public WeaponStanceEnum weaponStance;

    // public AnimationClip defaultStance;
    // public AnimationClip[] defaultAttacks = new AnimationClip[8]; // Default attacks for this type
}
