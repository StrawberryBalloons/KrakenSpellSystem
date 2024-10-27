using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpdater : MonoBehaviour
{
    public SimpleHealth simpleHealth; // Reference to the SimpleHealth script
    public Image healthBarImage; // Reference to the Image component

    void Update()
    {
        if (simpleHealth != null && healthBarImage != null)
        {
            // Assuming health is out of 1000
            float fillAmount = simpleHealth.health / 1000f;
            healthBarImage.fillAmount = Mathf.Clamp(fillAmount, 0f, 1f);
        }
    }
}
