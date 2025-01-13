using UnityEngine;

public class SeekingBehaviour : MonoBehaviour
{
    [SerializeField] GameObject character;
    [SerializeField] GameObject target;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Seeking(character, target);
        }
    }

    void Seeking(GameObject character, GameObject target)
    {
        // Use our GetRandomPointInViewport function and store it in a Vector3 for both our player and our target - This is so they both have different spawn locations
        Vector3 randomPointInViewportCharacter = GetRandomPointInViewport();
        Vector3 randomPointInViewportTarget = GetRandomPointInViewport();

        // When this function is called (when 1 is pressed) spawn a copy of the target at the given coordinates
        GameObject spawnedTarget = Instantiate(this.target, randomPointInViewportTarget, Quaternion.identity);

        // When this function is called (when 1 is pressed) spawn a copy of the character at the given coordinates
        GameObject spawnedCharacter = Instantiate(this.character, randomPointInViewportCharacter, Quaternion.identity);

        // All directionToTarget does is calculates the direction to the target
        Vector3 directionToTarget = (spawnedTarget.gameObject.transform.position - spawnedCharacter.gameObject.transform.position).normalized;
        Quaternion rotationToTarget = Quaternion.FromToRotation(new Vector3(0, 0, 1), directionToTarget);
        spawnedCharacter.transform.rotation = rotationToTarget;
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
}
