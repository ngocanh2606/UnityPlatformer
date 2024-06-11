using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private GameObject healthCubepf;
    [SerializeField] private GameObject[] healthCubes;
    [SerializeField] private GameObject[] healthCubesLocation;


    [SerializeField] private AudioSource deathSoundEffect;

    private int health;
    private float lastHitTime = 0;
    private float lastBulletHitTime = 0;
    public bool hit = false;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = healthCubes.Length; // 4 health cubes

    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Trap"))
        {
            for (int i = health - 1; i < 0; i--)
            {
                Destroy(healthCubes[i]);
            }
            Die();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            if (collision.gameObject.CompareTag("Bullet")) // injured by bullet
            {
                if(Time.time - lastBulletHitTime > 1f) {
                    for(int i =0; i<2;i++)
                    {
                        Destroy(healthCubes[health - 1]);
                        health -= 1;
                    }
                    lastBulletHitTime = Time.time;
                }
                
            }
            else //injured by colliding with enemies
            {
                if (Time.time - lastHitTime > 1f) //player cannot be injured continuously
                {
                    lastHitTime = Time.time;
                    health -= 1;
                    Destroy(healthCubes[health]);
                }

            }
        }

        if (collision.gameObject.CompareTag("Collectable Item")) //collect cherry
        {
            if(health < 4)
            {
                health++;
                
                GameObject latestHealthCube = Instantiate(healthCubepf, healthCubesLocation[health - 1].transform.position, Quaternion.identity);
                healthCubes[health - 1] = latestHealthCube;
                healthCubes[health - 1].transform.SetParent(healthCubesLocation[health - 1].transform);// set parent to health cube to be displayed in canvas

            }
            Destroy(collision.gameObject); //destroy item anyway
        }

        if (health <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        
        rb.bodyType = RigidbodyType2D.Static;
        deathSoundEffect.Play();
        anim.SetTrigger("death");
        
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
