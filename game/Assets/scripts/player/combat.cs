using UnityEngine;

public class combat : MonoBehaviour
{
    // references
    public Animator animator;
    public PlayerInput inputManager;

    public GameObject katana;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (katana != null)
        {
            katana.SetActive(false); // hide all elements of katana gameobject but not destroy and keep hierarchy
        }       
        //animator.SetBool("has_katana", false); // its false by default in animator
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);


        if (animator.GetBool("has_katana") == true)
        {
            katana.SetActive(true);
        }

        // Check if left mouse button was pressed this frame
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left mouse button was clicked.");
            animator.SetBool("has_katana_attacked", true);
        }
        if (stateInfo.IsName("YourAnimationStateName") && stateInfo.normalizedTime >= 1.0f)
        {
            animator.SetBool("has_katana_attacked", false);
        }
    }
}   
