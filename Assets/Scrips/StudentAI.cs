using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class StudentAI : MonoBehaviour
{
    public BoolValue isPaused;
    public int speed;
    public float nextWaypointDistance = 3f;

    [Header("Patrol")]
	[Space]
    public float startWaitTime;
    public Vector2 minPosition;
    public Vector2 maxPosition;

    Path path;
    int currentWaypoint = 0;

    Student student;
    Seeker seeker;
    Rigidbody2D rb;
    Animator anim;
    Vector2 target;

    float waitTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        student = GetComponent<Student>();

        waitTime = startWaitTime;
        target = GenerateRandomSpot();

        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target, OnPathComplete);
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
        if (isPaused.runtimeValue) {
            rb.velocity = Vector2.zero;
            return;
        }
        
        if (student.currentState == StudentState.stagger) {
            return;
        }

        if (path == null) {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count) {
            ChangeTarget(true);
            return;
        }

        // reachedEndPath = false;
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.velocity = force;

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
        ChangeAnim(direction);
    }

    private Vector2 GenerateRandomSpot() {
        float x = Random.Range(minPosition.x, maxPosition.x);
        float y = Random.Range(minPosition.y, maxPosition.y);
        return new Vector2(x, y);
    }

    private void ChangeTarget(bool updateSpot) {

        if (updateSpot) {
            if (waitTime <= 0) {
                target = GenerateRandomSpot();
                waitTime = startWaitTime;
            } else {
                waitTime -= Time.deltaTime;
            }
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
}
