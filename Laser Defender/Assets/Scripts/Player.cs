using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //config params
    [Header("Sprites")]
    [SerializeField] Sprite playerSprite;
    [SerializeField] Sprite playerDamagedSprite;

    [Header("Player Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;

    [Header("Health and Shields")]
    [SerializeField] bool godMode = false;
    [SerializeField] float health = 200f;
    [SerializeField] bool shieldStatus = false;
    [SerializeField] GameObject shield;

    [Header("Player Weapons")]
    [SerializeField] bool hasLaser = true;
    [SerializeField] int numLasers = 1;
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float laserSpeed = 20f;
    [SerializeField] float laserFiringSpeed = 0.1f;    
    [SerializeField] bool hasRockets = false;
    [SerializeField] GameObject rocketPrefab = default;
    [SerializeField] float rocketSpeed = 7f;
    int rocketCount = 0;

    [Header("SFX")]
    [SerializeField] AudioClip deathSFX = default;
    [SerializeField] AudioClip fireSFX = default;
    [SerializeField] [Range(0,1)] float fireVol = 1f;
    [SerializeField] [Range(0, 3)] float deathVol = 1f;
    [SerializeField] AudioClip rocketSFX = default;

    [Header("VFX")]
    [SerializeField] GameObject deathVFX = default;
    [SerializeField] float explosionTime = 3f;

    Coroutine firingCoroutine;
    AudioSource audioSource;
    Sprite sprite;

    //state variables
    float xMin, xMax, yMin, yMax;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaires();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            FireLaser();
            if (hasRockets && rocketCount%4 == 0) { FireRocket(); }
            yield return new WaitForSeconds(laserFiringSpeed);
        }

    }

    private void FireLaser()
    {
        audioSource.PlayOneShot(fireSFX, fireVol);
        rocketCount++;
        if (numLasers == 1)
        {            
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        }
        else if (numLasers == 2)
        {
            var loc1 = new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z);
            var loc2 = new Vector3(transform.position.x + 0.25f, transform.position.y, transform.position.z);
            GameObject laser1 = Instantiate(laserPrefab, loc1, Quaternion.identity) as GameObject; 
            GameObject laser2 = Instantiate(laserPrefab, loc2, Quaternion.identity) as GameObject;
            laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
            laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
        }
    }

    private void FireRocket()
    {
        audioSource.PlayOneShot(rocketSFX, fireVol);
        GameObject rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity) as GameObject;
        rocket.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rocketSpeed);
        rocketCount = 0;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetupMoveBoundaires()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (!shieldStatus)
        {
            if (!godMode) { health -= damageDealer.GetDamage(); }
            AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVol);
            damageDealer.Hit();
            GetComponent<SpriteRenderer>().sprite = playerDamagedSprite;
            if (health <= 0f)
            {
                FindObjectOfType<Level>().LoadGameOver();
                DestroyPlayer();
            }
        }
        else
        {
            ShieldDeactivate();
        }
        
    }

    public void ShieldActivate()
    {
        shield.GetComponent<Shield>().PowerUp();
        shieldStatus = true;
    }

    public void ShieldDeactivate()
    {
        shield.GetComponent<Shield>().PowerDown();        
        shieldStatus = false;
    }

    public bool ShieldStatus()
    {
        return shieldStatus;
    }

    private void DestroyPlayer()
    {
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathVol);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, explosionTime);
    }

    public float GetHealth()
    {
        return health;
    }

    public void AddHealth()
    {
        health += 100f;
        GetComponent<SpriteRenderer>().sprite = playerSprite;
    }

    public bool HasRockets()
    {
        return hasRockets;
    }

    public void AddRockets()
    {
        hasRockets = true;
    }

    public int NumberOfLasers()
    {
        return numLasers;
    }

    public void AddLaser()
    {
        numLasers = 2;
    }
}


