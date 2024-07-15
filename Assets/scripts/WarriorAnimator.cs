using UnityEngine;

/// <summary>
/// Animation controller for warrior characters
/// </summary>
public class WarriorAnimator : MonoBehaviour, IAnimationController
{
    public Animator m_animator;
    public void Move(float speed, bool isForward) {
        m_animator.SetFloat("Velocity", speed);
        m_animator.SetFloat("Animation Speed", isForward ? 1 : -1);
    }
    public void Attack() {
        m_animator.SetFloat("Animation Speed", 1);
        m_animator.SetTrigger("Attack");
    }
    public void Die() { }
    public void Respawn() { }

    public void Jump()
    {
        m_animator.SetTrigger("Jump");
    }

}
