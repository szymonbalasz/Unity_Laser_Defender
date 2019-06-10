using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float health = 100f;
    [SerializeField] int score = 50;

    [Header("Combat")]
    float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.3f;
    [SerializeField] float maxTimeBetweenShots = 3.5f;
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float laserSpeed = 10f;

    [Header("Effects")]
    [SerializeField] GameObject deathVFX = default;
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip deathSFX = default;
    [SerializeField] AudioClip fireSFX = default;
    [SerializeField] [Range(0, 1)] float fireVol = 1f;
    [SerializeField] [Range(0, 1)] float deathVol = 1f;

    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0f)
        {
            Fire();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
        audioSource.PlayOneShot(fireSFX, fireVol);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0f)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        FindObjectOfType<GameSession>().AddToScore(score);
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, explosionTime);
    }
}
