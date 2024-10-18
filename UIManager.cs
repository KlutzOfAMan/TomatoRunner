using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    public Collections coll;
    public PlayerMovement1 pm;
    public Image img;


    public TextMeshProUGUI CoinAmount;

    void Start()
    {
        img.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coll != null && CoinAmount != null)
        {
            CoinAmount.text = coll.CoinCounter.ToString();
        }
        else
        {
            Debug.LogWarning("coll or CoinAmount is not assigned in UIManager.");
        }


        if (pm.rollOnCoolDown == false)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }


    }
}
