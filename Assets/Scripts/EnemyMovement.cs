using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] stopPoints;
    private int currentWaypointIndex = 0;
    private int waypointLength;

    private SpriteRenderer sprite;
    [SerializeField] private int health;
    [SerializeField] private float speed;
    private Animator anim;

    private bool attack;

    private enum MovementState
    {
        idle, moving, hit, running, attacking
    }
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            anim.SetTrigger("death");
        }

        if (attack && gameObject.CompareTag("Bee"))
        {
            transform.position.x = Vector2.MoveTowards(transform.position.x, player.transform.position.y, Time.deltaTime * speed);
            if (gameObject.CompareTag("Bee") && transform.position == player.transform.position)
            {
               
            }
        }
        else
        {
            Moving();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            health -= 1;
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
    }

    //private void
}
