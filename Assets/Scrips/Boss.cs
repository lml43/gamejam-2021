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

  [Header("Chasing")]
  public Transform[] targets;
  public Vector2 minPosition;
  public Vector2 maxPosition;

  private Rigidbody2D body;
  private Vector2 movementToPlayer;
  private Vector2 movementToSpot;
  private Animator anim;
  private Transform target;

  private int randomSpot;

  private float chaseTime = 0;

  void Start() {
    target = player;
    body = this.GetComponent<Rigidbody2D>();
    anim = this.GetComponent<Animator>();

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

    ChangeAnim(directionToSpot);
  }

  private void FixedUpdate() {

    if (anim.GetBool("chasing") && isOutOfBound()) {
      anim.SetBool("chasing", false);
      anim.SetBool("isAngry", false);
      body.velocity = Vector2.zero;
    } else if (!anim.GetBool("isAngry")) {
      if (chaseTime <= 0) {
        MoveToSpot(movementToSpot);
      } else if (chaseTime < timeToCharge) {
        ChargePlayer(movementToPlayer);
      } else {
        ChasePlayer(movementToPlayer);
      }
    }

    
  }

  private bool isOutOfBound() {
    return transform.position.x < minPosition.x 
      || transform.position.x > maxPosition.x 
      || transform.position.y < minPosition.y 
      || transform.position.y > maxPosition.y;
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
    // chaseTime = maxChaseTime;

    StartCoroutine(AngryCo());

    if (maxHealth <= 0) {
      // TODO: Endgame
      this.gameObject.SetActive(false);
    }
  }

  private void ChangeAnim(Vector2 direction) {
    if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
        if (direction.x > 0) {
            SetAnimVector(Vector2.right);
        } else if (direction.x < 0) {
            SetAnimVector(Vector2.left);
        }
    } else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) {
        if (direction.y > 0) {
            SetAnimVector(Vector2.up);
        } else if (direction.y < 0) {
            SetAnimVector(Vector2.down);
        }
    }
  }

  private void SetAnimVector(Vector2 vector) {
      anim.SetFloat("moveX", vector.x);
      anim.SetFloat("moveY", vector.y);
  }

  private IEnumerator AngryCo() {
    anim.SetBool("isAngry", true);
    yield return new WaitForSeconds(1.2f);
    Attack(target);
  }

  private void Attack(Transform target) {
    anim.SetBool("chasing", true);
    Vector2 difference = target.position - transform.position;
    difference = difference.normalized * chargeSpeed;
    body.AddForce(difference, ForceMode2D.Impulse);
  }
}
