using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Items")]
	[Space]
    public GameObject coinPrefab;
    public GameObject heartPrefab;
    public int heartNumber;

    [Header("Player")]
	[Space]
    public IntValue playerHealth;
    public int healthReduceRate = 1;

    private int nextTimestamp = 3;
    private GameObject[] breakableObjects;

    void Start()
    {
        GenerateItems();
    }

    void GenerateItems() {
        if (breakableObjects == null)
            breakableObjects = GameObject.FindGameObjectsWithTag("Breakable");

        List<int> objectIndices = new List<int>();
        for (int i = 0; i < breakableObjects.Length; i++) {
            objectIndices.Add(i);
        }

        System.Random ran = new System.Random();

        // Generate Hearts
        for (int i = 0; i < heartNumber; i++)
        {
            int respawnIdx = GenerateObject(heartPrefab, breakableObjects, objectIndices, ran);
            objectIndices.RemoveAt(respawnIdx);
        }

        // Generate Coin
        GenerateObject(coinPrefab, breakableObjects, objectIndices, ran);
    }

    int GenerateObject(GameObject prefab, GameObject[] breakableObjects, List<int> objectIndices, System.Random ran) {
        int respawnIdx = ran.Next(objectIndices.Count);
        GameObject respawn = breakableObjects[objectIndices[respawnIdx]];
        GameObject heartItem = Instantiate(prefab, respawn.transform.position, respawn.transform.rotation);
        heartItem.SetActive(false);
        respawn.GetComponent<Pot>().SetItem(heartItem);
        return respawnIdx;
    }

    void Update() {
        if (nextTimestamp < Time.time) {
            playerHealth.runtimeValue -= healthReduceRate;
            nextTimestamp = (int) Mathf.Round(Time.time + 1);
        }
    }
}
