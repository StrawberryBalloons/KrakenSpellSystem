using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public enum StatType
    {
        Health,
        HealthRegen,
        Mana,
        ManaRegen,
        Stamina,
        StaminaRegen,
        Armour,
        CastDelay,
        CastStepDelay,
        SpellDuration
    }

    [SerializeField]
    public List<float> baseStats = new List<float>
    {
        100f,   // Health
        1f,     // HealthRegen
        100f,   // Mana
        1f,     // ManaRegen
        100f,   // Stamina
        1f,     // StaminaRegen
        10f,    // Armour
        1f,     // CastDelay
        1f,     // CastStepDelay
        10f     // SpellDuration
    };

    [SerializeField]
    public List<float> modifiedStats = new List<float>();

    [SerializeField]
    public List<float> currentStats = new List<float>();

    public Equipment[] currentEquipment;
    Title title = null;
    public SkinnedMeshRenderer targetMesh;
    SkinnedMeshRenderer[] currentMeshes;
    Inventory inventory;
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public OnEquipmentChanged onEquipmentChanged;

    private void Start()
    {
        title = GetComponent<Title>();
        title.LoadTitlesFromFolder("Assets/Resources/Titles/");
        int numSlots = Enum.GetValues(typeof(ArmourPiece.EquipmentType)).Length;
        currentEquipment = new Equipment[numSlots];
        inventory = GetComponent<Inventory>();
        currentMeshes = new SkinnedMeshRenderer[numSlots];

        InitializeStats(); //will need to be replaced when gear saving is done
        StartCoroutine(RegenerationRoutine());
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipmentInfo.equipmentType;

        Equipment oldItem = UnEquip(slotIndex);

        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }

        currentEquipment[slotIndex] = newItem;

        SkinnedMeshRenderer newMesh = Instantiate<SkinnedMeshRenderer>(newItem.mesh);
        newMesh.transform.parent = targetMesh.transform;

        newMesh.bones = targetMesh.bones;
        newMesh.rootBone = targetMesh.rootBone;
        currentMeshes[slotIndex] = newMesh;


        Debug.Log("currentEquipment: " + currentEquipment[slotIndex]);
        InitializeStats(); //will need to be replaced when gear saving is done
    }

    public Equipment UnEquip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null)
        {
            if (currentMeshes[slotIndex] != null)
            {
                Destroy(currentMeshes[slotIndex].gameObject);
            }
            Equipment oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem);

            currentEquipment[slotIndex] = null;

            if (onEquipmentChanged != null)
            {
                onEquipmentChanged.Invoke(null, oldItem);
            }
            InitializeStats(); //will need to be replaced when gear saving is done
            return oldItem;
        }
        return null;

    }

    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            UnEquip(i);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Calculate the collision force
        float collisionForce = collision.impulse.magnitude / Time.fixedDeltaTime;

        // Calculate the damage
        float damage = collisionForce / 100f;

        // Update the text component
        TakeDamage(damage);
    }

    private void InitializeStats()
    {
        ArmourPiece armourPiece = GetComponent<ArmourPiece>();

        if (armourPiece == null || title == null || title.titleStatsLists.Count == 0)
        {
            Debug.LogError("ArmourPiece or Title component not found, or Title's TitleStatsLists is empty.");
            return;
        }

        // Calculate modifiedStats: (baseStats + all armour pieces) * first title
        modifiedStats.Clear();
        modifiedStats.AddRange(baseStats);

        // EQUIPMENT
        // Add values from all armour pieces
        foreach (var item in currentEquipment)
        {
            // Debug.Log("Currrent Equipment: " + item);
            if (item != null)
            {
                for (int i = 0; i < Enum.GetValues(typeof(StatType)).Length; i++)
                {
                    modifiedStats[i] += item.equipmentInfo.armourStats[i].value;
                }
            }
        }

        // TITLES
        // Multiply by values from first title list
        var firstTitleList = title.titleStatsLists[0];
        for (int i = 0; i < modifiedStats.Count; i++)
        {
            modifiedStats[i] *= firstTitleList.titleStats[i].value;
        }

        // Initialize currentStats with modifiedStats values
        currentStats.Clear();
        currentStats.AddRange(modifiedStats);
    }
    private IEnumerator RegenerationRoutine()
    {
        while (true)
        {
            HandleRegeneration();
            yield return new WaitForSeconds(1f); // Wait for 1 second
        }
    }

    private void HandleRegeneration()
    {
        // Health Regeneration
        float healthRegen = modifiedStats[(int)StatType.HealthRegen];
        float maxHealth = modifiedStats[(int)StatType.Health];
        currentStats[(int)StatType.Health] = Mathf.Min(currentStats[(int)StatType.Health] + healthRegen, maxHealth);

        // Mana Regeneration
        float manaRegen = modifiedStats[(int)StatType.ManaRegen];
        float maxMana = modifiedStats[(int)StatType.Mana];
        currentStats[(int)StatType.Mana] = Mathf.Min(currentStats[(int)StatType.Mana] + manaRegen, maxMana);

        // Stamina Regeneration
        float staminaRegen = modifiedStats[(int)StatType.StaminaRegen];
        float maxStamina = modifiedStats[(int)StatType.Stamina];
        currentStats[(int)StatType.Stamina] = Mathf.Min(currentStats[(int)StatType.Stamina] + staminaRegen, maxStamina);
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("You took damage: " + damage + " - " + currentStats[(int)StatType.Armour] + " = " + Mathf.Max(0, damage - currentStats[(int)StatType.Armour]));

        float effectiveDamage = Mathf.Max(0, damage - currentStats[(int)StatType.Armour]);
        currentStats[(int)StatType.Health] = Mathf.Clamp(currentStats[(int)StatType.Health] - effectiveDamage, 0, modifiedStats[(int)StatType.Health]);

        Debug.Log("New Health: " + currentStats[(int)StatType.Health]);
    }

    public void UseMana(float manaUsed)
    {
        currentStats[(int)StatType.Mana] -= manaUsed;
    }

    public void UseStamina(float staminaUsed)
    {
        currentStats[(int)StatType.Stamina] -= staminaUsed;
    }

    public float GetCurrentStat(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health:
                return currentStats[0];
            case StatType.HealthRegen:
                return currentStats[1];
            case StatType.Mana:
                return currentStats[2];
            case StatType.ManaRegen:
                return currentStats[3];
            case StatType.Stamina:
                return currentStats[4];
            case StatType.StaminaRegen:
                return currentStats[5];
            case StatType.Armour:
                return currentStats[6];
            case StatType.CastDelay:
                return currentStats[7];
            case StatType.CastStepDelay:
                return currentStats[8];
            case StatType.SpellDuration:
                return currentStats[9];
            default:
                Debug.LogError("Invalid StatType");
                return 0f; // or another default value
        }
    }


    /*

        public void ReduceMana(float amount)
    {
        if (TitleSystem.Instance.activeTitle != null)
        {
            amount = TitleSystem.Instance.activeTitle.ModifyManaCost(amount);
        }

        if (stamina >= amount && stamForMana)
        {
            ReduceStamina(amount);
        }
        else if (health >= amount && (manaForHealth || (healthForMana && mana <= amount)))
        {
            ReduceHealth(amount);
        }
        else if (mana >= amount)
        {
            mana -= amount;
        }
        else
        {
            // Not enough mana, handle this case as per your game's logic
            Debug.Log("Not enough mana!");
        }
    }

    // Function to reduce stamina, considering toggles and title effects
    public void ReduceStamina(float amount)
    {
        if (TitleSystem.Instance.activeTitle != null)
        {
            amount = TitleSystem.Instance.activeTitle.ModifyStaminaCost(amount);
        }

        if (stamina >= amount)
        {
            stamina -= amount;
        }
        else if (health >= amount && healthForStam && (!(stamForMana && manaForHealth)))
        {
            ReduceHealth(amount);
        }
        else
        {
            // Not enough stamina, handle this case as per your game's logic
            Debug.Log("Not enough stamina!");
        }
    }

    // Function to reduce health (used when stamina or health is used instead of mana), considering title effects
    public void ReduceHealth(float amount)
    {
        if (TitleSystem.Instance.activeTitle != null)
        {
            amount = TitleSystem.Instance.activeTitle.ModifyHealthCost(amount);
        }

        if (health >= amount)
        {
            health -= amount;
        }
        else
        {
            // Not enough health, handle this case as per your game's logic
            Debug.Log("Not enough health!");
        }
    }

    // Example method to apply damage to health
    public void TakeDamage(float damage)
    {
        float damageAfterArmour = damage - armour;
        if (damageAfterArmour > 0)
        {
            health -= damageAfterArmour;
            if (health < 0)
            {
                health = 0;
            }
        }
    }
*/
}
