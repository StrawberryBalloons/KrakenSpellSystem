using UnityEngine;

[CreateAssetMenu(fileName = "NewPositionSet", menuName = "IK/PositionSet")]
public class PositionSetAsset : ScriptableObject //Decided not to use this
{
    [Header("Position Set Info")]
    public string setName;
    public IKTarget[] ikTargets;
    public float speed;
    public bool loop;
    public KeyCode activationKey;

    [System.Serializable]
    public struct IKTarget
    {
        public GameObject target; // Changed to GameObject
        public Transform[] positions; // Positions are still Transforms
    }
}
