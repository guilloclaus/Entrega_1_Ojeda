using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    public static GameManager instance;

    public static GameObject playerObject;
    private static PlayerController playerControler;
    public static int scorePlayer = 0;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            playerObject = GameObject.FindGameObjectWithTag("Player");
            playerControler = playerObject.GetComponent<PlayerController>();
            PlayerEvents.onDead += PlayerDead;

            scorePlayer = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayerDead()
    {
        playerControler.Life = 0;
    }

    public void AddScore(int _scorePlayer)
    {
        scorePlayer += _scorePlayer;
    }
    public void AddPlayerShield(int _shield)
    {
        playerControler.GetComponent<PlayerController>().AddShield(_shield);
    }
    public void AddPlayerAttack(int _attack)
    {
        playerControler.GetComponent<PlayerController>().AddAttack(_attack);
    }
    public void AddPlayerLife(int _life)
    {
        playerControler.GetComponent<PlayerController>().AddLife(_life);
    }

}
