using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public IntValue playerHealth;
    public PlayerStateValue currentState;
    public int heartValue;
    public float untouchableTime;
    public BoolValue isFirstLoad;

    private bool isProtected = false;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer renderer;
    private Animator anim;
    private int itemCount;
    private GameObject itemChemical;
    private GameObject itemRuler;
    private GameObject itemPipe;
    private GameObject itemFire;
    private GameObject itemGun;
    private GameObject itemWrench;
    private List<GameObject> itemList;

    void Start() {

        if (!isFirstLoad.runtimeValue) {
            transform.position = StateControl.Instance.playerPos;
        }

        myRigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        InitItems();
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Item")) {
            other.gameObject.SetActive(false);
            IncreaseHealth(heartValue);
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

    private void InitItems() {
        itemChemical = GameObject.Find("/Canvas/ItemBar/ItemChemical");
        itemRuler = GameObject.Find("/Canvas/ItemBar/ItemRuler");
        itemPipe = GameObject.Find("/Canvas/ItemBar/ItemPipe");
        itemFire = GameObject.Find("/Canvas/ItemBar/ItemFire");
        itemGun = GameObject.Find("/Canvas/ItemBar/ItemGun");
        itemWrench = GameObject.Find("/Canvas/ItemBar/ItemWrench");

        itemList = new List<GameObject>();
        // Debug.Log("Hi" + stateControl.hasRuler);

        if (StateControl.Instance.hasRuler) {
            anim.SetBool("hasRuler", true);
            itemRuler.SetActive(true);
            itemList.Add(itemRuler);
        }

        if (StateControl.Instance.hasFire) {
            itemFire.SetActive(true);
            itemList.Add(itemFire);
        }

        if (StateControl.Instance.hasGun) {
            itemGun.SetActive(true);
            itemList.Add(itemGun);
        }

        if (StateControl.Instance.hasPipe) {
            itemPipe.SetActive(true);
            itemList.Add(itemPipe);
        }

        if (StateControl.Instance.hasChemical) {
            itemChemical.SetActive(true);
            itemList.Add(itemChemical);
        }

        if (StateControl.Instance.hasWrench) {
            itemWrench.SetActive(true);
            itemList.Add(itemWrench);
        }

        RenderItem();
    }

    public int AddItem(GameObject item) {
        itemList.Add(item);
        return itemList.Count;
    }

    public void CheckGunMaterials() {
        if (itemList.Contains(itemChemical) 
            && itemList.Contains(itemPipe) 
            && itemList.Contains(itemFire)
            && itemList.Contains(itemWrench)
        ) {
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

            RenderItem();
        }
    }

    private void RenderItem() {
        for (int i = 0; i < itemList.Count; i++) {
            Vector3 pos = new Vector3(49 * (i - 2), 0, 0);
            itemList[i].GetComponent<RectTransform>().localPosition = pos;
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
