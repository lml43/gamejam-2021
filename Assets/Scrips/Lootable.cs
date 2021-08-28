using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public GameObject itemGFX;
    public GameObject fireBox;

    private bool isPlayerInRange;
    private GameObject player;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isPlayerInRange) {

            FindObjectOfType<AudioManager>().Play("Loot");

            if (gameObject.CompareTag("Ruler")) {
                gameManager.ShowRulerPopups();
                player.GetComponent<Animator>().SetBool("hasRuler", true);
                StateControl.Instance.hasRuler = true;
            }

            if (gameObject.CompareTag("FireExtinguisher")) {
                StateControl.Instance.hasFire = true;
                fireBox.GetComponent<Animator>().SetBool("looted", true);
            }

            if (gameObject.CompareTag("Chemical")) {
                StateControl.Instance.hasChemical = true;
            }

            if (gameObject.CompareTag("Pipe")) {
                StateControl.Instance.hasPipe = true;
            }

            if (gameObject.CompareTag("Wrench")) {
                StateControl.Instance.hasWrench = true;
            }

            if (gameObject.CompareTag("Heart1") || gameObject.CompareTag("Heart2")) {
                if (StateControl.Instance.hasHeart) {
                    return;
                }
                StateControl.Instance.hasHeart = true;
            }

            gameObject.SetActive(false);
            int listCount = gameManager.AddItem(itemGFX);
            ShowItem(itemGFX, listCount);
            gameManager.CheckGunMaterials();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInRange = false;
        }
    }

    private void ShowItem(GameObject item, int itemCount) {
        // itemList.Add(item);

        Vector3 pos = new Vector3(49 * (itemCount - 3), 0, 0);
        item.GetComponent<RectTransform>().localPosition = pos;

        item.SetActive(true);
        // CheckGunMaterials();
    }


    
}
