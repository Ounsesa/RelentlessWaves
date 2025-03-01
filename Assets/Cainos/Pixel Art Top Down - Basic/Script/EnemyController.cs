using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    public EnemyTypes enemyType;
    public float health = 1;
    public float speed = 0.005f;
    public int damage = 1;
    public float dropChance = 0;
    public int scoreOnDeath = 0;
    public IObjectPool<EnemyController> enemyPool;

    private float m_initialHealth = 1;
    private GameObject m_player;
    private Animator m_animator;
    private BoxCollider2D m_boxCollider;
    private bool m_dead = false;




    #endregion

    public void SetPool(IObjectPool<EnemyController> pool) {  enemyPool = pool; }

    // Start is called before the first frame update
    void Start()
    {
        m_player = GameplayManager.instance.player.gameObject;
        m_animator = GetComponent<Animator>();
        m_boxCollider= GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_dead)
        {
            if (transform.position.x - m_player.transform.position.x > 0) { transform.rotation = Quaternion.Euler(0, 180, 0); }
            else { transform.rotation = Quaternion.Euler(0, 0, 0); }
            transform.position = Vector3.MoveTowards(transform.position, m_player.transform.position, speed * Time.deltaTime);
        }

    }

    public void Init(EnemyValues enemyValues)
    {
        speed = enemyValues.speed;
        damage = enemyValues.damage;
        dropChance = enemyValues.dropChance;
        scoreOnDeath = enemyValues.scoreOnDeath;
        m_initialHealth = enemyValues.health;
    }

    public void TakeDamage(float damage)
    {
        if(m_dead)
        {
            return;
        }
        health -= damage;
        m_animator.SetTrigger("Hit");

        if (health <= 0)
        {
            m_dead = true;
            m_animator.SetBool("Dead", true);
            m_boxCollider.enabled = false;

            Invoke("RegisterDead", 2);

        }
    }

    public void Respawn()
    {
        health = m_initialHealth;
        m_dead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void RegisterDead()
    {
        try
        {
            enemyPool.Release(this);
        }
        catch
        {
            Debug.LogError("Enemy Already Released From Pool");
        }
    }
}
