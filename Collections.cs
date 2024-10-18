using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collections : MonoBehaviour
{

    //THIS SCRIPT IS FOR COINS, COLLECTABLES AND OTHER STATS THAT WILL BE TRACKED THROUGHOUT THE GAME
    public int CoinCounter;
    public float TomatoSavedInLevel;


    // Start is called before the first frame update
    void Start()
    {
        TomatoSavedInLevel = 0f;
        CoinCounter = 0;
    }

  
}
