//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ReloadController : MonoBehaviour
//{

//    public GameObject m_canvas;

//    public UnityEngine.UI.Image m_fillImage;

//    float m_reloadTime = 0.0f;
//    float m_elapsedTime = 0.0f;
//    public WeaponController m_weaponController;
//    public bool m_isReloading = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_reloadTime = m_weaponController.reloadTime;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(m_isReloading)
//        {
//            m_fillImage.enabled = true;
//            m_elapsedTime += Time.deltaTime;
//            m_fillImage.fillAmount = m_elapsedTime / m_reloadTime;
//            if(m_elapsedTime >= m_reloadTime)
//            {
//                m_fillImage.enabled = false;
//                m_isReloading = false;
//                m_elapsedTime = 0.0f;
//            }
//        }
//    }
//}
