using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    #region BaseStats
    [HideInInspector]
    public Vector2 Direction;

    float Damage = 5;
    float DamageMultiplier = 1;
    float Range = 5;
    float RangeMultiplier = 1;
    float Speed = 10;
    float SpeedMultiplier = 1;

    float Size = 1;
    #endregion

    #region CrazyStats
    bool Follower = false;
    float FollowerSizeDetection = 10;

    bool ExplodesOnHit = false;
    float ExplosionArea = 10;

    bool Piercing = false;

    bool AreaDamage = false;
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
        
        if(Vector3.Distance(InitialPosition, transform.position) >= Range * RangeMultiplier)
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

        GetComponent<Rigidbody2D>().velocity = (Speed * SpeedMultiplier) * Direction;
    }


    private void TargetNearbyEnemy()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(Damage * DamageMultiplier);
        }

        Destroy(gameObject);
    }
}
