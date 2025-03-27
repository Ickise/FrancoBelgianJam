using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Idle = Animator.StringToHash("Idle");

    // Je ne sais pas si ça sera utile, mais je le laisse ici pour le moment
    public void PlayWalkAnimation(GameObject targetObject)
    {
        Animator animator = targetObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool(Walk, true);
        }
    }

    public void PlayRunAnimation(GameObject targetObject)
    {
        Animator animator = targetObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool(Run, true);
        }
    }

    public void StopWalkAnimation(GameObject targetObject)
    {
        Animator animator = targetObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool(Walk, false);
        }
    }

    public void PlayIdleAnimation(GameObject targetObject)
    {
        Animator animator = targetObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger(Idle);
        }
    }

    public void PlayCustomAnimation(GameObject targetObject, string animationName)
    {
        Animator animator = targetObject.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play(animationName);
        }
    }
}