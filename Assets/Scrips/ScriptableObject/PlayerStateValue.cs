using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

[CreateAssetMenu]
public class PlayerStateValue : ScriptableObject, ISerializationCallbackReceiver {
    
    public PlayerState initialValue;

    [HideInInspector]
    public PlayerState runtimeValue;

    public void OnAfterDeserialize() {
        runtimeValue = initialValue;
    }

    public void OnBeforeSerialize() {
        
    }

    public void ResetValue() {
        runtimeValue = initialValue;
    }

}
