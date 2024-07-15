/// <summary>
/// Interface for animation controllers, opens AI and playercontrol to different
/// models
/// </summary>
public interface IAnimationController
{
    void Move(float speed, bool isForward);
    void Attack();
    void Die();
    void Respawn();
}
