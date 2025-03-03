using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
    public event System.Action<Collider2D> onTriggerEntered2D;
    public event System.Action<Collider2D> onTriggerExited2D;
    public event System.Action<Collider2D> onTriggerStays2D;


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        onTriggerEntered2D?.Invoke(collider2D);
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        onTriggerExited2D?.Invoke(collider2D);
    }
    void OnTriggerStay2D(Collider2D collider2D)
    {
        onTriggerStays2D?.Invoke(collider2D);
    }

}
