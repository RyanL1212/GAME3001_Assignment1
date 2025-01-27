using UnityEngine;

public class FleeingBehaviour : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject enemyPrefab;
    GameObject spawnedCharacter;
    GameObject spawnedEnemy;

    public bool hasEntitiesSpawned = false;
    float speed = 5f;

    [SerializeField] ArrivalBehaviour arrivalBehaviour;
    [SerializeField] SeekingBehaviour seekingBehaviour;
    [SerializeField] AvoidanceBehaviour avoidanceBehaviour;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DestroyEntities();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Destroy entities from other scripts if they are already spawned in
            if (arrivalBehaviour.hasEntitiesSpawned)
            {
                arrivalBehaviour.DestroyEntities();
            }
            else if (seekingBehaviour.hasEntitiesSpawned)
            {
                seekingBehaviour.DestroyEntities();
            }
            else if (avoidanceBehaviour.hasEntitiesSpawned)
            {
                avoidanceBehaviour.DestroyEntities();
            }

            if (hasEntitiesSpawned)
            {
                DestroyEntities();
            }
            SpawnEntities();
        }

        if (hasEntitiesSpawned)
        {
            MoveCharacterAwayFromEnemy();
        }
    }

    void MoveCharacterAwayFromEnemy()
    {
        if (spawnedCharacter != null && spawnedEnemy != null)
        {
            // Calculate the direction away from the enemy
            Vector3 directionAwayFromEnemy = (spawnedCharacter.transform.position - spawnedEnemy.transform.position).normalized;

            // Calculate the new position
            Vector3 newPosition = spawnedCharacter.transform.position + directionAwayFromEnemy * speed * Time.deltaTime;

            // Check if the character would leave the screen
            Vector3 viewportPosition = Camera.main.WorldToViewportPoint(newPosition);
            if (viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1)
            {
                // Stop the character if it reaches the edge of the screen
                return;
            }

            spawnedCharacter.transform.position = newPosition;

            // Rotate the character to face away from the enemy
            Quaternion rotationAwayFromEnemy = Quaternion.LookRotation(directionAwayFromEnemy);
            spawnedCharacter.transform.rotation = rotationAwayFromEnemy;
        }
    }

    void SpawnEntities()
    {
        // Spawn the character and enemy at random positions in viewport
        Vector3 randomPointInViewportCharacter = GetRandomPointInViewport();
        Vector3 randomPointInViewportEnemy = GetRandomPointInViewport();

        spawnedCharacter = Instantiate(characterPrefab, randomPointInViewportCharacter, Quaternion.identity);
        spawnedEnemy = Instantiate(enemyPrefab, randomPointInViewportEnemy, Quaternion.identity);

        hasEntitiesSpawned = true;
    }

    Vector3 GetRandomPointInViewport()
    {
        Vector3 viewportPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 15f);
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(viewportPoint);
        worldPoint.y = 1f;

        return worldPoint;
    }

    public void DestroyEntities()
    {
        if (spawnedCharacter != null)
        {
            Destroy(spawnedCharacter);
        }
        if (spawnedEnemy != null)
        {
            Destroy(spawnedEnemy);
        }
        hasEntitiesSpawned = false;
    }
}

