using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{

    #region BaseStats
    [HideInInspector]
    public Vector2 direction;
    [HideInInspector]
    public float damage = 5;
    [HideInInspector]
    public float damageMultiplier = 1;
    [HideInInspector]
    public float range = 5;
    [HideInInspector]
    public float speed = 10;
    [HideInInspector]
    public float size = 1;
    [HideInInspector]
    public bool piercing = false;
    public IObjectPool<Bullet> bulletPool;


    [SerializeField]
    private GameObject m_hitTrigger;
    private float m_distanceTravelled = 0;
    private bool m_bulletEnded = false;



    #region Follower
    [HideInInspector]
    public bool follower = false;

    private GameObject m_closestEnemy;
    [SerializeField]
    private float m_followerSizeDetection = 2;
    [SerializeField]
    private GameObject m_followerTrigger;
    #endregion

    #region Explosion
    [HideInInspector]
    public bool explodesOnHit = false;

    [SerializeField]
    private GameObject m_explosionTrigger;
    [SerializeField]
    private GameObject m_explosionCircle;
    private List<GameObject> m_enemiesInExplosionTrigger = new List<GameObject>();
    #endregion   

    #endregion

    public void SetPool(IObjectPool<Bullet> pool) { bulletPool = pool; }


    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == "Bullet")
        {
            return;
        }
        transform.localScale = Vector3.one * size;
        BindTriggers();
        InitTriggers();

    }
    private void InitTriggers()
    {
        m_followerTrigger.SetActive(follower);
        m_explosionTrigger.SetActive(explodesOnHit);
    }

    private void BindTriggers()
    {
        m_followerTrigger.GetComponent<CustomTrigger>().onTriggerEntered2D += FollowerTriggerEntered;
        m_followerTrigger.GetComponent<CustomTrigger>().onTriggerExited2D += FollowerTriggerExited;
        m_followerTrigger.GetComponent<CircleCollider2D>().radius = m_followerSizeDetection;

        m_explosionTrigger.GetComponent<CustomTrigger>().onTriggerEntered2D += ExplosionTriggerEntered;
        m_explosionTrigger.GetComponent<CustomTrigger>().onTriggerExited2D += ExplosionTriggerExited;

        m_hitTrigger.GetComponent<CustomTrigger>().onTriggerEntered2D += HitTriggerEnter;
        
    }

    private void OnDestroy()
    {
        m_followerTrigger.GetComponent<CustomTrigger>().onTriggerEntered2D -= FollowerTriggerEntered;
        m_followerTrigger.GetComponent<CustomTrigger>().onTriggerExited2D -= FollowerTriggerExited;

        m_explosionTrigger.GetComponent<CustomTrigger>().onTriggerEntered2D -= ExplosionTriggerEntered;
        m_explosionTrigger.GetComponent<CustomTrigger>().onTriggerExited2D -= ExplosionTriggerExited;

        m_hitTrigger.GetComponent<CustomTrigger>().onTriggerEntered2D -= HitTriggerEnter;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Bullet")
        {
            return;
        }

        if (m_distanceTravelled >= range)
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
        if(m_bulletEnded)
        {
            return;
        }
        m_bulletEnded = true;
        if (explodesOnHit)
        {
            m_explosionCircle.SetActive(true);
            speed = 0f;
            Debug.Log("Active Circle");
            Invoke("RemoveExplosionCircle", 0.25f);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<SpriteRenderer>().enabled = false;
            DealExplosionDamage();
        }
        else
        {
            bulletPool.Release(this);
        }
    }

    private void RemoveExplosionCircle()
    {
        bulletPool.Release(this);
        m_explosionCircle.SetActive(false);
        Debug.Log("Deactive Circle");
    }

    private void BulletMovement()
    {
        if(follower)
        {
            TargetNearbyEnemy();
        }

        GetComponent<Rigidbody2D>().velocity = speed * direction;
        m_distanceTravelled += speed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));  // Adjust angle to match bullet sprite orientation
    }


    private void TargetNearbyEnemy()
    {
        if(m_closestEnemy == null)
        {
            return;
        }

        Vector3 EnemyDirection = m_closestEnemy.transform.position - transform.position;

        Vector3 NewDirection = Vector3.RotateTowards(direction, EnemyDirection, 5 * Time.deltaTime, Time.deltaTime);
        direction = NewDirection;
    }

    private void HitTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage * damageMultiplier);
        }
        if(!piercing)
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
        for(int i = 0; i < m_enemiesInExplosionTrigger.Count; i++)
        {
            m_enemiesInExplosionTrigger[i].GetComponent<EnemyController>().TakeDamage(damage * damageMultiplier);
        }
    }   

    private void FollowerTriggerEntered(Collider2D collision)
    {
        if(collision.gameObject.tag != "Enemy")
        {
            return;
        }

        Vector3 EnemyDirection = collision.gameObject.transform.position - transform.position;
        if(Vector3.Angle(direction, EnemyDirection) > 80)
        {
            return;
        }

        if(m_closestEnemy == null)
        {
            m_closestEnemy = collision.gameObject;
            return;
        }
        if(Vector3.Distance(transform.position, collision.transform.position) < Vector3.Distance(transform.position, m_closestEnemy.transform.position))
        {
            m_closestEnemy = collision.gameObject; 
            return;
        }
        
    }

    private void FollowerTriggerExited(Collider2D collision)
    {
        if(collision.gameObject == m_closestEnemy) 
        {
            m_closestEnemy = null;
        }
    }
    private void ExplosionTriggerEntered(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            m_enemiesInExplosionTrigger.Add(collision.gameObject);
            return;
        }        
        
    }
  
    private void ExplosionTriggerExited(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            m_enemiesInExplosionTrigger.Remove(collision.gameObject);
            return;
        }
    }

    public void RespawnBullet(Bullet OriginBullet)
    {
        GetComponent<SpriteRenderer>().enabled = true;
        m_distanceTravelled = 0;
        size = OriginBullet.size;
        damage = OriginBullet.damage;
        damageMultiplier = OriginBullet.damageMultiplier;
        range = OriginBullet.range;
        speed = OriginBullet.speed;
        piercing = OriginBullet.piercing;
        follower = OriginBullet.follower;   
        explodesOnHit = OriginBullet.explodesOnHit;
        transform.localScale = Vector3.one * size;

        m_bulletEnded = false;
        InitTriggers();
    }

}
