using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Animations;
public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Weapons equippedWeapon;

    public void PerformAttack(int directionIndex)
    {
        if (equippedWeapon == null)
        {
            Debug.LogWarning("No weapon equipped.");
            return;
        }

        int weaponType = equippedWeapon.GetType(); // Ensure this method exists
        int stanceType = equippedWeapon.GetStance(); // Ensure this method exists

        animator.SetInteger("attackDirection", directionIndex);
        animator.SetInteger("WeaponType", weaponType);

        // Log the values after setting them
        Debug.Log("Weapon Type: " + animator.GetInteger("WeaponType"));
        Debug.Log("Weapon Stance: " + animator.GetInteger("StanceType"));
        Debug.Log("Attack Direction: " + animator.GetInteger("attackDirection"));
        Debug.Log("IsEquipped: " + animator.GetBool("IsEquipped"));
    }

    public void EquipWeapon(Weapons weapon)
    {
        equippedWeapon = weapon;
        animator.SetBool("IsEquipped", true);
        animator.SetInteger("StanceType", equippedWeapon.GetStance());
    }

    public void UnEquipWeapon()
    {
        equippedWeapon = null;
        animator.SetBool("IsEquipped", false);
        animator.SetInteger("StanceType", -1);
        animator.SetInteger("WeaponType", -1);
        animator.SetInteger("attackDirection", -1);
    }

}


