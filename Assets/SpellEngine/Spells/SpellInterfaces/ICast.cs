using UnityEngine;
using System.Collections.Generic;
public interface ICast
{
    List<GameObject> Cast(GameObject caster, List<GameObject> inputs, List<object> parameters);
    List<GameObject> ReturnAffectedObjects();
    float ReturnManaCost();
}