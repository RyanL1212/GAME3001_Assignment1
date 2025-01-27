using UnityEngine;

public class ArrivalBehaviour : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject targetPrefab;
    GameObject spawnedCharacter;
    GameObject spawnedTarget;

    public bool hasEntitiesSpawned = false;
    float speed = 4f; // Maximum speed
    float slowDownRadius = 2f; // Distance where the character starts slowing down
    float stopDistance = 0.5f; // Distance to stop at the target

    [SerializeField] FleeingBehaviour fleeingBehaviour;
    [SerializeField] SeekingBehaviour seekingBehaviour;
    [SerializeField] AvoidanceBehaviour avoidanceBehaviour;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DestroyEntities();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
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
            MoveCharacterTowardsTarget();
        }
    }

    void MoveCharacterTowardsTarget()
    {
        if (spawnedCharacter != null && spawnedTarget != null)
        {
            // Calculate the direction to the target
            Vector3 directionToTarget = (spawnedTarget.transform.position - spawnedCharacter.transform.position);

            // .magnitude calculates the distance between the vectors start point and end point
            float distanceToTarget = directionToTarget.magnitude;

            // Stop the character if it's close enough to the target
            if (distanceToTarget <= stopDistance)
            {
                return;
            }

            // Normalize the direction
            Vector3 direction = directionToTarget.normalized;

            // Adjust speed based on distance
            if (distanceToTarget <= slowDownRadius)
            {
                speed = 2f;
            }

            // Move the character towards the target
            spawnedCharacter.transform.position += direction * speed * Time.deltaTime;

            // Rotate the character to face the target
            Quaternion rotationToTarget = Quaternion.LookRotation(direction);
            spawnedCharacter.transform.rotation = rotationToTarget;
        }
    }

    void SpawnEntities()
    {
        speed = 4f;

        // Spawn the character and target at random positions
        Vector3 randomPointInViewportCharacter = GetRandomPointInViewport();
        Vector3 randomPointInViewportTarget = GetRandomPointInViewport();

        spawnedCharacter = Instantiate(characterPrefab, randomPointInViewportCharacter, Quaternion.identity);
        spawnedTarget = Instantiate(targetPrefab, randomPointInViewportTarget, Quaternion.identity);

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
        if (spawnedTarget != null)
        {
            Destroy(spawnedTarget);
        }
        hasEntitiesSpawned = false;
    }
}

