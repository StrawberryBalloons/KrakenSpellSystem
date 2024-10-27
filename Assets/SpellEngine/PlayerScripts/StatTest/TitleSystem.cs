// using UnityEngine;
// using System.Collections.Generic;

// public class TitleSystem : MonoBehaviour
// {
//     // Singleton instance for easy access
//     public static TitleSystem Instance;

//     // List of all available titles
//     public List<Title> availableTitles = new List<Title>();

//     // Currently active title
//     public Title activeTitle;

//     private void Awake()
//     {
//         if (Instance == null)
//         {
//             Instance = this;
//         }
//         else
//         {
//             Destroy(gameObject); // Ensure only one instance exists
//             return;
//         }

//         // Initialize titles (example)
//         availableTitles.Add(new Title("Warrior", 10f, 0f, 0f, 5f, 0f, 0f, 0f, new List<float> { 5f }, 0.8f, 0.8f, 0.8f));
//         availableTitles.Add(new Title("Mage", 0f, 10f, 0f, 0f, 0f, 0f, 0f, new List<float> { 10f }, 0.9f, 0.9f, 0.9f));
//         // Add more titles as needed
//     }

//     private void Start()
//     {
//         // Example: Assign a title
//         AssignTitle(availableTitles[0]);
//     }

//     // Function to assign a title
//     public void AssignTitle(Title newTitle)
//     {
//         if (activeTitle != null)
//         {
//             // Remove effects of the current title
//             ApplyTitleEffects(activeTitle, false);
//         }

//         // Assign the new title
//         activeTitle = newTitle;

//         // Apply effects of the new title
//         ApplyTitleEffects(activeTitle, true);
//     }

//     // Function to remove the active title
//     public void RemoveTitle()
//     {
//         if (activeTitle != null)
//         {
//             // Remove effects of the current title
//             ApplyTitleEffects(activeTitle, false);
//             activeTitle = null;
//         }
//     }

//     // Apply or remove title effects on stats
//     private void ApplyTitleEffects(Title title, bool apply)
//     {
//         if (title == null)
//             return;

//         Stats stats = GetComponent<Stats>(); // Get reference to Stats script

//         if (apply)
//         {
//             // Apply title effects on stats
//             stats.health += title.healthBonus;
//             stats.mana += title.manaBonus;
//             stats.stamina += title.staminaBonus;
//             stats.armour += title.armourBonus;
//             stats.castDelay += title.castDelayBonus;
//             stats.castStepDelay += title.castStepDelayBonus;
//             stats.spellDuration += title.spellDurationBonus;

//             // Example: Apply modifiers
//             foreach (float modifier in title.statModifiers)
//             {
//                 stats.statIncreaseModifiers.Add(modifier);
//             }
//         }
//         else
//         {
//             // Remove title effects on stats
//             stats.health -= title.healthBonus;
//             stats.mana -= title.manaBonus;
//             stats.stamina -= title.staminaBonus;
//             stats.armour -= title.armourBonus;
//             stats.castDelay -= title.castDelayBonus;
//             stats.castStepDelay -= title.castStepDelayBonus;
//             stats.spellDuration -= title.spellDurationBonus;

//             // Example: Remove modifiers
//             foreach (float modifier in title.statModifiers)
//             {
//                 stats.statIncreaseModifiers.Remove(modifier);
//             }
//         }
//     }

//     // Title class to define properties of a title
//     public class Title
//     {
//         public string titleName;
//         public float healthBonus;
//         public float manaBonus;
//         public float staminaBonus;
//         public float armourBonus;
//         public float healthRegenBonus;
//         public float manaRegenBonus;
//         public float staminaRegenBonus;
//         public List<float> statModifiers = new List<float>();
//         public float castDelayBonus;
//         public float castStepDelayBonus;
//         public float spellDurationBonus;

//         // Constructor
//         public Title(string name, float health, float mana, float stamina, float armour, float healthRegen, float manaRegen, float staminaRegen, List<float> modifiers,
//             float castDelayMod, float castStepDelayMod, float spellDurationMod)
//         {
//             titleName = name;
//             healthBonus = health;
//             manaBonus = mana;
//             staminaBonus = stamina;
//             armourBonus = armour;
//             healthRegenBonus = healthRegen;
//             manaRegenBonus = manaRegen;
//             staminaRegenBonus = staminaRegen;
//             statModifiers = modifiers;
//             castDelayBonus = castDelayMod;
//             castStepDelayBonus = castStepDelayMod;
//             spellDurationBonus = spellDurationMod;
//         }

//         // Method to modify mana cost based on title
//         public float ModifyManaCost(float baseManaCost)
//         {
//             return baseManaCost * 1f; // Default: Reduce mana cost by 20%
//         }

//         // Method to modify stamina cost based on title
//         public float ModifyStaminaCost(float baseStaminaCost)
//         {
//             return baseStaminaCost * 1f; // Default: Reduce stamina cost by 20%
//         }

//         // Method to modify health cost based on title
//         public float ModifyHealthCost(float baseHealthCost)
//         {
//             return baseHealthCost * 1f; // Default: Reduce health cost by 20%
//         }
//     }
// }
