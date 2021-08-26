using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public float inactiveAfter = .3f;

    private Animator anim;
    public GameObject item;

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
