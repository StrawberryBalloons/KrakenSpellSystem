using System.Collections.Generic;
using UnityEngine;

public class Debuff : MonoBehaviour
{
    [System.Serializable]
    public struct DebuffStat
    {
        public StatType type;
        public float value;

        public DebuffStat(StatType type, float value)
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
            new DebuffStat(StatType.Health, -10f),
            new DebuffStat(StatType.HealthRegen, -0.5f),
            new DebuffStat(StatType.Mana, -10f),
            new DebuffStat(StatType.ManaRegen, -0.5f),
            new DebuffStat(StatType.Stamina, -10f),
            new DebuffStat(StatType.StaminaRegen, -0.5f),
            new DebuffStat(StatType.Armour, 0f),
            new DebuffStat(StatType.CastDelay, 0.2f),
            new DebuffStat(StatType.CastStepDelay, 0.2f),
            new DebuffStat(StatType.SpellDuration, -2f)
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
