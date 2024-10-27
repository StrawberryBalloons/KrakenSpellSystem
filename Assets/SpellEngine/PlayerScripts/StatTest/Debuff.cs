using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [System.Serializable]
    public struct DebuffStat
    {
        public PlayerStats.StatType type;
        public float value;

        public DebuffStat(PlayerStats.StatType type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }

    [SerializeField]
    private List<List<DebuffStat>> debuffStatsLists = new List<List<DebuffStat>>();

    public List<List<DebuffStat>> DebuffStatsLists => debuffStatsLists;

    private void Awake()
    {
        // Initialize default first list
        InitialiseDefaultDebuffStatsList();
    }

    private void InitialiseDefaultDebuffStatsList()
    {
        List<DebuffStat> defaultList = new List<DebuffStat>
        {
            new DebuffStat(PlayerStats.StatType.Health, -10f),
            new DebuffStat(PlayerStats.StatType.HealthRegen, -0.5f),
            new DebuffStat(PlayerStats.StatType.Mana, -10f),
            new DebuffStat(PlayerStats.StatType.ManaRegen, -0.5f),
            new DebuffStat(PlayerStats.StatType.Stamina, -10f),
            new DebuffStat(PlayerStats.StatType.StaminaRegen, -0.5f),
            new DebuffStat(PlayerStats.StatType.Armour, 0f),
            new DebuffStat(PlayerStats.StatType.CastDelay, 0.2f),
            new DebuffStat(PlayerStats.StatType.CastStepDelay, 0.2f),
            new DebuffStat(PlayerStats.StatType.SpellDuration, -2f)
        };

        debuffStatsLists.Add(defaultList);
    }

    public void AddDebuffStatsList(List<DebuffStat> newStatsList)
    {
        debuffStatsLists.Add(newStatsList);
    }

    public void RemoveDebuffStatsList(int index)
    {
        if (index >= 0 && index < debuffStatsLists.Count)
        {
            debuffStatsLists.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Invalid index to remove debuff stats list.");
        }
    }
}
