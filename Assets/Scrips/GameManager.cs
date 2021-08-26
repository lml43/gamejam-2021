using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoolValue isFirstLoad;
    [Header("Items")]
	[Space]
    public GameObject coinPrefab;
    public GameObject heartPrefab;
    public int heartNumber;
    public GameObject itemChemical;
    public GameObject itemRuler;
    public GameObject itemPipe;
    public GameObject itemWrench;
    public GameObject fireBox;


    // [Header("Player")]
	// [Space]
    // public IntValue playerHealth;
    // public int healthReduceRate = 1;

    // private int nextTimestamp = 3;
    private GameObject[] breakableObjects;

    void Start()
    {
        if (StateControl.Instance.didFireBoxBroke) {
            if (StateControl.Instance.hasFire) {
                Debug.Log("Fire Box broke, hasFire");
            } else {
                Debug.Log("Fire Box broke, not hasFire");
            }
        }

        if (StateControl.Instance.hasPipe) {
            itemPipe.SetActive(false);
        }

        if (StateControl.Instance.hasRuler) {
            itemRuler.SetActive(false);
        }

        if (StateControl.Instance.hasChemical) {
            itemChemical.SetActive(false);
        }

        if (StateControl.Instance.hasWrench) {
            itemWrench.SetActive(false);
        }

        if (isFirstLoad.runtimeValue) {
            isFirstLoad.runtimeValue = false;
            // GenerateItems();
        }
    }

    // void GenerateItems() {
    //     if (breakableObjects == null)
    //         breakableObjects = GameObject.FindGameObjectsWithTag("Breakable");

    //     List<int> objectIndices = new List<int>();
    //     for (int i = 0; i < breakableObjects.Length; i++) {
    //         objectIndices.Add(i);
    //     }

    //     System.Random ran = new System.Random();

    //     // Generate Hearts
    //     for (int i = 0; i < heartNumber; i++)
    //     {
    //         int respawnIdx = GenerateObject(heartPrefab, breakableObjects, objectIndices, ran);
    //         objectIndices.RemoveAt(respawnIdx);
    //     }

    //     // Generate Coin
    //     GenerateObject(coinPrefab, breakableObjects, objectIndices, ran);
    // }

    // int GenerateObject(GameObject prefab, GameObject[] breakableObjects, List<int> objectIndices, System.Random ran) {
    //     int respawnIdx = ran.Next(objectIndices.Count);
    //     GameObject respawn = breakableObjects[objectIndices[respawnIdx]];
    //     GameObject heartItem = Instantiate(prefab, respawn.transform.position, respawn.transform.rotation);
    //     heartItem.SetActive(false);
    //     respawn.GetComponent<Pot>().SetItem(heartItem);
    //     return respawnIdx;
    // }

    // void Update() {
    //     if (nextTimestamp < Time.time) {
    //         playerHealth.runtimeValue -= healthReduceRate;
    //         nextTimestamp = (int) Mathf.Round(Time.time + 1);
    //     }
    // }
}
