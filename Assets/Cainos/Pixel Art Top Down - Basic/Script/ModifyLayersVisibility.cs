using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyLayersVisibility : MonoBehaviour
{
    public GameObject[] hideItems;
    public GameObject[] showItems;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            foreach (GameObject item in hideItems)
            {
                item.SetActive(false);
            }
            foreach (GameObject item in showItems)
            {
                item.SetActive(true);
            }
        }
    }
}
