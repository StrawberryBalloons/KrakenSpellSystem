using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    [System.Serializable]
    public struct TitleStat
    {
        public PlayerStats.StatType type;
        public float value;

        public TitleStat(PlayerStats.StatType type, float value)
        {
            this.type = type;
            this.value = value;
        }
    }

    [System.Serializable]
    public struct TitleData
    {
        public string name;
        public string description;
        public List<TitleStat> titleStats;

        public TitleData(string name, string description, List<TitleStat> titleStats)
        {
            this.name = name;
            this.description = description;
            this.titleStats = titleStats;
        }
    }

    [SerializeField]
    public List<TitleData> titleStatsLists = new List<TitleData>();

    [SerializeField]
    public List<TitleData> TitleStatsLists => titleStatsLists;

    private void Awake()
    {
        // Initialize default title data
        InitialiseDefaultTitleStatsList();
    }

    private void InitialiseDefaultTitleStatsList()
    {
        List<TitleStat> defaultList = new List<TitleStat>
        {
            new TitleStat(PlayerStats.StatType.Health, 1f),
            new TitleStat(PlayerStats.StatType.HealthRegen, .1f),
            new TitleStat(PlayerStats.StatType.Mana, 5f),
            new TitleStat(PlayerStats.StatType.ManaRegen, 2f),
            new TitleStat(PlayerStats.StatType.Stamina, 1f),
            new TitleStat(PlayerStats.StatType.StaminaRegen, 0.5f),
            new TitleStat(PlayerStats.StatType.Armour, 1f),
            new TitleStat(PlayerStats.StatType.CastDelay, 1f),
            new TitleStat(PlayerStats.StatType.CastStepDelay, 1f),
            new TitleStat(PlayerStats.StatType.SpellDuration, 1f)
        };

        TitleData defaultTitleData = new TitleData("Default Title", "This is the default title description.", defaultList);
        titleStatsLists.Add(defaultTitleData);
    }

    public void AddTitleStatsList(string name, string description, List<TitleStat> newStatsList)
    {
        TitleData newTitleData = new TitleData(name, description, newStatsList);
        titleStatsLists.Insert(0, newTitleData);
    }

    public void MoveTitleStatsListToFront(int index)
    {
        if (index >= 0 && index < titleStatsLists.Count)
        {
            var titleData = titleStatsLists[index];
            titleStatsLists.RemoveAt(index);
            titleStatsLists.Insert(0, titleData);
        }
        else
        {
            Debug.LogWarning("Invalid index to move title stats list to front.");
        }
    }

    public void RemoveTitleStatsList(int index)
    {
        if (index >= 0 && index < titleStatsLists.Count)
        {
            titleStatsLists.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Invalid index to remove title stats list.");
        }
    }
}
