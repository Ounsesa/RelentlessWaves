using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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


    public bool Piercing = false;

    #endregion



    [SerializeField]
    private GameObject HitTrigger;

    private float DistanceTravelled = 0;

    #region Follower
    public bool Follower = false;
    private GameObject closestEnemy;
    [SerializeField]
    float FollowerSizeDetection = 2;
    [SerializeField]
    private GameObject FollowerTrigger;
    #endregion

    #region Explosion
    [SerializeField]
    private GameObject ExplosionTrigger;
    private List<GameObject> EnemiesInExplosionTrigger = new List<GameObject>();

    public bool ExplodesOnHit = false;

    #endregion   

    bool BallEnded = false;


    private IObjectPool<Bullet> BulletPool;

    public void SetPool(IObjectPool<Bullet> pool) { BulletPool = pool; }

    [SerializeField]
    private GameObject ExplosionCircle;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Bullet")
        {
            return;
        }
        transform.localScale = Vector3.one * Size;
        BindTriggers();
        InitTriggers();

    }
    private void InitTriggers()
    {
        FollowerTrigger.SetActive(Follower);
        ExplosionTrigger.SetActive(ExplodesOnHit);
    }

    private void BindTriggers()
    {
        FollowerTrigger.GetComponent<CustomTrigger>().OnTriggerEntered2D += FollowerTriggerEntered;
        FollowerTrigger.GetComponent<CustomTrigger>().OnTriggerExited2D += FollowerTriggerExited;
        FollowerTrigger.GetComponent<CircleCollider2D>().radius = FollowerSizeDetection;

        ExplosionTrigger.GetComponent<CustomTrigger>().OnTriggerEntered2D += ExplosionTriggerEntered;
        ExplosionTrigger.GetComponent<CustomTrigger>().OnTriggerExited2D += ExplosionTriggerExited;

        HitTrigger.GetComponent<CustomTrigger>().OnTriggerEntered2D += HitTriggerEnter;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Bullet")
        {
            return;
        }

        if (DistanceTravelled >= Range)
        {
            EndBullet();
        }
        else
        {
            BulletMovement();            
        }


    }

    private void EndBullet()
    {
        if(BallEnded)
        {
            return;
        }
        BallEnded = true;
        if (ExplodesOnHit)
        {
            ExplosionCircle.SetActive(true);
            Speed = 0f;
            Debug.Log("Active Circle");
            Invoke("RemoveExplosionCircle", 0.25f);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            DealExplosionDamage();
        }
        else
        {
            BulletPool.Release(this);
        }
    }

    private void RemoveExplosionCircle()
    {
        BulletPool.Release(this);
        ExplosionCircle.SetActive(false);
        Debug.Log("Deactive Circle");
    }

    private void BulletMovement()
    {
        if(Follower)
        {
            TargetNearbyEnemy();
        }

        GetComponent<Rigidbody2D>().velocity = Speed * Direction;
        DistanceTravelled += Speed * Time.deltaTime;

        float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));  // Adjust angle to match bullet sprite orientation
    }


    private void TargetNearbyEnemy()
    {
        if(closestEnemy == null)
        {
            return;
        }

        Vector3 EnemyDirection = closestEnemy.transform.position - transform.position;

        Vector3 NewDirection = Vector3.RotateTowards(Direction, EnemyDirection, 5 * Time.deltaTime, Time.deltaTime);
        Direction = NewDirection;
    }

    private void HitTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(Damage * DamageMultiplier);
        }
        if(!Piercing)
        {
            EndBullet();
        }
        else if(collision.gameObject.tag != "Enemy")
        {
            EndBullet();
        }
    }

    private void DealExplosionDamage()
    {
        for(int i = 0; i < EnemiesInExplosionTrigger.Count; i++)
        {
            EnemiesInExplosionTrigger[i].GetComponent<EnemyController>().TakeDamage(Damage * DamageMultiplier);
        }
    }   

    private void FollowerTriggerEntered(Collider2D collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            return;
        }

        Vector3 EnemyDirection = collision.gameObject.transform.position - transform.position;
        if(Vector3.Angle(Direction, EnemyDirection) > 80)
        {
            return;
        }

        if(closestEnemy == null)
        {
            closestEnemy = collision.gameObject;
            return;
        }
        if(Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, closestEnemy.transform.position))
        {
            closestEnemy = collision.gameObject; 
            return;
        }
        
    }

    private void FollowerTriggerExited(Collider2D collision)
    {
        if(collision.gameObject == closestEnemy) 
        {
            closestEnemy = null;
        }
    }
    private void ExplosionTriggerEntered(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            EnemiesInExplosionTrigger.Add(collision.gameObject);
            return;
        }        
        
    }
  
    private void ExplosionTriggerExited(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemiesInExplosionTrigger.Remove(collision.gameObject);
            return;
        }
    }

    public void RespawnBullet(Bullet OriginBullet)
    {
        GetComponent<SpriteRenderer>().enabled = true;
        DistanceTravelled = 0;
        Size = OriginBullet.Size;
        Damage = OriginBullet.Damage;
        DamageMultiplier = OriginBullet.DamageMultiplier;
        Range = OriginBullet.Range;
        Speed = OriginBullet.Speed;
        Piercing = OriginBullet.Piercing;
        Follower = OriginBullet.Follower;   
        ExplodesOnHit = OriginBullet.ExplodesOnHit;
        transform.localScale = Vector3.one * Size;

        BallEnded = false;
        InitTriggers();
    }

}
