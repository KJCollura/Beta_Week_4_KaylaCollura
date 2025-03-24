using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public float rayDistance = 3f;
    public LayerMask clueLayer;  // Optional: Assign specific layer for clues

    private DetectiveGameManager gameManager;

    public class DetectiveGameManager : MonoBehaviour
{
    // Existing code

    public void TryInteractWithClue(GameObject clue)
    {
        // Implement the interaction logic here
        Debug.Log("Interacted with clue: " + clue.name);
    }
}

    void Start()
    {
        gameManager = FindFirstObjectByType<DetectiveGameManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(transform.position, transform.forward);  // Cast from camera or FPC
            if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, clueLayer))
            {
                if (hit.collider.CompareTag("Clue"))
                {
                    gameManager?.TryInteractWithClue(hit.collider.gameObject);
                }
            }
        }
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance, Color.red);
    }
}

