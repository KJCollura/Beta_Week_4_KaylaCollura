using UnityEngine;

namespace DoorScript
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

        // Collider that blocks the player
        public Collider doorCollider;

        void Start()
        {
            // Force test score to 100
            KC_ScoreManager.instance?.AddScore(100);
            Debug.Log($"Start: Score forced to {KC_ScoreManager.instance?.GetScore()}");

            if (doorCollider != null) doorCollider.enabled = true;
            else Debug.LogWarning("Door Collider not assigned!");
        }

        void Update()
        {
            RotateDoor();

            // Detect E key and attempt to open door if conditions are met
            if (playerNearby && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("E key pressed - checking score...");

                int currentScore = KC_ScoreManager.instance?.GetScore() ?? -1;
                Debug.Log($"Current Score: {currentScore}");

                if (currentScore >= 50)
                {
                    OpenDoor();
                }
                else
                {
                    Debug.Log("Insufficient score to open door.");
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
                playerNearby = true;
                Debug.Log("Player entered door trigger.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                playerNearby = false;
                Debug.Log("Player left door trigger.");
            }
        }
    }
}