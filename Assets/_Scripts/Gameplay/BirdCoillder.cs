using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdCoillder : MonoBehaviour
{

    public PlayerController controller;
    private bool playerDead;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerDead)
            return;

        if (collision.CompareTag("Flying Obstacle"))
        {
            playerDead = true;
            controller.Dead(true);
        }
    }

}