using UnityEngine;

/// <summary>
/// custom magic handling for each character
/// </summary>
public class PrincessSpells : MonoBehaviour, ISpells
{
    public GameObject fireball;
    public Transform muzzle;
    public string attackSpellName;
    public string defenseSpellName;
    private Health health;
    void Start()
    {
        health = GetComponent<Health>(); 
    }

    /// <summary>
    /// Instantiates and fires a projectile from the muzzle's position and rotation.
    /// The projectile's shooterTag is set to the current object's tag, and 
    /// </summary>
    public void AttackSpell()
    {
        GameObject projectile = GameObject.Instantiate(fireball, muzzle.position, muzzle.rotation);
        projectile.GetComponent<Fireball>().shooterTag = "Player";
    }

    /// <summary>
    /// Heals the players by increasing their health by 1 unit. 
    /// Adjusts the players's current speed based on the new health status, 
    /// factoring in any fatigue.
    /// </summary>
    public bool DefenseSpell()
    {
        if (health.GetWounds() > 0)
        {
            health.AddHealth();
            GameController.instance.SetHealth(health.GetHealth(), health.MaxHealth());
            return true;
        } 
        return false;
    }
    public string GetAttackSpellName() => attackSpellName;
    public string GetDefenseSpellName() => defenseSpellName;

    /// <summary>
    /// To be invented
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void MasterySpell()
    {
        throw new System.NotImplementedException();
    }
}
