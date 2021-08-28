using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boss : MonoBehaviour
{
  public Transform player;
  public float moveSpeed = 3.5f;
  public float chargeSpeed = 6f;
  public Transform[] moveSpots;
  public float maxHealth = 10f;
  public float maxChaseTime = 5f;
  public float timeToCharge = 2f;

  private Rigidbody2D body;
  private Vector2 movementToPlayer;
  private Vector2 movementToSpot;

  private int randomSpot;

  private float chaseTime = 0;

  void Start() {
    body = this.GetComponent<Rigidbody2D>();

    randomSpot = Random.Range(0, moveSpots.Length);
  }

  void Update() {
    // Calculate movement to player
    Vector3 directionToPlayer = player.position - transform.position;

    directionToPlayer.Normalize();
    movementToPlayer = directionToPlayer;

    // Calculate movement to move spot
    float distanceToSpot = Vector3.Distance(transform.position, moveSpots[randomSpot].position);

    if (distanceToSpot < 0.5f) {
      randomSpot = GetNewSpot(randomSpot);
    }

    Vector3 directionToSpot = moveSpots[randomSpot].position - transform.position;

    directionToSpot.Normalize();
    movementToSpot = directionToSpot;

    // Count down chase time
    if (chaseTime > 0) {
      chaseTime -= Time.deltaTime;
    }
  }

  private void FixedUpdate() {
    if (chaseTime <= 0) {
      MoveToSpot(movementToSpot);
    } else if (chaseTime < timeToCharge) {
      ChargePlayer(movementToPlayer);
    } else {
      ChasePlayer(movementToPlayer);
    }
  }

  int GetNewSpot(int currentSpot) {
    int random = Random.Range(0, moveSpots.Length);
    while (random == currentSpot) {
      random = Random.Range(0, moveSpots.Length);
    }
    return random;
  }

  void MoveToSpot(Vector2 direction) {
    body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
  }

  void ChasePlayer(Vector2 direction) {
    body.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
  }

  void ChargePlayer(Vector2 direction) {
    body.MovePosition((Vector2)transform.position + (direction * chargeSpeed * Time.deltaTime));
  }

  public void TakeDamage(float damage) {
    maxHealth -= damage;
    chaseTime = maxChaseTime;

    if (maxHealth <= 0) {
      this.gameObject.SetActive(false);
    }
  }
}
