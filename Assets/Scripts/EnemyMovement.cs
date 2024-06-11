using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    //set up for moving between 2 points
    private int currentWaypointIndex = 0;
    private int waypointLength;
    [SerializeField] private GameObject[] waypoints;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject beeBullet;
    
    private SpriteRenderer sprite;

    [SerializeField] private int lastFrameHealth; //the health value before getting injured
    public int currentHealth;

    [SerializeField] private float speed;
    [SerializeField] private AnimationClip attackClip;

    [SerializeField] private float attackDelay; //0.5s after the Bee_Moving animation starts
    private float spawnRate;
    private float spawnTimer;

    private Animator anim;
    private MovementState state;

    private bool attack;
    private bool alive;
    private float lastHitTime = 0;

    //use to set integer for animation state, easier to understand than numbers
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
        currentHealth = lastFrameHealth;
        alive = true;

        if (attackClip)
        {
            spawnRate = attackClip.length;
        }

        spawnTimer = attackDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastFrameHealth <= 0 && alive==true)
        {
            StartCoroutine(Die());
            alive = false; //avoid any action when death animation is happening
        }
        else if (lastFrameHealth > 0 && alive==true) 
        {

            if (Hit())
            {
                state = MovementState.hit;
            }
            else
            {
                Moving(); //enemy keep moving if not being injured or dying
            }
            anim.SetInteger("state", (int)state);
        }
        
        
    }

    private void Moving()
    {
        state = MovementState.moving;
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
            if (spawnTimer < 0) //bee drops bullet after some time to match with animation
            {
                Attack();
                spawnTimer = spawnRate;
            }
        }
         
    }

    private bool Hit()
    {
        bool hit = false;
        if(currentHealth < lastFrameHealth) { //compare with the health before getting injured
            if (Time.time - lastHitTime >= 0.7f)
            {
                hit = true;
                
                lastHitTime = Time.time;
                lastFrameHealth= currentHealth;
            }
        }
        return hit;
    }

    void Attack()
    {
        GameObject go = Instantiate(beeBullet, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity);
        BeeAttack bullet = go.GetComponent<BeeAttack>();
    }

    IEnumerator Die()
    {
        anim.SetTrigger("death");
        yield return new WaitForSeconds(.5f); //wait animation to complete before destroying the game object
        Debug.Log("Destroy enemy");
        Destroy(gameObject);
    }

}
