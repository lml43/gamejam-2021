using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState {
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{

    public EnemyState currentState;
    public int baseAttack;
    public float moveSpeed;
    public FloatValue maxHealth;

    public void ChangeState(EnemyState newState) {
        if (newState != currentState) {
            currentState = newState;
        }
    }

    public void Knock(Rigidbody2D myRb, float knockTime, float damage) {
        StartCoroutine(KnockCo(myRb, knockTime));
        TakeDamage(myRb, damage);
    }

    private void TakeDamage(Rigidbody2D myRb, float damage) {
        maxHealth.runtimeValue -= damage;

        if (maxHealth.runtimeValue <= 0) {
            this.gameObject.GetComponent<Animator>().SetBool("isDead", true);
            myRb.velocity = Vector2.zero;
            StartCoroutine(Inactive(2f));
        }
    }

    private IEnumerator Inactive(float time) {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }

    private IEnumerator KnockCo(Rigidbody2D myRb, float knockTime) {
        if (myRb != null) {
            yield return new WaitForSeconds(knockTime);
            myRb.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
