using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StudentState {
    walk,
    stagger
}

public class Student : MonoBehaviour
{

    public IntValue studentHealth;
    public StudentState currentState;

    private Rigidbody2D myRigidbody;
    private Animator anim;
    public int currentHealth;

    // Start is called before the first frame update
    void Awake()
    {
        currentState = StudentState.walk;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHealth = studentHealth.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Knock(float knockTime, int damage) {
        // If wearing mask
        currentHealth -= damage;


        if (currentHealth > 0) {
            StartCoroutine(KnockCo(knockTime));
        } else {
            anim.SetBool("isDead", true);
            StartCoroutine(Inactive());
        }
    }

    private IEnumerator KnockCo(float knockTime) {
        yield return new WaitForSeconds(knockTime);
        currentState = StudentState.walk;
        myRigidbody.velocity = Vector2.zero;
    }

    private IEnumerator Inactive() {
        yield return new WaitForSeconds(2f);
    }

}
