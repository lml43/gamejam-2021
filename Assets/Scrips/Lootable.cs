using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour
{
    public GameObject itemGFX;

    private bool isPlayerInRange;
    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && isPlayerInRange) {

            if (gameObject.CompareTag("Ruler")) {
                player.GetComponent<Animator>().SetBool("hasRuler", true);
                StateControl.Instance.hasRuler = true;
            }

            if (gameObject.CompareTag("FireExtinguisher")) {
                StateControl.Instance.hasFire = true;
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

            gameObject.SetActive(false);
            int listCount = player.GetComponent<PlayerManager>().AddItem(itemGFX);
            ShowItem(itemGFX, listCount);
            player.GetComponent<PlayerManager>().CheckGunMaterials();
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
