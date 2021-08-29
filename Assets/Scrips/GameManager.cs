using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public BoolValue isFirstLoad;
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
    private List<GameObject> itemList;

    private bool showGunPopups;


    [Header("Healing")]
	[Space]
    public IntValue playerHealth;
    public int heartValue;


    // private int nextTimestamp = 3;
    private GameObject[] breakableObjects;

    void Start()
    {
        if (isFirstLoad.runtimeValue) {
            FindObjectOfType<Popups>().ShowPopupsInfo();
            isFirstLoad.runtimeValue = false;
            // GenerateItems();
        }
        InitItemsGFX();
    }

    // TODO: move to GameManager
    private void InitItemsGFX() {
        itemChemical = GameObject.Find("/Canvas/ItemBar/ItemChemical");
        itemRuler = GameObject.Find("/Canvas/ItemBar/ItemRuler");
        itemPipe = GameObject.Find("/Canvas/ItemBar/ItemPipe");
        itemFire = GameObject.Find("/Canvas/ItemBar/ItemFire");
        itemGun = GameObject.Find("/Canvas/ItemBar/ItemGun");
        itemWrench = GameObject.Find("/Canvas/ItemBar/ItemWrench");
        itemHeart = GameObject.Find("/Canvas/ItemBar/ItemHeart");

        itemList = new List<GameObject>();

        if (StateControl.Instance.didFireBoxBroke) {
            if (StateControl.Instance.hasFire) {
                itemFire.SetActive(true);
                itemList.Add(itemFire);

                if (isRealWorld) {
                    fireBox.GetComponent<Animator>().SetBool("looted", true);
                }
            }
            
            if (isRealWorld) {
                fireBox.GetComponent<Animator>().SetBool("smash", true);
            }
        }

        if (StateControl.Instance.hasRuler) {
            player.GetComponent<Animator>().SetBool("hasRuler", true);
            itemRuler.SetActive(true);
            itemList.Add(itemRuler);
            if (isRealWorld) {
                ruler.SetActive(false);
            }
        }

        if (StateControl.Instance.hasHeart) {
            itemHeart.SetActive(true);
            itemList.Add(itemHeart);
        }

        if (isRealWorld && StateControl.Instance.lootHeart1) {
            heart1.SetActive(false);
        }
        
        if (isRealWorld && StateControl.Instance.lootHeart2) {
            heart2.SetActive(false);
        }

        if (StateControl.Instance.hasPipe) {
            itemPipe.SetActive(true);
            itemList.Add(itemPipe);
            if (isRealWorld) {
                pipe.SetActive(false);
            }
        }

        if (StateControl.Instance.hasChemical) {
            itemChemical.SetActive(true);
            itemList.Add(itemChemical);
            if (isRealWorld) {
                chemical.SetActive(false);
            }
        }

        if (StateControl.Instance.didWrenchBoxBroke) {
            if (StateControl.Instance.hasWrench) {
                itemWrench.SetActive(true);
                itemList.Add(itemWrench);

                if (isRealWorld) {
                    wrenchBox.GetComponent<Animator>().SetBool("looted", true);
                }
            }
            
            if (isRealWorld) {
                wrenchBox.GetComponent<Animator>().SetBool("smash", true);
            }
        }

        if (StateControl.Instance.hasGun) {
            player.GetComponent<Animator>().SetBool("hasGun", true);
            itemGun.SetActive(true);
            itemList.Add(itemGun);

            if (isRealWorld) {
                pipe.SetActive(false);
                chemical.SetActive(false);
                wrench.SetActive(false);
                fireBox.GetComponent<Animator>().SetBool("looted", true);
                wrenchBox.GetComponent<Animator>().SetBool("looted", true);
            }
        }

        RenderItemGFX();
    }


    public void IncreaseHealth() {
        itemList.Remove(itemHeart);
        itemHeart.SetActive(false);
        playerHealth.runtimeValue = Mathf.Min(playerHealth.initialValue, playerHealth.runtimeValue + heartValue);
    }

    public void ReloadGame() {
        StateControl.Instance.ResetState();
        SceneManager.LoadScene("RealWorldScene");
    }


    // TODO: move to GameManager
    public int AddItem(GameObject item) {
        itemList.Add(item);
        return itemList.Count;
    }

    // TODO: move to GameManager
    public void CheckGunMaterials() {
        if (itemList.Contains(itemChemical) 
            && itemList.Contains(itemPipe) 
            && itemList.Contains(itemFire)
            && itemList.Contains(itemWrench)
        ) {
            StartCoroutine(ShowPopups(gunPopups));

            FindObjectOfType<AudioManager>().Play("Gun");
            player.GetComponent<Animator>().SetBool("hasGun", true);

            itemList.Remove(itemChemical);
            itemList.Remove(itemPipe);
            itemList.Remove(itemFire);
            itemList.Remove(itemWrench);

            itemChemical.SetActive(false);
            itemPipe.SetActive(false);
            itemFire.SetActive(false);
            itemWrench.SetActive(false);

            StateControl.Instance.hasFire = false;
            StateControl.Instance.hasChemical = false;
            StateControl.Instance.hasPipe = false;
            StateControl.Instance.hasWrench = false;

            itemList.Add(itemGun);
            itemGun.SetActive(true);
            StateControl.Instance.hasGun = true;

            RenderItemGFX();
        }
    }

    // TODO: move to GameManager
    private void RenderItemGFX() {
        for (int i = 0; i < itemList.Count; i++) {
            Vector3 pos = new Vector3(46 * (i - 2), 0, 0);
            itemList[i].GetComponent<RectTransform>().localPosition = pos;
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
