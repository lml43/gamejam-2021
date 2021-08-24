using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public IntValue playerHealth;
    public PlayerStateValue currentState;
    public int heartValue;
    public float untouchableTime;

    [Header("Items")]
	[Space]
    public GameObject itemChemical;
    public GameObject itemRuler;
    public GameObject itemPipe;
    public GameObject itemFire;
    public GameObject itemGun;
    public BoolValue hasGun;

    private bool isProtected = false;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer renderer;
    private Animator anim;
    private int itemCount;
    private List<GameObject> itemList;

    void Start() {
        itemList = new List<GameObject>();
        myRigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Item")) {
            other.gameObject.SetActive(false);
            IncreaseHealth(heartValue);
        }
        
        // if (other.gameObject.CompareTag("Mask")) {
        //     other.gameObject.SetActive(false);
        //     isProtected = true;
        // }

        if (other.gameObject.CompareTag("Ruler")) {
            other.gameObject.SetActive(false);
            anim.SetBool("hasRuler", true);
            ShowItem(itemRuler);
        }

        if (other.gameObject.CompareTag("Chemical")) {
            other.gameObject.SetActive(false);
            ShowItem(itemChemical);
        }

        if (other.gameObject.CompareTag("Pipe")) {
            other.gameObject.SetActive(false);
            ShowItem(itemPipe);
        }

        if (other.gameObject.CompareTag("FireExtinguisher")) {
            other.gameObject.SetActive(false);
            ShowItem(itemFire);
        }
    }

    public void Blur() {
        renderer.color = new Color(1f,1f,1f,.5f);
        currentState.runtimeValue = PlayerState.untouchable;
        
        Physics2D.IgnoreLayerCollision(9, 10);

        StartCoroutine(UnBlur(untouchableTime));
    }

    public void Knock(float knockTime, int damage) {
        // If wearing mask
        if (isProtected) {
            currentState.runtimeValue = PlayerState.stagger;
            isProtected = false;
        } else {
            playerHealth.runtimeValue -= damage;
        }


        if (playerHealth.runtimeValue > 0) {
            StartCoroutine(KnockCo(knockTime));
        } else {
            this.gameObject.SetActive(false);
        }
    }

    private void ShowItem(GameObject item) {
        itemList.Add(item);

        Vector3 pos = new Vector3(49 * (itemList.Count - 3), 0, 0);
        item.GetComponent<RectTransform>().localPosition = pos;

        item.SetActive(true);
        CheckGunMaterials();
    }

    private void CheckGunMaterials() {
        if (itemList.Contains(itemChemical) && itemList.Contains(itemPipe) && itemList.Contains(itemFire)) {
            itemList.Remove(itemChemical);
            itemList.Remove(itemPipe);
            itemList.Remove(itemFire);
            itemChemical.SetActive(false);
            itemPipe.SetActive(false);
            itemFire.SetActive(false);

            itemList.Add(itemGun);
            itemGun.SetActive(true);
            hasGun.runtimeValue = true;

            for (int i = 0; i < itemList.Count; i++) {
                Vector3 pos = new Vector3(49 * (i - 2), 0, 0);
                itemList[i].GetComponent<RectTransform>().localPosition = pos;
            }
        }
    }

    private IEnumerator UnBlur(float untouchableTime) {
        yield return new WaitForSeconds(untouchableTime);
        currentState.runtimeValue = PlayerState.idle;
        renderer.color = new Color(1f,1f,1f,1f);
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private IEnumerator KnockCo(float knockTime) {
        yield return new WaitForSeconds(knockTime);
        myRigidbody.velocity = Vector2.zero;
        Blur();
    }

    private void IncreaseHealth(int value) {
        playerHealth.runtimeValue = Mathf.Min(playerHealth.initialValue, playerHealth.runtimeValue + value);
    }
}
