using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    #region BaseStats
    [HideInInspector]
    public Vector2 Direction;

    public float Damage = 5;
    public float DamageMultiplier = 1;
    public float Range = 5;
    public float Speed = 10;

    public float Size = 1;
    #endregion

    #region CrazyStats
    public bool Follower = false;
    float FollowerSizeDetection = 10;

    public bool ExplodesOnHit = false;
    float ExplosionArea = 10;

    public bool Piercing = false;

    public bool AreaDamage = false;
    float AreaSize = 10;
    #endregion


    Vector3 InitialPosition;



    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.one * Size;
        InitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(InitialPosition, transform.position) >= Range)
        {
            Destroy(gameObject);
        }
        else
        {
            BulletMovement();
        }

    }

    private void BulletMovement()
    {
        if(Follower)
        {
            TargetNearbyEnemy();
        }

        GetComponent<Rigidbody2D>().velocity = Speed * Direction;
    }


    private void TargetNearbyEnemy()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(Damage * DamageMultiplier);
        }
        if(!Piercing)
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag != "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
