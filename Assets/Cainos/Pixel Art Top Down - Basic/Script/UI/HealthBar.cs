using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField]
    List<GameObject> HeartImages = new List<GameObject>();

    [SerializeField]
    List<Sprite> HeartSprites = new List<Sprite>();

    public void SetHealth(int health)
    {
        int NumberOfFullHearths = health / 2;
        bool hasHalfHearth = health % 2 != 0 && health > 0;

        for (int i = 0; i < HeartImages.Count; i++)
        {
            if (i < NumberOfFullHearths)
            {
                HeartImages[i].GetComponent<Image>().sprite = HeartSprites[0];
            }
            else if (hasHalfHearth)
            {
                hasHalfHearth = false;
                HeartImages[i].GetComponent<Image>().sprite = HeartSprites[1];
            }
            else
            {
                HeartImages[i].GetComponent<Image>().sprite = HeartSprites[2];
            }
        }
    }
}
