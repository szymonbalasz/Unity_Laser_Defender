using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> powerups;
    [SerializeField] bool looping = true;
    [SerializeField] float timeBetweenPowerups = 15f;
    [SerializeField] float padding = 2f;

    float xMin, xMax, yMin, yMax;
    GameObject[] existingPowerups;

    IEnumerator Start()
    {
        SetUpBoundaries();
        do
        {
            yield return StartCoroutine(SpawnPowerUp());
        } while (looping);
    }

    private IEnumerator SpawnPowerUp()
    {
        DestroyOtherPowerups();
        var powerup = Instantiate(powerups[SelectPowerup()], PowerupLocation(), Quaternion.identity);
        yield return new WaitForSeconds(timeBetweenPowerups);
    }

    private int SelectPowerup()
    {
        return Random.Range(0, powerups.Count);
    }

    private void SetUpBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private Vector2 PowerupLocation()
    {
        return new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
    }

    private void DestroyOtherPowerups()
    {
        existingPowerups = GameObject.FindGameObjectsWithTag("Powerup");

        foreach (GameObject powerup in existingPowerups)
        {
            Destroy(powerup);
        }
    }
}
