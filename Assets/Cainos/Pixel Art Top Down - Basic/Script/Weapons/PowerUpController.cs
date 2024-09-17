using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public static PowerUpController m_instance;

    public GameObject m_gameObject;
    void Awake()
    {
        if (m_instance != null)
        {
            Destroy(m_instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        m_instance = this;
    }

    public void SpawnPowerUp(Vector3 position)
    {
        Instantiate(m_gameObject, position, Quaternion.identity);
    }
}
