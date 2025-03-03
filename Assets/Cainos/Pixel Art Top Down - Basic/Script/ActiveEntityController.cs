using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEntityController : MonoBehaviour
{
    public static ActiveEntityController instance;
    public List<EnemyController> enemiesActive;
    public List<Bullet> bulletsActive;
    public List<PowerUp> powerUpsActive;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        instance = this;
    }

    private void Start()
    {
        GameManager.instance.onRestartPressed.AddListener(Restart); 
    }


    void Restart()
    {
        for (int i = enemiesActive.Count - 1; i >= 0; i--)
        {
            enemiesActive[i].enemyPool?.Release(enemiesActive[i]);
        }
        enemiesActive.Clear();

        for (int i = bulletsActive.Count - 1; i >= 0; i--)
        {
            try
            {
                bulletsActive[i].bulletPool?.Release(bulletsActive[i]);
            }
            catch
            {
               
            }
        }
        bulletsActive.Clear();

        for (int i = powerUpsActive.Count - 1; i >= 0; i--)
        {
            Destroy(powerUpsActive[i].gameObject);
        }
        powerUpsActive.Clear();
    }


    public void AddActiveEnemy(EnemyController enemy)
    {
        enemiesActive.Add(enemy);
    }

    public void RemoveActiveEnemy(EnemyController enemy) 
    {
        enemiesActive.Remove(enemy);
    }

    public void AddActiveBullet(Bullet bullet)
    {
        bulletsActive.Add(bullet);
    }

    public void RemoveActiveBullet(Bullet bullet)
    {
        bulletsActive.Remove(bullet);
    }

    public void AddActivePowerUp(PowerUp powerUp)
    {
        powerUpsActive.Add(powerUp);
    }

    public void RemoveActivePowerUp(PowerUp powerUp)
    {
        powerUpsActive.Remove(powerUp);
    }

}
