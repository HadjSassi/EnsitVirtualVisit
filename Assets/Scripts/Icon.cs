using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icon : MonoBehaviour
{
    public string playerTag = "Player"; // Tag of the player object
    public float minDistance = 2f; // Minimum distance for the icon to be fully opaque
    public float maxDistance = 6f; // Maximum distance for the icon to start fading out
    public float fadeDistance = 4f; // Distance over which the icon fades out

    private Renderer iconRenderer;

    void Start()
    {
        // Get the renderer component of the icon
        iconRenderer = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // Get the main camera
        Camera mainCamera = Camera.main;

        // Check if the main camera exists
        if (mainCamera != null)
        {
            // Get the direction from the icon to the camera
            Vector3 direction = mainCamera.transform.position - transform.position;
            direction.y = 0f; // Ensure icon only rotates around the Y-axis

            // Rotate the icon to face the camera
            transform.rotation = Quaternion.LookRotation(direction);
        }
        
        // Find the player object using the tag
        GameObject player = GameObject.FindWithTag(playerTag);

        // Check if the player object is found
        if (player != null)
        {
            // Calculate the distance between the player and the icon
            float distance = Vector3.Distance(player.transform.position, transform.position);

            // Calculate the opacity based on the distance
            float opacity = Mathf.Clamp01(1f - (distance - minDistance) / fadeDistance);

            // Adjust opacity based on the fade range
            if (distance > maxDistance)
            {
                opacity = 0f; // Icon should be fully transparent if it's beyond the max distance
            }

            // Apply the opacity to the icon's material
            Color iconColor = iconRenderer.material.color;
            iconColor.a = opacity;
            iconRenderer.material.color = iconColor;
        }
    }
}
