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
    public float health;
    public string enermyName;
    public int baseAttack;
    public float moveSpeed;
    public FloatValue maxHealth;

    private void Awake() {
        health = maxHealth.runtimeValue;
    }

    public void Knock(Rigidbody2D myRb, float knockTime, float damage) {
        StartCoroutine(KnockCo(myRb, knockTime));
        TakeDamage(damage);
    }

    private void TakeDamage(float damage) {
        health -= damage;

        if (health <= 0) {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(Rigidbody2D myRb, float knockTime) {
        if (myRb != null) {
            yield return new WaitForSeconds(knockTime);
            myRb.velocity = Vector2.zero;
            currentState = EnemyState.idle;
        }
    }
}
