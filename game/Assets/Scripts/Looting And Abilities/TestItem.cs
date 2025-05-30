using UnityEngine;

public class TestItem : MonoBehaviour
{
    public string itemName = "TestItem"; // Name of the item
    public int itemValue = 10;           // Value of the item
    public Sprite itemIcon;              // Icon for UI display

    public int damageValue = 20;         // Damage the item can deal
    public float attackRange = 10f;      // Melee attack range
    public LayerMask enemyLayer;         // Enemy layer for raycast

    public Quaternion ItemRotation = Quaternion.Euler(0, 0, 0); // Local rotation when equipped
    public Vector3 ItemPosition = Vector3.zero;                // Local position when equipped
    private Camera playerCamera; // Reference to the player's camera

    private void Awake()
    {
        // Setup layer mask for enemies
        enemyLayer = LayerMask.GetMask("enemyLayer");

        // Find the camera with the correct tag
        playerCamera = GameObject.FindWithTag("playerCamera")?.GetComponent<Camera>();
    }

    private void Start()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Camera with tag 'playerCamera' not found. Please ensure your camera is tagged correctly.");
        }
    }

    public void RotateTo(Vector3 desiredRotation)
    {
        transform.rotation = Quaternion.Euler(desiredRotation);
    }

    // Called when the item is picked up
    public void PickupInput(Transform handTransform)
    {
        if (handTransform == null)
        {
            Debug.LogError("Hand transform is null. Cannot attach item.");
            return;
        }

        Debug.Log($"Picked up: {itemName}, value: {itemValue}");

        // Reset rotation to avoid mismatch
        transform.rotation = Quaternion.identity;

        // Parent to hand and set local transform
        transform.SetParent(handTransform);
        transform.localPosition = ItemPosition;
        transform.localRotation = ItemRotation;

        // Disable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
    }

    // Method to perform a melee attack
    public void PerformAttack()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player camera is not assigned. Cannot perform attack.");
            return;
        }

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, attackRange, enemyLayer))
        {
            Debug.Log("Hit something with attack ray.");

            if (hit.collider.TryGetComponent(out hp_system enemyHealth))
            {
                enemyHealth.take_damage(damageValue);
                Debug.Log($"{itemName} attacked {hit.collider.gameObject.name} for {damageValue} damage.");
            }
            else
            {
                Debug.LogError("Target does not have a valid 'hp_system' component.");
            }
        }
        else
        {
            Debug.Log("No enemy in range to attack.");
        }
    }
}
