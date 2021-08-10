using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator anim;
    private GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>(); 
    }

    public void SetItem(GameObject item) {
        this.item = item;
    }

    public void Smash() {
        anim.SetBool("smash", true);
        StartCoroutine(breakCo());
    }

    IEnumerator breakCo() {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
        if (item != null) {
            item.SetActive(true);
        }
    }

}
