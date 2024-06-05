using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] stopPoints;
    [SerializeField] private GameObject beeBullet;
    private int currentWaypointIndex = 0;
    private int waypointLength;

    private SpriteRenderer sprite;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    [SerializeField] private AnimationClip attackClip;
    [SerializeField] private float attackDelay;
    private float lastHitTime = 0;
    private float spawnRate;
    private float spawnTimer;

    private Animator anim;
    private MovementState state;

    private bool attack;

    private enum MovementState
    {
        moving, hit
    }
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        state = MovementState.moving;

        if (attackClip)
        {
            spawnRate = attackClip.length;
        }

        spawnTimer = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            anim.SetTrigger("death");
            StartCoroutine(Die());
        }
        
        anim.SetInteger("state", (int)state);

        Moving();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - lastHitTime > 0.6f)
        {
            if (collision.gameObject.CompareTag("Sword"))
            {
                health -= 1;
                state = MovementState.hit;
                lastHitTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Time.time - lastHitTime >= 0.7f)
        {
            if (collision.gameObject.CompareTag("Fire ball"))
            {
                health -= 2;
                state = MovementState.hit;
                lastHitTime = Time.time;
            }
        } 
    }

    private void Moving()
    {
        if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
        spawnTimer -= Time.deltaTime;
        if (gameObject.CompareTag("Bee"))
        {
            if (spawnTimer < 0)
            {
                Attack();
                spawnTimer = spawnRate;
            }
        }
         
    }

    void Attack()
    {
        GameObject go = Instantiate(beeBullet, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        BeeAttack bullet = go.GetComponent<BeeAttack>();
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(4);
        Destroy(this);
    }
}
