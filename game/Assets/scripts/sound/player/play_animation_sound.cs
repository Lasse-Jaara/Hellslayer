using UnityEngine;
using System.Collections.Generic;
public class play_animation_sound : MonoBehaviour
{
// ALL ANIMATIONS:
    // idle       - standing
    // run        - Running forward
    // walk       - Walking forward
    // run_b      - Running backwards
    // walk_b     - Walking backwards
    // jump       - Jumping
    // jump_b     - Jumping backwards
    // walk_r     - Walking right
    // walk_l     - Walking left
    // run_r      - Running right
    // run_l      - Running left
    // crouch_i   - Crouching idle
    // crouch_s   - Transition from crouch to stand 
    // crouch_w   - Crouch walking forward
    // crouch_b   - Crouch walking backwards
    // crouch_l   - Crouch walking left
    // crouch_r   - Crouch walking right



// ANIMATION type hashsets:
    private HashSet<string> all_walk = new HashSet<string>
    {
        "run", "walk", "walk_b", "walk_r", "walk_l", "crouch_w", "crouch_b", "crouch_l", "crouch_r"
    };

    private HashSet<string> all_run = new HashSet<string> 
    {
         "run", "run_b", "run_r", "run_l" 
    };

    private HashSet<string> all_jump = new HashSet<string> 
    {
         "jump", "jump_b"
    };
    private HashSet<string> all_crouch = new HashSet<string> 
    {
         "crouch_s", "crouch_i"
    };

        private string last_animation = "";



// Getting References:
    public what_animation_is_playing what_animation_is_playing_here;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }



// Checking what animation is playing
    void Update()
    {
        string current_animation = what_animation_is_playing_here.GetCurrentShortCode();

        if (last_animation != current_animation)
        {
            last_animation = current_animation;

            // Prioritize RUN over WALK if they overlap
            if (all_run.Contains(current_animation))
            {
                if (!audio_manager.Instance.sfxSource.isPlaying)
                {
                    audio_manager.Instance.PlaySFX("player_run", 1.00f);
                }
            }
            else if (all_walk.Contains(current_animation))
            {
                if (!audio_manager.Instance.sfxSource.isPlaying)
                {
                    audio_manager.Instance.PlaySFX("player_run", 0.75f);
                }
            }

            // Jumping sound
            else if (all_jump.Contains(current_animation))
            {
                audio_manager.Instance.PlaySFX("player_jump", 0.50f);
            }

            // Crouch sound
            else if (all_crouch.Contains(current_animation))
            {
                audio_manager.Instance.PlaySFX("player_crouch", 0.75f);
            }
            else
            {
                audio_manager.Instance.sfxSource.Stop();
            }
        }
    }
}