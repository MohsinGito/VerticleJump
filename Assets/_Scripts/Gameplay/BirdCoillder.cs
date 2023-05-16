using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities.Audio;

public class BirdCoillder : MonoBehaviour
{

    public PlayerController controller;
    private bool playerDead;

    private void Update()
    {
        transform.position = controller.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerDead)
            return;

        if (collision.CompareTag("Flying Obstacle"))
        {
            playerDead = controller.Dead();
            if (!playerDead)
            {
                VFXManager.Instance.DisplayVFX("Enemy Die Effect", collision.transform.position);
                collision.gameObject.SetActive(false); AudioController.Instance.PlayAudio(AudioName.Enemy_Hit);
            }
        }
    }

}