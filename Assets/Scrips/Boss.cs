using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Boss : MonoBehaviour
{
  public Transform player;
  public float moveSpeed = 20f;
  public Transform[] moveSpots;

  private Rigidbody2D body;
  private Vector2 movementToPlayer;
  private Vector2 movementToSpot;

  private int randomSpot;
  private int[] traveledSpots = new int[0];

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
      if (traveledSpots.Length == moveSpots.Length) {
        Debug.Log("FULL");
        ArrayUtility.Clear(ref traveledSpots);
        randomSpot = Random.Range(0, moveSpots.Length);
      } else {
        randomSpot = GetNewSpot(randomSpot);
      }
    }

    // Debug.Log(traveledSpots);

    Vector3 directionToSpot = moveSpots[randomSpot].position - transform.position;

    directionToSpot.Normalize();
    movementToSpot = directionToSpot;
  }

  private void FixedUpdate() {
    // ChasePlayer(movementToPlayer);
    MoveToSpot(movementToSpot);
  }

  int GetNewSpot(int currentSpot) {
    // Add traveled spot
    ArrayUtility.Add(ref traveledSpots, currentSpot);

    int random = Random.Range(0, moveSpots.Length);
    while (ArrayUtility.IndexOf(traveledSpots, random) != -1) {
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
}
