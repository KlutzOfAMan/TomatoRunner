using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public PlayerMovement1 pm;

    [Header("Animations")]
    public Animator animator;
    public string currentAnimation;

   

    const string PLAYER_IDLE = "Idle";
    const string PLAYER_RUN = "Running";
    const string PLAYER_JUMP = "Jump";
    const string PLAYER_ROLL = "Roll";
    const string PLAYER_DIE = "Died";
    const string PLAYER_SHOOT = "Shoot";
    const string PLAYER_DASH = "Dash";


    public void Start()
    {
        pm = FindObjectOfType<PlayerMovement1>();
        animator = gameObject.GetComponent<Animator>();
    }

    public void Update()
    {
        if (pm.isIdle)
        {
            ChangeAnimationState(PLAYER_IDLE);
        }

        if (pm.isMoving) //If moving and not jumping or rolling
        {
            ChangeAnimationState(PLAYER_RUN);

        }
        

        if (pm.isRolling)
        {
            ChangeAnimationState(PLAYER_ROLL);
        }

        if (pm.isDashing)
        {
            ChangeAnimationState(PLAYER_DASH);
        }

        if (pm.isJumping) //If Jumping
        {
            ChangeAnimationState(PLAYER_JUMP);
            
        }
    }

    //ANIMATION HANDLER
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }
}
