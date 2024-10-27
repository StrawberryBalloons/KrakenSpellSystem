// HealthBar.cs
using UnityEngine;
using UnityEngine.UI;

public class StatBars : MonoBehaviour
{
    public PlayerStats stats; // Reference to the Health script
    public Image healthBarImage; // Reference to the UI Image
    public Image manaBarImage; // Reference to the UI Image
    public Image staminaBarImage; // Reference to the UI Image

    void FixedUpdate()
    {
        // Ensure health and healthBarImage are not null
        if (stats != null)
        {
            float healthRatio = Mathf.Clamp01(stats.currentStats[(int)PlayerStats.StatType.Health] / stats.modifiedStats[(int)PlayerStats.StatType.Health]);
            if (healthRatio < 1f)
            {
                healthBarImage.fillAmount = healthRatio;
            }

            float manaRatio = Mathf.Clamp01(stats.currentStats[(int)PlayerStats.StatType.Mana] / stats.modifiedStats[(int)PlayerStats.StatType.Mana]);
            if (manaRatio < 1f)
            {
                manaBarImage.fillAmount = manaRatio;
            }

            float stamRatio = Mathf.Clamp01(stats.currentStats[(int)PlayerStats.StatType.Stamina] / stats.modifiedStats[(int)PlayerStats.StatType.Stamina]);
            if (stamRatio < 1f)
            {
                staminaBarImage.fillAmount = stamRatio;
            }


        }
    }
}
