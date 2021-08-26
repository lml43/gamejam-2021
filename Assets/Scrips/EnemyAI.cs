using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public IntValue speed;
    public float nextWaypointDistance = 3f;
    public float chaseRadius;
    public float attackRadius;

    [Header("Patrol")]
	[Space]
    public float startWaitTime;
    public Transform[] moveSpots;

    Path path;
    int currentWaypoint = 0;

    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    Transform target;
    Enemy enemy;
    Transform player;


    float waitTime;
    bool isChasing;
    int randomSpot;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        player = GameObject.FindWithTag("Player").transform;

        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
        target = moveSpots[randomSpot];

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null) {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            isChasing = false;
            ChangeTarget(isChasing, true);
            enemy.ChangeState(EnemyState.walk);
            return;
        }

        // reachedEndPath = false;
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * enemy.moveSpeed * Time.deltaTime;

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
        ChangeAnim(direction);
        CheckDistanceToPlayer();
    }

    private void CheckDistanceToPlayer() {
        float distance = Vector2.Distance(player.position, rb.position);
        if (distance <= chaseRadius && distance > attackRadius && enemy.currentState != EnemyState.stagger) {
            if (!isChasing) {
                isChasing = true;
                ChangeTarget(isChasing, false);
                enemy.ChangeState(EnemyState.attack);
            }
        } else {
            isChasing = false;
            ChangeTarget(isChasing, false);
            enemy.ChangeState(EnemyState.walk);
        }
    }

    private void ChangeTarget(bool isChasing, bool updateSpot) {

        if (isChasing) {
            target = player;
            return;
        }

        if (updateSpot) {
            if (waitTime <= 0) {
                int currentSpot = randomSpot;
                while (currentSpot == randomSpot) {
                    randomSpot = Random.Range(0, moveSpots.Length);
                }
                waitTime = startWaitTime;
            } else {
                waitTime -= Time.deltaTime;
            }
        }

        target = moveSpots[randomSpot];
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
}
