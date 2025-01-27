using UnityEngine;

public class SeekingBehaviour : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab; // Character prefab
    [SerializeField] GameObject targetPrefab; // Target prefab
    GameObject spawnedCharacter; // Reference to the spawned character
    GameObject spawnedTarget; // Reference to the spawned target

    public bool hasEntitiesSpawned = false;
    float speed = 5f;

    [SerializeField] ArrivalBehaviour arrivalBehaviour;
    [SerializeField] FleeingBehaviour fleeingBehaviour;
    [SerializeField] AvoidanceBehaviour avoidanceBehaviour;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            DestroyEntities();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Destroy entities from other scripts if they are already spawned in
            if (arrivalBehaviour.hasEntitiesSpawned)
            {
                arrivalBehaviour.DestroyEntities();
            }
            else if (fleeingBehaviour.hasEntitiesSpawned)
            {
                fleeingBehaviour.DestroyEntities();
            }
            else if (avoidanceBehaviour.hasEntitiesSpawned)
            {
                avoidanceBehaviour.DestroyEntities();
            }

            if (hasEntitiesSpawned == true)
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
            Vector3 directionToTarget = (spawnedTarget.transform.position - spawnedCharacter.transform.position).normalized;
            spawnedCharacter.transform.position += directionToTarget * speed * Time.deltaTime;
        }
    }

    void SpawnEntities()
    {
        // Use the GetRandomPointInViewport function and store it in a Vector3 for both our player and our target - This is so they both have different spawn locations
        Vector3 randomPointInViewportCharacter = GetRandomPointInViewport();
        Vector3 randomPointInViewportTarget = GetRandomPointInViewport();

        // When this function is called (when 1 is pressed) spawn a copy of the target at the given coordinates
        spawnedTarget = Instantiate(targetPrefab, randomPointInViewportTarget, Quaternion.identity);

        // When this function is called (when 1 is pressed) spawn a copy of the character at the given coordinates
        spawnedCharacter = Instantiate(characterPrefab, randomPointInViewportCharacter, Quaternion.identity);

        // All directionToTarget does is calculates the direction to the target by subtracting the characters position from the targets position
        Vector3 directionToTarget = (spawnedTarget.gameObject.transform.position - spawnedCharacter.gameObject.transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
        spawnedCharacter.transform.rotation = rotationToTarget;

        hasEntitiesSpawned = true;
    }

    // This function is used to get a random point from the viewport.
    Vector3 GetRandomPointInViewport()
    {
        Vector3 viewportPoint = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 15f);

        // Convert the viewport point to a world space position
        Vector3 worldPoint = Camera.main.ViewportToWorldPoint(viewportPoint);

        // Adjust the Y position to match the floor level so that the character always spawns on the floor/plane
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
