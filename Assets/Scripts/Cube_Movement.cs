using UnityEngine;

public class Cube_Movement : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    public Transform player; // Reference to the first-person controller
    public Rigidbody playerRb; // Rigidbody for the player
    public Rigidbody cubeRb; // Rigidbody for the cube
    public float interactionDistance = 1.0f; // Increased distance to interact with cube

    void Start()
    {
        if (cubeRb == null)
            cubeRb = GetComponent<Rigidbody>();
    }

    void Update()
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
        
        Vector3 movement = player.forward * move;
        
        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + movement);
        }
        
        if (cubeRb != null && Vector3.Distance(player.position, cubeRb.position) <= interactionDistance)
        {
            cubeRb.MovePosition(cubeRb.position + movement);
        }
    }
}