using UnityEngine;
using System.Collections.Generic;

// Custom class to hold spell and its details
[System.Serializable]
public class SpellDetails
{
    public ICast spell;
    public string details;

    public SpellDetails(ICast spell, string details)
    {
        this.spell = spell;
        this.details = details;
    }
}