using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const string MOUSE_AXIS_X = "Mouse X";
    private const string MOUSE_AXIS_Y = "Mouse Y";

    [SerializeField] private float distance = 0f; // Distance from the player
    [SerializeField] private float sensitivity = 2f; // Mouse sensitivity
    [SerializeField] private Vector2 pitchMinMax = new Vector2(-30f, 60f); // Minimum and maximum pitch angle

    private Transform playerTransform;
    private float yaw; // Horizontal rotation
    private float pitch; // Vertical rotation

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag assigned.");
        }

        // Initialize camera rotation to match player's rotation
        yaw = playerTransform.eulerAngles.y;
        pitch = playerTransform.eulerAngles.x;
    }

    private void LateUpdate()
    {
        // Get mouse input
        float mouseX = Input.GetAxis(MOUSE_AXIS_X);
        float mouseY = Input.GetAxis(MOUSE_AXIS_Y);

        Debug.Log("Mouse X: " + mouseX);
        Debug.Log("Mouse Y: " + mouseY);

        yaw += mouseX * sensitivity;
        pitch -= mouseY * sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // Calculate camera position based on rotation
        Vector3 targetPosition = playerTransform.position - transform.forward * distance;
        transform.position = targetPosition;

        // Rotate camera around player
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}