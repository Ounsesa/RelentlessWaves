using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTrigger : MonoBehaviour
{
    public event System.Action<Collider2D> OnTriggerEntered2D;
    public event System.Action<Collider2D> OnTriggerExited2D;
    public event System.Action<Collider2D> OnTriggerStays2D;


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        OnTriggerEntered2D?.Invoke(collider2D);
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {
        OnTriggerExited2D?.Invoke(collider2D);
    }
    void OnTriggerStay2D(Collider2D collider2D)
    {
        OnTriggerStays2D?.Invoke(collider2D);
    }

}
