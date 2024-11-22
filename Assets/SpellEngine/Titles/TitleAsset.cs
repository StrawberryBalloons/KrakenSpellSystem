using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Title", menuName = "Inventory/Title")]
public class TitleAsset : ScriptableObject
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
    public TitleData title;

}
