using UnityEngine;

public class animation_sound_player : MonoBehaviour
{
    [SerializeField] private PlayerInputSubscription GetInput;
    private Animator animator;
    public what_animation_is_playing what_animation_is_playing_here;
    private bool isGrounded = true;
    private CharacterController characterController; // reference to CharacterController
    

    void Start()
    {
        GetInput = GetComponent<PlayerInputSubscription>();
        animator = GetComponent<Animator>();
        what_animation_is_playing_here = GetComponent<what_animation_is_playing>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = characterController.isGrounded;
        animator.SetBool("isGrounded", isGrounded);
        Debug.Log(isGrounded);
        if (GetInput.JumpInput)
        {
            animator.SetBool("is_jumping", true);
            // audio_manager.Instance.PlaySFX("player_jump", 0.50f); 
        }
        else
        {
            //Debug.Log("is not jumping");
            animator.SetBool("is_jumping", false);
        }

        if (GetInput.CrouchInput)
        {
            animator.SetBool("is_crouching", true);
            // audio_manager.Instance.PlaySFX("player_crouch", 0.50f);
        }
        else
        {
            animator.SetBool("is_crouching", false);
        }

        if (GetInput.SprintInput)
        {
            if (isGrounded)
            {
                animator.SetBool("is_running", true);
            }
        }
        else
        {
            animator.SetBool("is_running", false);
        }

        if (GetInput.MoveInput != Vector2.zero)
        {
            animator.SetBool("is_walking", true);
            // audio_manager.Instance.PlaySFX("player_walk", 0.50f);
        }
        else
        {
            animator.SetBool("is_walking", false);
        }
    }
}
