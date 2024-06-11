using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FireBall : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float flySpeed;
    [SerializeField] private LayerMask collidableLayers;
   

    private void Awake()
    {
        if (!rb)
        {
            renderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }
    }


    public void Launch(float direction)
    {
        if (direction < 0)
        {
            renderer.flipX = true;
        }
        else
        {
            renderer.flipX = false;
        }
        rb.velocity = new Vector2(flySpeed * direction, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {
            EnemyMovement enemy = collision.GetComponent<EnemyMovement>(); //access EnemyMovement script
            enemy.currentHealth -= 1; //health decrease
            Debug.Log("Enemy current health is " + enemy.currentHealth);
        }
        if (collidableLayers == (collidableLayers | (1 << collision.gameObject.layer)))
        {
            Destroy(gameObject);
        }
    }
}