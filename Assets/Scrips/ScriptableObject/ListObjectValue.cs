using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ListObjectValue : ScriptableObject, ISerializationCallbackReceiver {
    
    public List<GameObject> initialValue = new List<GameObject>();

    [HideInInspector]
    public List<GameObject> runtimeValue;

    public void OnAfterDeserialize() {
        runtimeValue = initialValue;
    }

    public void OnBeforeSerialize() {
        
    }

    public void ResetValue() {
        runtimeValue = initialValue;
    }

}