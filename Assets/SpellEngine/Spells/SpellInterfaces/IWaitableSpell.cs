using UnityEngine;
using System.Collections.Generic;

public interface IWaitableSpell
{
    float WaitDuration { get; }
    bool IsTriggered { get; }
}
