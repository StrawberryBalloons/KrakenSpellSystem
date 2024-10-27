using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class SpellParameters : MonoBehaviour
{
    public List<object> parameters = new List<object>();

    public void AddParameter(object para)
    {
        parameters.Add(para);
    }

    public void NewParameters()
    {
        List<object> parameters = new List<object>();
    }

}
