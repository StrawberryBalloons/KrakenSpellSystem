using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTime : MonoBehaviour, ICast, IWaitableSpell
{

    [SerializeField] private float waitDuration = 1f;
    private bool isTriggered = false;

    public float WaitDuration => waitDuration;
    public bool IsTriggered => isTriggered;
    public List<GameObject> affectedObjects;


    public List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters)
    {
        //LOAD PARAMS
        if (parameters.Count == 1 && parameters[0] is float paramFloat)
        {
            waitDuration = paramFloat;
        }
        else if (parameters.Count == 0)
        {
            Debug.Log("No params");
        }
        else
        {
            Debug.Log("Param Error");
        }

        // StartCoroutine(WaitAndTrigger());
        //MAKE NEW LIST
        //Do Nothing for a reason
        affectedObjects = inputs;
        return inputs;
    }

    IEnumerator WaitAndTrigger()
    {
        yield return new WaitForSeconds(WaitDuration);
        isTriggered = true;
    }
    public List<GameObject> ReturnAffectedObjects()
    {
        return affectedObjects;
    }
    public float ReturnManaCost()
    {
        return (10 * waitDuration) + 10;
    }

}
