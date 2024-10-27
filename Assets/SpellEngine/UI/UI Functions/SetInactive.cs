using UnityEngine;

public class SetInactive : MonoBehaviour
{
    // This function sets the GameObject this script is attached to as inactive
    public void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }


}
