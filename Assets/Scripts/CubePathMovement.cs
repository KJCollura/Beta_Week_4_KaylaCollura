using UnityEngine;

public class CubePathMovement : MonoBehaviour
{
    public float speed = 2f; // Movement speed
    public Transform player;
    public CharacterController playerController;
    public float interactionDistance = 3.0f;
    public float pushForce = 10f;
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private bool isMoving = false;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true; // **Fix: Disable external physics forces**
            rb.useGravity = false; // Cube follows waypoints only
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent rolling
        }

        if (waypoints.Length == 0)
        {
            Debug.LogError("Waypoints array is empty! Assign waypoints in the Inspector.");
        }
    }

    void Update()
    {
        if (player == null || playerController == null) return;

        HandlePlayerMovement();
        HandleCubeInteraction();
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            MoveCubeAlongPath();
        }
    }

    void HandlePlayerMovement()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            move = speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            move = -speed * Time.deltaTime;
        }

        Vector3 movement = player.transform.forward * move;
        playerController.Move(movement);
    }

    void HandleCubeInteraction()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isMoving)
                {
                    Debug.Log("Cube movement started!");
                    isMoving = true;
                }
                else
                {
                    Debug.Log("Cube movement stopped!");
                    isMoving = false;
                }
            }
        }
    }

    void MoveCubeAlongPath()
    {
        if (waypoints.Length == 0 || currentWaypointIndex >= waypoints.Length) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        Debug.Log($"Moving towards: {targetWaypoint.name}, Current Index: {currentWaypointIndex}, Cube Position: {rb.position}");

        // Move cube towards the next waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.fixedDeltaTime);

        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        Debug.Log($"Distance to {targetWaypoint.name}: {distanceToWaypoint}");

        // **Fix: Increased detection threshold & forced waypoint transition**
        if (distanceToWaypoint < 1.5f) // Increased threshold to 1.5
        {
            Debug.Log($"Reached waypoint {currentWaypointIndex}: {targetWaypoint.name}");

            // **Forcefully advance to the next waypoint**
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                Debug.Log("Last waypoint reached. Stopping movement.");
                isMoving = false;
            }
        }
    }
}