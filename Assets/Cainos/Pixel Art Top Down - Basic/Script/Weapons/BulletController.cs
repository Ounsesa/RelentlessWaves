using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int speed = 1;
    public float damage;
    public float range;
    Vector2 bulletDirection;

    SpriteRenderer spriteRenderer;

    public void Init(Vector2 direction, string layer, string sortingLayer, float weaponDamage, float weaponRange)
    {
        //gameObject.layer = LayerMask.NameToLayer(layer);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = sortingLayer;

        transform.parent = null;
        bulletDirection = direction;
        float rotation = direction.x * -90 + Mathf.Abs(direction.y) * (90 + -90 * direction.y);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));

        damage = weaponDamage;
        range = weaponRange;
    }

    private void Start()
    {
        Destroy(gameObject, range);
    }
    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = speed * bulletDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!(collision.gameObject.tag == "DamageWeapon"))
        {
            Destroy(gameObject);
        }
    }
}
