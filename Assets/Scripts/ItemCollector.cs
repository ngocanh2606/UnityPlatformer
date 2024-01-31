using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int items = 0;
    [SerializeField] private string itemName;

    [SerializeField] private Text itemText;

    [SerializeField] private AudioSource collectSoundEffect;

    private void Start()
    {
        itemText.text = itemName + ": 0";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable Item"))
        {
            collectSoundEffect.Play();
            Destroy(collision.gameObject);
            items++;
            itemText.text = itemName+": " + items;
        }
    }
}
