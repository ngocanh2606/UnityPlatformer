using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float DOUBLE_CLICK_TIME = .3f;
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;

    [SerializeField] private AudioSource jumpSoundEffect;

    private float dirx = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce= 14f;
    [SerializeField] private float lastClickTime;

    [SerializeField] private GameObject fireballPrefab;

    private enum MovementState
    {
        idle, running, jumping, falling, attacking, shooting
    }


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirx = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirx * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }


    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirx > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirx < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y> .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        if (Input.GetMouseButtonDown(0))
        {

            state = MovementState.attacking;
        }

        if (Input.GetMouseButtonDown(1)&&(Time.time - lastClickTime>0.5f))
        {
            state = MovementState.shooting;
            if(rb.velocity.y<.1f && rb.velocity.y>-.1f)
            { 
                ShootFireBall();
                lastClickTime = Time.time;
            }
            
            
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void ShootFireBall()
    {
        GameObject go = Instantiate(fireballPrefab, transform.position + new Vector3((sprite.flipX ? -1.2f : 1.2f), -1.0f, 0), Quaternion.identity);
        FireBall fb = go.GetComponent<FireBall>();
        fb.Launch(sprite.flipX ? -1 : 1);
    }
}
 