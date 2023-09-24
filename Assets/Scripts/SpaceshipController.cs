using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float raycastDistance = 20f; // Increase the raycast distance
    
    public TMP_Text timerText;
    public Transform targetLocation;

    private bool hasReachedTarget = false;
    private float timeRemaining = 30f;
    private bool hasWon = false;

    void Update()
    {
        Vector3 directionToTarget = (targetLocation.position - transform.position).normalized;

        // Perform raycasts in multiple directions
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, raycastDistance);
        bool AsteroidInFront = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Asteroid"))
            {
                AsteroidInFront = true;
                break;
            }
        }

        hits = Physics.RaycastAll(transform.position, -transform.right, raycastDistance);
        bool AsteroidToLeft = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Asteroid"))
            {
                AsteroidToLeft = true;
                break;
            }
        }

        hits = Physics.RaycastAll(transform.position, transform.right, raycastDistance);
        bool AsteroidToRight = false;

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Asteroid"))
            {
                AsteroidToRight = true;
                break;
            }
        }

        if (!hasReachedTarget)
        {
            // No Asteroid detected, rotate towards the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move forward at a constant speed
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        if (AsteroidInFront)
        {
            // Rotate away from the Asteroid
            Vector3 avoidAsteroidDirection = Vector3.Cross(Vector3.up, Vector3.one);
            Quaternion targetRotation = Quaternion.LookRotation(avoidAsteroidDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (AsteroidToLeft && !AsteroidToRight)
        {
            // Rotate to the right to avoid the Asteroid on the left
            Quaternion targetRotation = Quaternion.LookRotation(transform.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else if (!AsteroidToLeft && AsteroidToRight)
        {
            // Rotate to the left to avoid the Asteroid on the right
            Quaternion targetRotation = Quaternion.LookRotation(-transform.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (timeRemaining > 0f && !hasWon)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        }
        if (timeRemaining <= 0f && !hasWon)
        {
            timerText.text = "Lose";
            hasReachedTarget = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == targetLocation.gameObject)
        {
            // Cube has collided with the target location, stop the movement
            hasReachedTarget = true;
            hasWon = true;
            timerText.text = "Win";
        }
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = seconds.ToString();
    }
}
