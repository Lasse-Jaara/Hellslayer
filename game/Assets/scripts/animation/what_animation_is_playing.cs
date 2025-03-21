using UnityEngine;
using System.Collections.Generic;

public class what_animation_is_playing : MonoBehaviour
{
    private Animator animator;

    private Dictionary<string, string> ani = new Dictionary<string, string>
    {
        { "run", "running_player" },
        { "walk", "walking_player" },
        { "run_b", "running_backwards" },
        { "walk_b", "walking_backward" },
        { "jump", "jumping_player" },
        { "jump_b", "jump_backwards" },
        { "walk_r", "right_strafe_walk" },
        { "walk_l", "left_strafe_walk" },
        { "run_r", "right_strafe_run" },
        { "run_l", "left_strafe_run" },
        { "crouch_i", "crouching_idle" },
        { "crouch_s", "Crouch_To_Stand" },
        { "crouch_w", "Crouch_Walk_Forward" },
        { "crouch_b", "Crouch_Walk_Back" },
        { "crouch_l", "Crouch_Walk_Left" },
        { "crouch_r", "Crouch_Walk_Right" }
    };


    // animator variables 
    private Dictionary<string, string> pa = new Dictionary<string, string>
    {
        { "run", "is_running" },
        { "walk", "is_walking" },
        { "back", "is_moving_backwards" },
        { "jump", "is_jumping" },
        { "crouch", "is_crouching" },
        { "right", "is_moving_right" },
        { "left", "is_moving_left" }
    };

    // Private variables
    private string currentShortCode;
    private string currentAnimationName;
    private AnimatorStateInfo current_Animation;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckCurrentAnimation();
    }

    void CheckCurrentAnimation()
    {
        current_Animation = animator.GetCurrentAnimatorStateInfo(0);

        foreach (KeyValuePair<string, string> entry in ani)
        {
            if (current_Animation.IsName(entry.Value))
            {
                currentShortCode = entry.Key;
                currentAnimationName = entry.Value;
                break;
            }
        }
    }

    // PUBLIC GETTERS so other scripts can read these!
    public string GetCurrentShortCode()
    {
        return currentShortCode; // returns like "run", "walk_b", etc.
    }

    public string GetCurrentAnimationName()
    {
        return currentAnimationName; // returns like "running_player"
    }
}
