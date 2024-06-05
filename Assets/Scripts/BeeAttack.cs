using System.Collections;
using System.ComponentModel.Design;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BeeAttack : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallSpeed;
    [SerializeField] private LayerMask collidableLayers;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collidableLayers == (collidableLayers | (1 << collision.gameObject.layer))) //only collides with player or ground
        {
            Destroy(gameObject);
        }
    }

}