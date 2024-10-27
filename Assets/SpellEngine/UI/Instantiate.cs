using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{

    // Reference to the prefab to instantiate
    public GameObject prefab;
    public GameObject player;


    public void btnClick()
    {
        // Instantiate the prefab at the specified position and rotation
        GameObject instantiatedObject = Instantiate(prefab);
        GeckoController_Full geckoController = instantiatedObject.GetComponent<GeckoController_Full>();

        geckoController.target = player.transform;

    }
}
