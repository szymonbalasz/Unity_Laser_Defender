using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupFloat : MonoBehaviour
{
    [SerializeField] float padding = 2f;
    [SerializeField] float moveSpeed = 0.3f;

    float xMin, xMax, yMin, yMax;
    Vector2 targetPosition;
    
    void Start()
    {
        SetUpMoveBoundaries();
        NewTarget();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if ((Vector2)transform.position == targetPosition) { NewTarget(); }
        var movementThisFrame = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
    }

    private void NewTarget()
    {
        targetPosition = targetPosition = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
}
