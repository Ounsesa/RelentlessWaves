using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 1;
    private GameObject player;
    public float speed = 0.005f;
    Animator animator;
    BoxCollider2D boxCollider;
    EnemyTypes EnemyType;

    private bool Dead = false;

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
        if(!Dead)
        {
            if (transform.position.x - player.transform.position.x > 0) { transform.rotation = Quaternion.Euler(0, 180, 0); }
            else { transform.rotation = Quaternion.Euler(0, 0, 0); }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        
    }

    public void Init(EnemyTypes enemyType)
    {
        EnemyType = enemyType;
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
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
        gameObject.SetActive(true);
        health = 1;
        Dead = false;
        GetComponent<BoxCollider2D>().enabled = true;
    }

    private void RegisterDead()
    {
        SpawnerController.m_instance.RegisterDead(EnemyType, gameObject);
    }
}
