using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Transform respawnPoint; // The respawn point (set in the Inspector)
    public Quaternion respawnRotation = Quaternion.identity; // Default respawn rotation

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Reset player position to the respawn point
            transform.position = respawnPoint.position;

            // Reset player rotation to the default rotation
            transform.rotation = respawnRotation;

            // Reset physics (optional)
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // Stop movement
                rb.angularVelocity = Vector3.zero; // Stop rotation
            }

            Debug.Log("Player respawned at the starting point!");
        }
    }
}
