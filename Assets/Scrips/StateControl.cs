using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateControl : MonoBehaviour 
{
    public static StateControl Instance;

    public bool hasRuler;
    public bool hasChemical;
    public bool didFireBoxBroke;
    public bool hasPipe;
    public bool hasFire;
    public bool hasGun;
    public bool hasWrench;
    public Vector3 playerPos;


    void Awake ()   
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
    }


}