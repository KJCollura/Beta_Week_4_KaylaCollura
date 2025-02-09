using UnityEngine;

public class KC_Raycast : MonoBehaviour
{
    public float range = 7;
    public float shrinkSpeed = 2f; // Speed of shrinking
    public float moveDownSpeed = 1f; // Speed of moving down
    public float minScale = 0.1f; // Minimum allowed scale

    void Start()
    {
        Debug.Log("Raycast script has started");
    }

    void Update()
    {
        Ray playerRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * range, Color.red);

        if (Physics.Raycast(playerRay, out RaycastHit hit, range))
        {
            if (hit.collider.CompareTag("Static"))
            {
                Debug.Log("My raycast hit a STATIC object");
            }
            else if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("My raycast hit an ENEMY object");

                // Get the enemy transform
                Transform enemyTransform = hit.collider.transform;

                // Shrink the enemy
                Vector3 newScale = enemyTransform.localScale - Vector3.one * shrinkSpeed * Time.deltaTime;
                
                // Move the enemy downward
                Vector3 newPosition = enemyTransform.position - new Vector3(0, moveDownSpeed * Time.deltaTime, 0);

                // Ensure the scale doesn't become too small
                newScale = new Vector3(
                    Mathf.Max(newScale.x, minScale),
                    Mathf.Max(newScale.y, minScale),
                    Mathf.Max(newScale.z, minScale)
                );

                // Apply the transformations
                enemyTransform.localScale = newScale;
                enemyTransform.position = newPosition;
            }
        }
    }
}