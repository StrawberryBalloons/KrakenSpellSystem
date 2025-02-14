using UnityEngine;

public class ItemSparkles : MonoBehaviour
{
    public ItemPickup itemPickup; // Assign this in the inspector
    public ParticleSystem sparklesPrefab; // Assign a particle system prefab in the inspector

    void Start()
    {
        if (itemPickup != null && itemPickup.item != null)
        {
            if (itemPickup.item is Equipment equipment)
            {
                Debug.Log("Item is of type Equipment.");
                Debug.Log("Rarity: " + equipment.equipmentInfo.rarity);
                InstantiateSparkles(equipment.equipmentInfo.rarity);
            }
            else
            {
                Debug.Log("Item is not Equipment.");
            }
        }
        else
        {
            Debug.Log("ItemPickup or Item is null.");
        }
    }

    void InstantiateSparkles(Rarity rarity)
    {
        if (sparklesPrefab == null)
        {
            Debug.LogWarning("Sparkles prefab is not assigned.");
            return;
        }

        // Instantiate the particle system as a child of the item
        ParticleSystem sparkles = Instantiate(sparklesPrefab, transform.position, Quaternion.identity, transform);

        // Set the color of the particle system based on the rarity
        var mainModule = sparkles.main;
        mainModule.startColor = GetRarityColor(rarity);
    }

    Color GetRarityColor(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Color.gray;
            case Rarity.Uncommon: return Color.green;
            case Rarity.Rare: return Color.blue;
            case Rarity.Epic: return new Color(0.6f, 0f, 1f); // Purple
            case Rarity.Legendary: return new Color(1f, 0.5f, 0f); // Orange
            case Rarity.Mythic: return Color.red;
            case Rarity.Unique: return Color.black;
            default: return Color.white;
        }
    }
}
