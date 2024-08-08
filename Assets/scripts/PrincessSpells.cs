using UnityEngine;

/// <summary>
/// custom magic handling for each character
/// </summary>
public class PrincessSpells : MonoBehaviour, ISpells
{
    public GameObject fireball;
    public Transform muzzle;
    private string attackSpellName="Fireball";
    private string defenseSpellName="Health";
    private string potionName = "Mystery";

    private Health health;
    public AudioSource attackAudio;
    public AudioSource defenseAudio;
    public AudioSource potionAudio;
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
        attackAudio.Play();
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
            defenseAudio.Play();
            return true;
        } 
        return false;
    }
    /// <summary>
    /// To be invented
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Potion()
    {
        potionAudio.Play();
        throw new System.NotImplementedException();
    }

    public string GetAttackSpellName() => attackSpellName;
    public string GetDefenseSpellName() => defenseSpellName;
    public string GetPotionName() => potionName;
}
