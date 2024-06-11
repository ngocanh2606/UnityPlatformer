using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    private int items = 0;
    [SerializeField] private string itemName;

    [SerializeField] private TMP_Text itemText;

    [SerializeField] private AudioSource collectSoundEffect;

    private void Start()
    {
        itemText.text = itemName + ": 0";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Collectable Item"))
        {
            collectSoundEffect.Play();
            items++;
            itemText.text = itemName+": " + items;
        }
    }
}
