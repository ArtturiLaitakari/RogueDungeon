using UnityEngine;

/// <summary>
/// Animation controller for Zombie
/// </summary>
public class ZombieAnimator : MonoBehaviour, IAnimationController
{
    public Animator m_animator;    
    public void Move(float speed, bool isForward) => m_animator.SetFloat("MoveSpeed", speed);
    public void Attack() => m_animator.SetTrigger("Attack");
    public void Die() => m_animator.SetTrigger("Dead");
    public void Respawn() => m_animator.SetTrigger("Reset");
}
