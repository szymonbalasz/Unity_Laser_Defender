using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] AudioClip powerUp;
    [SerializeField] AudioClip powerDown;
    [SerializeField] float volume = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PowerDown()
    {
        AudioSource.PlayClipAtPoint(powerDown, Camera.main.transform.position, volume);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void PowerUp()
    {
        AudioSource.PlayClipAtPoint(powerUp, Camera.main.transform.position, volume);
        GetComponent<SpriteRenderer>().enabled = true;
    }
}
