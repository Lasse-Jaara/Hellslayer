using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 8.5f;
    private Camera playerCamera;
    private bool OnHand = false;

    [SerializeField] private PlayerInputManager getInput;
    [SerializeField] private Transform handPosition;
    [SerializeField] private ItemSlotHandler itemSlotHandler;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public TestItem currentlyHeldItem;

    private void Start()
    {
        playerCamera = GameObject.FindWithTag("playerCamera")?.GetComponent<Camera>();

        if (playerCamera == null)
            Debug.LogError("Camera with tag 'playerCamera' not found.");
        if (getInput == null)
            Debug.LogError("PlayerInputManager not assigned.");
        if (handPosition == null)
            Debug.LogError("Hand position not assigned.");
    }

    private void Update()
    {
        if (playerCamera == null) return;

        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange))
        {
            if (hit.collider.TryGetComponent<TestItem>(out TestItem item))
            {
                Debug.Log($"Looking at {item.itemName}, press pickup key");

                if (getInput.PickupInput.WasPressedThisFrame())
                {
                    itemSlotHandler.PickUpItem(item);
                    item.PickupInput(handPosition);      // 🔧 Use correct overload
                    MoveItemToHand(item);               // 🔧 Handle parenting and colliders

                    Transform child = transform.Find("rpg");
                    EnableWeaponScript(item.gameObject); // 🔧 Enable weapon behavior
                }
            }
        }

        if (getInput.DropInput.WasPressedThisFrame() && currentlyHeldItem != null)
        {
            DropItem();
            itemSlotHandler.DropItem();
        }

        if (getInput.ItemSlot1Input.WasPressedThisFrame())
        {
            itemSlotHandler.SwitchSlot(1);
        }
        else if (getInput.ItemSlot2Input.WasPressedThisFrame())
        {
            itemSlotHandler.SwitchSlot(2);
        }
    }
    private void HideArmsMesh(bool value)
    {
        if (value)
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = false; // or true to enable
            }
        }
        else
        {
            if (skinnedMeshRenderer != null)
            {
                skinnedMeshRenderer.enabled = true; // or false to disable
            }
        }
    }
    private void MoveItemToHand(TestItem item)
    {
        HideArmsMesh(true); // Hide arms mesh when picking up an item
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        item.transform.SetParent(handPosition);
        item.transform.localPosition = item.ItemPosition;
        item.transform.localRotation = item.ItemRotation;

        foreach (Collider collider in item.GetComponents<Collider>())
            collider.enabled = false;

        currentlyHeldItem = item;
        Debug.Log($"{item.itemName} has been picked up and moved to the hand.");
    }

    public void DropItem()
    {
        HideArmsMesh(false);
        if (currentlyHeldItem == null) return;

        Rigidbody rb = currentlyHeldItem.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;
            rb.detectCollisions = true;

        currentlyHeldItem.transform.SetParent(null);

        foreach (Collider collider in currentlyHeldItem.GetComponents<Collider>())
            collider.enabled = true;

        if (rb != null)
            rb.AddForce(playerCamera.transform.forward * 2f, ForceMode.Impulse);

        Debug.Log($"{currentlyHeldItem.itemName} has been dropped.");

        DisableWeaponScript(currentlyHeldItem.gameObject);

        currentlyHeldItem = null;
    }

    public void EnableWeaponScript(GameObject itemObject)
    {
        RPG[] allRPGs = Object.FindObjectsByType<RPG>(FindObjectsSortMode.None);
        foreach (RPG rpg in allRPGs) rpg.enabled = false;

        LaserDesertEagle[] allLasers = Object.FindObjectsByType<LaserDesertEagle>(FindObjectsSortMode.None);
        foreach (LaserDesertEagle laser in allLasers)
        {
            laser.enabled = false;
            laser.SetEquipped(false);
        }

        RPG thisRPG = itemObject.GetComponent<RPG>();
        if (thisRPG != null)
        {
            thisRPG.enabled = true;
            Debug.Log("Enabled RPG script on equipped item.");
        }

        LaserDesertEagle thisLaser = itemObject.GetComponent<LaserDesertEagle>();
        if (thisLaser != null)
        {
            thisLaser.enabled = true;
            thisLaser.SetEquipped(true);
            Debug.Log("Enabled LaserDesertEagle script on equipped item.");
        }
    }

    public void DisableWeaponScript(GameObject itemObject)
    {
        RPG rpgScript = itemObject.GetComponent<RPG>();
        if (rpgScript != null)
        {
            rpgScript.enabled = false;
            Debug.Log("Disabled RPG script on dropped item.");
        }

        LaserDesertEagle laser = itemObject.GetComponent<LaserDesertEagle>();
        if (laser != null)
        {
            laser.enabled = false;
            laser.SetEquipped(false);
            Debug.Log("Disabled LaserDesertEagle script on dropped item.");
        }
    }
}
