using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [Tooltip("Assign the transform where the weapon should be held (e.g., HandPositionForWeapons)")]
    public Transform handPosition; // Drag HandPositionForWeapons here or set the tag

    private GameObject currentWeapon;

    private void Awake()
    {
        // Try to find handPosition by tag if not assigned
        if (handPosition == null)
        {
            GameObject handObject = GameObject.FindGameObjectWithTag("handPosition");
            if (handObject != null)
            {
                handPosition = handObject.transform;
            }
            else
            {
                Debug.LogError("Hand position object with tag 'handPosition' not found! Please assign it in the Inspector or tag it correctly.");
            }
        }
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (handPosition == null)
        {
            Debug.LogWarning("Cannot equip weapon: handPosition is not set.");
            return;
        }

        // Destroy the currently held weapon if any
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instantiate the new weapon and parent it to the hand
        currentWeapon = Instantiate(weaponPrefab);
        currentWeapon.transform.SetParent(handPosition);

        // Align it properly
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.identity;
    }
}
