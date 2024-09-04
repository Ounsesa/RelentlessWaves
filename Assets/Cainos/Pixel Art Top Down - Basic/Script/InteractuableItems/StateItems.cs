using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateItems : InteractuableItemWithInput
{
    public int[] isPassableStates;
    BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        numberStates = sprites.Length;
    }

    public override void Interact()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[currentState];
        

        if(isPassableStates.Contains(currentState))
        {
            boxCollider2D.enabled = false;
        }
        else
        {
            boxCollider2D.enabled = true;
        }

        currentState++;
        currentState %= numberStates;

    }
}
