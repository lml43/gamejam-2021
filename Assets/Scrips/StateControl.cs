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
    public bool hasHeart;
    public bool lootHeart1;
    public bool lootHeart2;
    public Vector3 playerPos;

    [Header("Scriptable Objects")]
	[Space]
    public BoolValue isFirstLoad;
    public IntValue playerHealth;
    public PlayerStateValue playerState;

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

    public void ResetState() {
        hasRuler = false;
        hasChemical = false;
        didFireBoxBroke = false;
        hasPipe = false;
        hasFire = false;
        hasGun = false;
        hasWrench = false;
        hasHeart = false;
        lootHeart1 = false;
        lootHeart2 = false;
        playerPos = new Vector3(-11f, -7f, 0);

        isFirstLoad.runtimeValue = isFirstLoad.initialValue;
        playerHealth.runtimeValue = playerHealth.initialValue;
        playerState.runtimeValue = PlayerState.idle;
    }


}