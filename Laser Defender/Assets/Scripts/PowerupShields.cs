using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupShields : MonoBehaviour
{
    [SerializeField] GameObject pickupVFX;
    [SerializeField] GameObject scoreVFX;
    [SerializeField] float destroyTime = 2f;
    [SerializeField] int score = 1000;
    [SerializeField] AudioClip pickupSFX;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (!player.ShieldStatus())
        {
            player.ShieldActivate();
            Destroy(gameObject);
            GameObject VFX = Instantiate(pickupVFX, transform.position, Quaternion.identity) as GameObject;
            Destroy(VFX, destroyTime);
        }
        else
        {
            AudioSource.PlayClipAtPoint(pickupSFX, Camera.main.transform.position);
            FindObjectOfType<GameSession>().AddToScore(score);
            Destroy(gameObject);
            GameObject VFX = Instantiate(scoreVFX, transform.position, Quaternion.identity) as GameObject;
            Destroy(VFX, destroyTime);
        }

    }
}
