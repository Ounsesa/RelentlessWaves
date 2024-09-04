using Cainos.PixelArtTopDown_Basic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractuableItemWithInput : MonoBehaviour
{
    protected int numberStates;
    public Sprite[] sprites;

    protected int currentState = 0;

    public abstract void Interact();



    // Start is called before the first frame update
    void Start()
    {
        print(numberStates);
        print(gameObject.name);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<TopDownCharacterController>().m_interactObject = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<TopDownCharacterController>().m_interactObject = null;
        }
    }

}
