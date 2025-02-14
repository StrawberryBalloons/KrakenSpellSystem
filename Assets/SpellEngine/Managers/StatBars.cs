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
            float healthRatio = Mathf.Clamp01(stats.currentStats[(int)StatType.Health] / stats.modifiedStats[(int)StatType.Health]);
            if (healthRatio < 1f)
            {
                healthBarImage.fillAmount = healthRatio;
            }

            float manaRatio = Mathf.Clamp01(stats.currentStats[(int)StatType.Mana] / stats.modifiedStats[(int)StatType.Mana]);
            if (manaRatio < 1f)
            {
                manaBarImage.fillAmount = manaRatio;
            }

            float stamRatio = Mathf.Clamp01(stats.currentStats[(int)StatType.Stamina] / stats.modifiedStats[(int)StatType.Stamina]);
            if (stamRatio < 1f)
            {
                staminaBarImage.fillAmount = stamRatio;
            }


        }
    }
}
