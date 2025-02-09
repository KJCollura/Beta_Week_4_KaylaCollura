using UnityEngine;

public class Cube_Movement : MonoBehaviour
{
    public float speed = 5f; // Speed of movement
    public Transform player; // Reference to the first-person controller
    public CharacterController playerController; // Character Controller for the player
    public Rigidbody cubeRb; // Rigidbody for the cube
    public float interactionDistance = 1.0f; // Distance to interact with cube
    public float pushForce = 10f; // Strength of push force

    void Start()
    {
        if (cubeRb == null)
            cubeRb = GetComponent<Rigidbody>();
        
        if (cubeRb != null)
        {
            cubeRb.isKinematic = false; // Ensure Rigidbody is not kinematic
            cubeRb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation; // Keep cube on the plane
        }
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
        
        Vector3 movement = player.transform.forward * move;
        
        if (playerController != null)
        {
            playerController.Move(movement); // Move player
        }
        
        if (cubeRb != null && Vector3.Distance(player.position, cubeRb.position) <= interactionDistance)
        {
            Vector3 pushDirection = (cubeRb.position - player.position).normalized;
            pushDirection.y = 0; // Ensure movement stays on the plane
            cubeRb.linearVelocity = Vector3.zero; // Stop the cube before applying new force
            cubeRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
