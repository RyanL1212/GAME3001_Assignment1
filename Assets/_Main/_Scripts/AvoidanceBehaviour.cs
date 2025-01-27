using UnityEngine;

public class AvoidanceBehaviour : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject targetPrefab; 
    [SerializeField] GameObject enemyPrefab;
    GameObject spawnedCharacter;
    GameObject spawnedTarget;
    GameObject spawnedEnemy; 

    public bool hasEntitiesSpawned = false;
    [SerializeField] float speed = 4f;
    float avoidanceRadius = 5f;
    float avoidanceStrength = 1f;

    float slowDownRadius = 2f;
    float stopDistance = 0.5f;

    [SerializeField] FleeingBehaviour fleeingBehaviour;
    [SerializeField] SeekingBehaviour seekingBehaviour;
    [SerializeField] ArrivalBehaviour arrivalBehaviour;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DestroyEntities();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            // Destroy entities from other scripts if they are already spawned in
            if (fleeingBehaviour.hasEntitiesSpawned)
            {
                fleeingBehaviour.DestroyEntities();
            }
            else if (seekingBehaviour.hasEntitiesSpawned)
            {
                seekingBehaviour.DestroyEntities();
            }
            else if (arrivalBehaviour.hasEntitiesSpawned)
            {
                arrivalBehaviour.DestroyEntities();
            }

            if (hasEntitiesSpawned)
            {
                DestroyEntities();
            }
            SpawnEntities();
        }

        if (hasEntitiesSpawned)
        {
            MoveCharacterTowardsTarget();
        }
    }

    void MoveCharacterTowardsTarget()
    {
        if (spawnedCharacter != null && spawnedTarget != null && spawnedEnemy != null)
        {
            // Calculate the direction to the target
            Vector3 directionToTarget = (spawnedTarget.transform.position - spawnedCharacter.transform.position);

            // Calculate the direction to the enemy
            Vector3 directionToEnemy = (spawnedEnemy.transform.position - spawnedCharacter.transform.position);

            // Calculate the distance to the enemy and target
            float distanceToEnemy = directionToEnemy.magnitude;
            float distanceToTarget = directionToTarget.magnitude;

            // Stop the character if it's close enough to the target
            if (distanceToTarget <= stopDistance)
            {
                return;
            }

            // Slow down the character as it reaches the target
            if (distanceToTarget <= slowDownRadius)
            {
                speed = 2f;
            }

            Vector3 direction = directionToTarget.normalized;

            // If the enemy is too close, steer away
            if (distanceToEnemy < avoidanceRadius)
            {
                directionToEnemy.Normalize();

                // The idea I had here was to create a new vector3 that is 90 degrees from the enemy's direction.
                // This helps the character be able to avoid the enemy by steering around it.
                Vector3 avoidanceDirection = new Vector3(directionToEnemy.z, 0f, -directionToEnemy.x);

                direction = direction + avoidanceDirection * avoidanceStrength;

                // Normalize the result
                direction.Normalize();
            }

            // Move the character towards the target while avoiding the enemy
            spawnedCharacter.transform.position = spawnedCharacter.transform.position + direction * speed * Time.deltaTime;

            // Rotate the character to face the movement direction
            Quaternion rotationToTarget = Quaternion.LookRotation(direction);
            spawnedCharacter.transform.rotation = rotationToTarget;
        }
    }

    void SpawnEntities()
    {
        speed = 4f;

        // Spawn the target and character at random locations
        Vector3 randomPointInViewportCharacter = GetRandomPointInViewport();
        Vector3 randomPointInViewportTarget = GetRandomPointInViewport();

        // Instantiate target and character at the calculated positions
        spawnedTarget = Instantiate(targetPrefab, randomPointInViewportTarget, Quaternion.identity);
        spawnedCharacter = Instantiate(characterPrefab, randomPointInViewportCharacter, Quaternion.identity);

        // Place the enemy between the character and the target
        Vector3 enemyPosition = CalculateEnemyPosition(spawnedCharacter.transform.position, spawnedTarget.transform.position);
        spawnedEnemy = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);

        hasEntitiesSpawned = true;
    }

    // I created this function to calculate the enemy's position between the character and the target
    Vector3 CalculateEnemyPosition(Vector3 characterPos, Vector3 targetPos)
    {
        // Place the enemy halfway between the character and the target
        return Vector3.Lerp(characterPos, targetPos, 0.5f);
    }

    // Function to get a random point in the viewport
    Vector3 GetRandomPointInViewport()
    {
        Vector3 viewportPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 15f);

        // Convert the viewport point to world space
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(viewportPoint);

        // Adjust the Y position to match the floor level
        worldPoint.y = 1f;

        return worldPoint;
    }

    public void DestroyEntities()
    {
        if (spawnedCharacter != null)
        {
            Destroy(spawnedCharacter);
        }
        if (spawnedTarget != null)
        {
            Destroy(spawnedTarget);
        }
        if (spawnedEnemy != null)
        {
            Destroy(spawnedEnemy);
        }
        hasEntitiesSpawned = false;
    }
}