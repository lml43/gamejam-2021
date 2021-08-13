using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public float inactiveAfter = .3f;

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
        yield return new WaitForSeconds(inactiveAfter);
        this.gameObject.SetActive(false);
        if (item != null) {
            item.SetActive(true);
        }
    }

}
