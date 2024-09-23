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
    public float damage = 1;
    Animator animator;
    BoxCollider2D boxCollider;

    [SerializeField]
    public EnemyTypes EnemyType;

    NavMeshAgent agent;

    private bool Dead = false;

    private IObjectPool<EnemyController> EnemyPool;

    public void SetPool(IObjectPool<EnemyController> pool) {  EnemyPool = pool; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        boxCollider= GetComponent<BoxCollider2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = speed;
        agent.radius = boxCollider.size.x/2;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Dead)
        {
            agent.SetDestination(player.transform.position);
            if (transform.position.x - player.transform.position.x > 0) { transform.rotation = Quaternion.Euler(0, 180, 0); }
            else { transform.rotation = Quaternion.Euler(0, 0, 0); }
        }

    }

    public void Init(EnemyValues enemyValues)
    {
        speed = enemyValues.speed;
        InitialHealth = enemyValues.health;
        damage = enemyValues.damage;
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
            agent.updatePosition = false;
            Dead = true;
            animator.SetBool("Dead", true);
            boxCollider.enabled = false;

            Invoke("RegisterDead", 2);

        }
    }

    public void Respawn()
    {
        if(agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
        agent.updatePosition = true;
        health = InitialHealth;
        Dead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void RegisterDead()
    {
        EnemyPool.Release(this);
    }
}
