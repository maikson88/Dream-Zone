using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinianeAnimEvents : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private PlayerController playerController;
    public bool JumpUpFinished;

    AnimatorClipInfo[] animatorinfo;
    public string currentAnimation;

    public bool isSuperJump;
    public bool isShooting;
    private string previousAnim;
    private string currentAnim;

    private void Update()
    {
        
    }

    public void SuperJump(bool isSuperJump)
    {
        if (isSuperJump)
        {
            this.isSuperJump = isSuperJump;
            anim.SetBool("isSuperJump", true);
        }
        else if (!isSuperJump)
        {
            anim.SetBool("isNormalJump", true);
        }
    }

    public void JumpReset()
    {
        anim.SetBool("isNormalJump", false);
        anim.SetBool("isSuperJump", false);
        anim.SetBool("isJumping", false);
        JumpUpFinished = false;
        playerController.IsJumpingReset();
    }

    public void SkipAnimationTo(string targetAnim)
    {
        anim.GetComponent<Animator>().enabled = false;
        ResetAllParameters();
        anim.GetComponent<Animator>().enabled = true;
        anim.Play(targetAnim);
    }


    private void CurrentAnim()
    {
        animatorinfo = anim.GetCurrentAnimatorClipInfo(0);
        currentAnimation = animatorinfo[0].clip.name;
    }

    public string GetCurrentAnim()
    {
        return currentAnimation;
    }

    public void DebugAnimEvents()
    {
        currentAnim = GetCurrentAnim();
        if (currentAnim != previousAnim) Debug.Log(string.Concat("Animation: ", currentAnim));
        previousAnim = currentAnim;
    }

    public void ResetAllParameters()
    {
        anim.SetBool("isNormalJump", false);
        anim.SetBool("isSuperJump", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFalling", false);
        anim.SetBool("isGroundMoving", false);
    }

    //Animator event
    private void JumpUpTrigger() => JumpUpFinished = true;
}
