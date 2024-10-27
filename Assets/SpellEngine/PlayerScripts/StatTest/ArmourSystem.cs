using UnityEngine;

public class ArmourSystem : MonoBehaviour
{
    // Singleton instance for easy access
    public static ArmourSystem Instance;

    // Maximum number of armour pieces
    public const int MaxArmourPieces = 8;

    // Array to hold armour pieces
    public ArmourPiece[] armourPieces = new ArmourPiece[MaxArmourPieces];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }

        // Initialize armour pieces (example)
        armourPieces[0] = new ArmourPiece("Helmet", 0f, 0f, 5f, 0f);
        armourPieces[1] = new ArmourPiece("Chestplate", 0f, 0f, 10f, 0f);
        // Add more armour pieces as needed
    }

    // Armour piece class to define properties of an armour piece
    public class ArmourPiece
    {
        public string armourName;
        public float healthBonus;
        public float manaBonus;
        public float staminaBonus;
        public float armourBonus;

        // Constructor
        public ArmourPiece(string name, float health, float mana, float stamina, float armour)
        {
            armourName = name;
            healthBonus = health;
            manaBonus = mana;
            staminaBonus = stamina;
            armourBonus = armour;
        }
    }
}
