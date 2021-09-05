using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public BoolValue isPaused;
    public GameObject player;
    public bool isRealWorld;

    [Header("Items")]
	[Space]
    // public GameObject coinPrefab;
    // public GameObject heartPrefab;
    // public int heartNumber;
    public GameObject chemical;
    public GameObject ruler;
    public GameObject pipe;
    public GameObject wrench;
    public GameObject fireBox;
    public GameObject wrenchBox;
    public GameObject heart1;
    public GameObject heart2;

    [Header("Popups")]
	[Space]
    public GameObject gunPopups;
    public GameObject rulerPopups;
    public GameObject telepopups;
    public GameObject wonPopups;
    public GameObject lostPopups;

    private GameObject itemChemical;
    private GameObject itemRuler;
    private GameObject itemPipe;
    private GameObject itemFire;
    private GameObject itemGun;
    private GameObject itemWrench;
    private GameObject itemHeart;

    private HashSet<string> itemsForGun;

    private bool showGunPopups;


    [Header("Healing")]
	[Space]
    public IntValue playerHealth;
    public int heartValue;


    // private int nextTimestamp = 3;
    // private GameObject[] breakableObjects;

    void Start()
    {
        Debug.Log(StateControl.Instance.itemArr.Length);

        InitItemsGFX();
    }

    private void InitItemsGFX() {
        itemChemical = GameObject.Find("/Canvas/ItemBar/ItemChemical");
        itemRuler = GameObject.Find("/Canvas/ItemBar/ItemRuler");
        itemPipe = GameObject.Find("/Canvas/ItemBar/ItemPipe");
        itemFire = GameObject.Find("/Canvas/ItemBar/ItemFire");
        itemGun = GameObject.Find("/Canvas/ItemBar/ItemGun");
        itemWrench = GameObject.Find("/Canvas/ItemBar/ItemWrench");
        itemHeart = GameObject.Find("/Canvas/ItemBar/ItemHeart");

        itemsForGun = new HashSet<string>();
        itemsForGun.Add("FireExtinguisher");
        itemsForGun.Add("Wrench");
        itemsForGun.Add("Chemical");
        itemsForGun.Add("Pipe");



        if (isRealWorld && StateControl.Instance.didFireBoxBroke) {
            fireBox.GetComponent<Animator>().SetBool("smash", true);
        }

        if (isRealWorld && StateControl.Instance.didWrenchBoxBroke) {           
            wrenchBox.GetComponent<Animator>().SetBool("smash", true);
        }

        if (isRealWorld && StateControl.Instance.lootHeart1) {
            heart1.SetActive(false);
        }
        
        if (isRealWorld && StateControl.Instance.lootHeart2) {
            heart2.SetActive(false);
        }

        for (int i = 0; i < 5; i++) {
            switch (StateControl.Instance.itemArr[i]) {
                case "FireExtinguisher": 
                    AddItem(StateControl.Instance.itemArr, "FireExtinguisher", i); 
                    if (isRealWorld) {
                        fireBox.GetComponent<Animator>().SetBool("looted", true);
                    }
                    break;
                case "Ruler": AddItem(StateControl.Instance.itemArr, "Ruler", i); 
                    player.GetComponent<Animator>().SetBool("hasRuler", true);
                    if (isRealWorld) {
                        ruler.SetActive(false);
                    }
                    break;
                case "Gun": 
                    AddItem(StateControl.Instance.itemArr, "Gun", i); 
                    player.GetComponent<Animator>().SetBool("hasGun", true);
                    if (isRealWorld) {
                        pipe.SetActive(false);
                        chemical.SetActive(false);
                        wrench.SetActive(false);
                        fireBox.GetComponent<Animator>().SetBool("looted", true);
                        wrenchBox.GetComponent<Animator>().SetBool("looted", true);
                    }
                    break;
                case "Wrench": 
                    AddItem(StateControl.Instance.itemArr, "Wrench", i); 
                    if (isRealWorld) {
                        wrenchBox.GetComponent<Animator>().SetBool("looted", true);
                    }
                    break;
                case "Chemical": 
                    AddItem(StateControl.Instance.itemArr, "Chemical", i); 
                     if (isRealWorld) {
                        chemical.SetActive(false);
                    }
                    break;
                case "Pipe": 
                    AddItem(StateControl.Instance.itemArr, "Pipe", i); 
                    if (isRealWorld) {
                        pipe.SetActive(false);
                    }
                    break;
                case "Heart1": AddItem(StateControl.Instance.itemArr, "Heart1", i); break;
                case "Heart2": AddItem(StateControl.Instance.itemArr, "Heart2", i); break;
                default: break;
            }
        }

        RenderItemGFX();
    }


    public void IncreaseHealth() {
        RemoveItem(StateControl.Instance.itemArr, "Heart1");
        RemoveItem(StateControl.Instance.itemArr, "Heart2");
        itemHeart.SetActive(false);
        playerHealth.runtimeValue = Mathf.Min(playerHealth.initialValue, playerHealth.runtimeValue + heartValue);
    }

    public void ReloadGame() {
        StateControl.Instance.ResetState();
        SceneManager.LoadScene("RealWorldScene");
    }

    // TODO: move to GameManager
    public void CheckGunMaterials() {

        if (ContainsAll(StateControl.Instance.itemArr, itemsForGun)) {
            StartCoroutine(ShowPopups(gunPopups));

            FindObjectOfType<AudioManager>().Play("Gun");
            player.GetComponent<Animator>().SetBool("hasGun", true);

            RemoveItem(StateControl.Instance.itemArr, itemsForGun);

            itemChemical.SetActive(false);
            itemPipe.SetActive(false);
            itemFire.SetActive(false);
            itemWrench.SetActive(false);

            AddItem(StateControl.Instance.itemArr, "Gun");
            itemGun.SetActive(true);
            StateControl.Instance.hasGun = true;

            RenderItemGFX();
        }
    }

    public void ShowLostPopups() {
        lostPopups.SetActive(true);
    }

    public void ShowWonPopups() {
        wonPopups.SetActive(true);
    }

    public void ShowRulerPopups() {
        StartCoroutine(ShowPopups(rulerPopups));
    }
    public void ShowGunPopups() {
        StartCoroutine(ShowPopups(gunPopups));
    }

    public void ShowTelePopups() {
        StartCoroutine(ShowPopups(telepopups));
    }

    private IEnumerator ShowPopups(GameObject popups) {
        Vector2 pos = popups.GetComponent<RectTransform>().localPosition;
        for (int i = 0; i < 20; i++) {
            pos.y += 4f;
            popups.GetComponent<RectTransform>().localPosition = pos;
            yield return new WaitForSeconds (0.01f);
        }

        yield return new WaitForSeconds (5f);

        for (int i = 0; i < 20; i++) {
            pos.y -= 4f;
            popups.GetComponent<RectTransform>().localPosition = pos;
            yield return new WaitForSeconds (0.01f);
        }
    }

    private bool ContainsAll(string[] arr, HashSet<string> gunItems) {
        int count = 0;
        for (int i = 0; i < arr.Length; i++) {
            if (gunItems.Contains(arr[i])) {
                count++;
            }
        }
        return count == 4;
    }

    private void RemoveItem(string[] arr, string strToRemove) {
        for (int i = 0; i < arr.Length; i++) {
            if (strToRemove == arr[i]) {
                arr[i] = "";
            }
        }
    }

    public int AddItem(string[] arr, string strToAdd) {
        for (int i = 0; i < arr.Length; i++) {
            // Debug.Log(i + ": " + arr[i]);
            if (arr[i] == "") {
                arr[i] = strToAdd;
                return i;
            }
        }
        return 0;
    }

    public int AddItem(string[] arr, string strToAdd, int i) {
        arr[i] = strToAdd;
        return i;
    }

    private void RemoveItem(string[] arr, HashSet<string> gunItems) {
        for (int i = 0; i < arr.Length; i++) {
            if (gunItems.Contains(arr[i])) {
                arr[i] = "";
            }
        }
    }

    // TODO: move to GameManager
    private void RenderItemGFX() {
        for (int i = 0; i < 5; i++) {
            string itemStr = StateControl.Instance.itemArr[i];
            
            if (itemStr == "") {
                continue;
            }

            GameObject itemGFX;

            switch (itemStr) {
                case "FireExtinguisher": itemGFX = itemFire; break;
                case "Ruler": itemGFX = itemRuler; break;
                case "Gun": itemGFX = itemGun; break;
                case "Wrench": itemGFX = itemWrench; break;
                case "Chemical": itemGFX = itemChemical; break;
                case "Pipe": itemGFX = itemPipe; break;
                case "Heart1": itemGFX = itemHeart; break;
                case "Heart2": itemGFX = itemHeart; break;
                default: itemGFX = null; break;
            }

            Vector3 pos = new Vector3(46 * (i - 2), 0, 0);

            if (itemGFX != null) {
                itemGFX.SetActive(true);
                itemGFX.GetComponent<RectTransform>().localPosition = pos;
            }
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
