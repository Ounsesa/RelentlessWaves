using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private List<GameObject> m_heartImages = new List<GameObject>();

    [SerializeField]
    [Tooltip("In order from full -> half -> empty")]
    private List<Sprite> m_heartSprites = new List<Sprite>();
    #endregion

    public void SetHealth(int health)
    {
        int NumberOfFullHearths = health / 2;
        bool HasHalfHearth = health % 2 != 0 && health > 0;

        for (int i = 0; i < m_heartImages.Count; i++)
        {
            if (i < NumberOfFullHearths)
            {
                m_heartImages[i].GetComponent<Image>().sprite = m_heartSprites[0];
            }
            else if (HasHalfHearth)
            {
                HasHalfHearth = false;
                m_heartImages[i].GetComponent<Image>().sprite = m_heartSprites[1];
            }
            else
            {
                m_heartImages[i].GetComponent<Image>().sprite = m_heartSprites[2];
            }
        }
    }
}
