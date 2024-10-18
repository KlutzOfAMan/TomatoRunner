using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageScript : MonoBehaviour
{

    public Animator animator;
    public string currentAnimation;
    private Collections coll;

    const string CAGE_IDLE = "Cage_Idle";
    const string CAGE_BREAK = "Cage_Break";

    // Start is called before the first frame update
    void Start()
    {
        coll = FindObjectOfType<Collections>();
        animator = gameObject.GetComponent<Animator>();
        ChangeAnimationState(CAGE_IDLE);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            ChangeAnimationState(CAGE_BREAK);
            FindObjectOfType<AudioManager>().Play("CageBreak");
        }
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
        coll.TomatoSavedInLevel++;
    }


    //ANIMATION HANDLER
    void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;
    }

}
