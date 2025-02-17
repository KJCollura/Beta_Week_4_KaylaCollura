using System.Collections;
using UnityEngine;

namespace KC_DoorScript
{
    [RequireComponent(typeof(AudioSource))]
    public class KC_DoorScript : MonoBehaviour
    {
        public bool open = false;
        public float smooth = 1.0f;
        private float doorOpenAngle = -90.0f;
        private float doorCloseAngle = 0.0f;

        public AudioSource asource;
        public AudioClip openDoor, closeDoor;

        private bool playerNearby = false;

        // Minimum score required to open door
        public int requiredScore = 50;

        // Collider to block the doorway
        public Collider doorCollider;

        void Start()
        {
            if (doorCollider != null) doorCollider.enabled = true;
            else Debug.LogWarning("Door Collider not assigned!");

            asource = GetComponent<AudioSource>();
        }

        void Update()
        {
            RotateDoor();

            // Check if player presses E
            if (playerNearby && Input.GetKeyDown(KeyCode.E))
            {
                int currentScore = KC_ScoreManager.instance?.GetScore() ?? -1;

                Debug.Log($"E pressed. Player Score: {currentScore}. Required: {requiredScore}");

                // Check if player has enough points
                if (currentScore >= requiredScore)
                {
                    Debug.Log("Score requirement met. Opening door.");
                    OpenDoor();
                }
                else
                {
                    Debug.LogWarning($"Insufficient points! Need {requiredScore} but have {currentScore}.");
                }
            }
        }

        void RotateDoor()
        {
            Quaternion targetRotation = open
                ? Quaternion.Euler(0, doorOpenAngle, 0)
                : Quaternion.Euler(0, doorCloseAngle, 0);

            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * 5 * smooth
            );
        }

        public void OpenDoor()
        {
            open = !open;
            Debug.Log(open ? "Door opening..." : "Door closing...");

            if (doorCollider != null)
            {
                doorCollider.enabled = !open;
                Debug.Log($"Door Collider state: {!open}");
            }

            if (asource != null)
            {
                asource.clip = open ? openDoor : closeDoor;
                asource.Play();
                Debug.Log("Door sound played.");
            }
            else
            {
                Debug.LogWarning("AudioSource missing on door!");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player detected near the door.");
                playerNearby = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player left the door area.");
                playerNearby = false;
            }
        }
    }
}