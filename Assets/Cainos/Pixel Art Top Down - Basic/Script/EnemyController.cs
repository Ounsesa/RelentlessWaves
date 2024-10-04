using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class EnemyController : MonoBehaviour
{
    public float health = 1;
    private float InitialHealth = 1;
    private GameObject player;
    public float speed = 0.005f;
    public int damage = 1;
    Animator animator;
    BoxCollider2D boxCollider;

    [SerializeField]
    public EnemyTypes EnemyType;

    private bool Dead = false;

    public float dropChance = 0;
    public int scoreOnDeath = 0;


    public IObjectPool<EnemyController> EnemyPool;

    public void SetPool(IObjectPool<EnemyController> pool) {  EnemyPool = pool; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        boxCollider= GetComponent<BoxCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            if (transform.position.x - player.transform.position.x > 0) { transform.rotation = Quaternion.Euler(0, 180, 0); }
            else { transform.rotation = Quaternion.Euler(0, 0, 0); }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

    }

    public void Init(EnemyValues enemyValues)
    {
        speed = enemyValues.speed;
        InitialHealth = enemyValues.health;
        damage = enemyValues.damage;
        dropChance = enemyValues.dropChance;
        scoreOnDeath = enemyValues.scoreOnDeath;
    }

    public void TakeDamage(float damage)
    {
        if(Dead)
        {
            return;
        }
        health -= damage;
        animator.SetTrigger("Hit");

        if (health <= 0)
        {
            Dead = true;
            animator.SetBool("Dead", true);
            boxCollider.enabled = false;

            Invoke("RegisterDead", 2);

        }
    }

    public void Respawn()
    {
        health = InitialHealth;
        Dead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void RegisterDead()
    {
        try
        {
            EnemyPool.Release(this);
        }
        catch
        {

        }
    }
}
