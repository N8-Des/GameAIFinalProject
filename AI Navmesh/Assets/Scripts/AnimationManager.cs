using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Animator anim;
    public virtual void Initialize()
    {
        anim = GetComponent<Animator>();
    }
    public virtual void PlayTargetAnimation(string targetAnim, bool isInteracting)
    {
        anim.SetBool("IsInteracting", isInteracting);
        anim.CrossFade(targetAnim, 0.2f);
    }
    public void CanInputAgain()
    {
        anim.SetBool("IsInteracting", false);
    }
    public void EndDamage()
    {
        anim.SetTrigger("EndHit");
    }
}
