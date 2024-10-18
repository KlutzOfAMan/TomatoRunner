using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public PlayerMovement1 ps;
    public Health hs;
    #region Singleton

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    #endregion

    public void OnDie()
    {
        Application.Quit();
    }

}
