using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public int numberOfAsteroidsToSpawn = 10;
    public float minYPosition = 5f;
    public GameObject spaceship; // Reference to the spaceship GameObject
    public float minDistanceFromSpaceship = 10f; // Minimum distance from the spaceship

    private void Start()
    {
        // Get the size of the plane GameObject
        Vector3 planeSize = GetComponent<Renderer>().bounds.size;

        for (int i = 0; i < numberOfAsteroidsToSpawn; i++)
        {
            Vector3 randomPosition = GenerateRandomPosition(planeSize);

            // Ensure that the asteroid is not too close to the spaceship
            while (Vector3.Distance(randomPosition, spaceship.transform.position) < minDistanceFromSpaceship)
            {
                randomPosition = GenerateRandomPosition(planeSize);
            }

            randomPosition.y = minYPosition; // Set the Y position to 5 units

            GameObject asteroid = Instantiate(asteroidPrefab, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GenerateRandomPosition(Vector3 planeSize)
    {
        Vector3 randomPosition = new Vector3(
            Random.Range(-planeSize.x / 2, planeSize.x / 2), // X position within the plane
            minYPosition, // Y position at 5 units
            Random.Range(-planeSize.z / 2, planeSize.z / 2) // Z position within the plane
        );

        return randomPosition;
    }
}
