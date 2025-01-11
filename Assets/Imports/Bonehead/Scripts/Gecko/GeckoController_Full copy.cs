using System.Collections;
using UnityEngine;

public class GeckoController_FullCopy : MonoBehaviour
{

    [SerializeField] bool legSteppingEnabled;
    bool legIKEnabled;

    void Awake()
    {
        StartCoroutine(LegUpdateCoroutine());
    }

    #region Legs

    [Header("Legs")]
    [SerializeField] LegStepper_Full frontLeftLegStepper;
    [SerializeField] LegStepper_Full frontRightLegStepper;
    [SerializeField] LegStepper_Full backLeftLegStepper;
    [SerializeField] LegStepper_Full backRightLegStepper;

    // Only allow diagonal leg pairs to step together
    IEnumerator LegUpdateCoroutine()
    {
        while (true)
        {
            while (!legSteppingEnabled) yield return null;

            do
            {
                frontLeftLegStepper.TryMove();
                yield return null;
            } while (frontLeftLegStepper.Moving);

            do
            {
                frontRightLegStepper.TryMove();
                yield return null;
            } while (frontRightLegStepper.Moving);
        }
    }
    #endregion

    [Button]
    void ToggleLegIK()
    {
        legIKEnabled = !legIKEnabled;
        foreach (var ik in GetComponentsInChildren<InverseKinematics>())
        {
            ik.enabled = legIKEnabled;
        }
    }
}
