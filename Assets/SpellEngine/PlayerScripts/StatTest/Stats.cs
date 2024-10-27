// using UnityEngine;
// using System.Collections.Generic;

// public class Stats : MonoBehaviour
// {
//     // Health
//     public float health = 100f;
//     public float healthRegen = 1f;
//     public bool healthForMana = false; // Toggle to use health instead of mana when mana is insufficient
//     public bool healthForStam = false; // Toggle to use health instead of stamina when stamina is insufficient

//     // Mana
//     public float mana = 100f;
//     public float manaRegen = 1f;
//     public bool manaForHealth = false; // Toggle to use mana instead of health when health is insufficient

//     // Stamina
//     public float stamina = 100f;
//     public float staminaRegen = 1f;
//     public bool stamForMana = false; // Toggle to use stamina instead of mana when mana is insufficient

//     // Armour
//     public float armour = 10f;

//     // Spells
//     public float castDelay = 1f;
//     public float castStepDelay = 1f;
//     public float spellDuration = 10f;

//     // Stat Modifiers
//     public List<float> statReductionModifiers = new List<float>();
//     public List<float> statIncreaseModifiers = new List<float>();

//     private void Start()
//     {
//         // Example: Assign a title
//         TitleSystem.Instance.AssignTitle(TitleSystem.Instance.availableTitles[0]);

//         // Example: Equip armour pieces
//         ArmourSystem.Instance.armourPieces[0] = new ArmourSystem.ArmourPiece("Helmet", 0f, 0f, 5f, 0f);
//         ArmourSystem.Instance.armourPieces[1] = new ArmourSystem.ArmourPiece("Chestplate", 0f, 0f, 10f, 0f);
//     }

//     // Function to reduce mana, considering toggles and title effects
//     public void ReduceMana(float amount)
//     {
//         if (TitleSystem.Instance.activeTitle != null)
//         {
//             amount = TitleSystem.Instance.activeTitle.ModifyManaCost(amount);
//         }

//         if (stamina >= amount && stamForMana)
//         {
//             ReduceStamina(amount);
//         }
//         else if (health >= amount && (manaForHealth || (healthForMana && mana <= amount)))
//         {
//             ReduceHealth(amount);
//         }
//         else if (mana >= amount)
//         {
//             mana -= amount;
//         }
//         else
//         {
//             // Not enough mana, handle this case as per your game's logic
//             Debug.Log("Not enough mana!");
//         }
//     }

//     // Function to reduce stamina, considering toggles and title effects
//     public void ReduceStamina(float amount)
//     {
//         if (TitleSystem.Instance.activeTitle != null)
//         {
//             amount = TitleSystem.Instance.activeTitle.ModifyStaminaCost(amount);
//         }

//         if (stamina >= amount)
//         {
//             stamina -= amount;
//         }
//         else if (health >= amount && healthForStam && (!(stamForMana && manaForHealth)))
//         {
//             ReduceHealth(amount);
//         }
//         else
//         {
//             // Not enough stamina, handle this case as per your game's logic
//             Debug.Log("Not enough stamina!");
//         }
//     }

//     // Function to reduce health (used when stamina or health is used instead of mana), considering title effects
//     public void ReduceHealth(float amount)
//     {
//         if (TitleSystem.Instance.activeTitle != null)
//         {
//             amount = TitleSystem.Instance.activeTitle.ModifyHealthCost(amount);
//         }

//         if (health >= amount)
//         {
//             health -= amount;
//         }
//         else
//         {
//             // Not enough health, handle this case as per your game's logic
//             Debug.Log("Not enough health!");
//         }
//     }

//     // Example method to apply damage to health
//     public void TakeDamage(float damage)
//     {
//         float damageAfterArmour = damage - armour;
//         if (damageAfterArmour > 0)
//         {
//             health -= damageAfterArmour;
//             if (health < 0)
//             {
//                 health = 0;
//             }
//         }
//     }

//     // Example method to regenerate health, mana, and stamina, considering title effects and armour bonuses
//     void Update()
//     {
//         RegenerateStats();
//         ApplyArmourBonuses();
//         ClampStats();
//     }

//     // Method to regenerate health, mana, and stamina
//     void RegenerateStats()
//     {
//         health += (healthRegen + (TitleSystem.Instance.activeTitle != null ? TitleSystem.Instance.activeTitle.healthRegenBonus : 0)) * Time.deltaTime;
//         mana += (manaRegen + (TitleSystem.Instance.activeTitle != null ? TitleSystem.Instance.activeTitle.manaRegenBonus : 0)) * Time.deltaTime;
//         stamina += (staminaRegen + (TitleSystem.Instance.activeTitle != null ? TitleSystem.Instance.activeTitle.staminaRegenBonus : 0)) * Time.deltaTime;
//     }

//     // Method to apply armour bonuses
//     void ApplyArmourBonuses()
//     {
//         for (int i = 0; i < ArmourSystem.MaxArmourPieces; i++)
//         {
//             if (ArmourSystem.Instance.armourPieces[i] != null)
//             {
//                 armour += ArmourSystem.Instance.armourPieces[i].armourBonus;
//             }
//         }
//     }

//     // Method to clamp stats to their maximums
//     void ClampStats()
//     {
//         health = Mathf.Clamp(health, 0f, 100f);
//         mana = Mathf.Clamp(mana, 0f, 100f);
//         stamina = Mathf.Clamp(stamina, 0f, 100f);
//         castDelay = Mathf.Max(castDelay, 0f);
//         castStepDelay = Mathf.Max(castStepDelay, 0f);
//         spellDuration = Mathf.Max(spellDuration, 0f);
//     }
// }
