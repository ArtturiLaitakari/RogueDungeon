using UnityEngine;

/// <summary>
/// Magic system for Hammer Hero
/// </summary>
public class HammerSpells : MonoBehaviour, ISpells
{
    public GameObject forcefield;
    private GameObject fieldInstance;
    public Transform muzzle;
    public float shieldDuration=9;
    private float t;
    public string attackSpellName="Hammer";
    public string defenseSpellName="Forcefield";

    public AudioSource defenseAudio;
    public AudioSource potionAudio;
    public void AttackSpell() {}

    /// <summary>
    /// create a forcefield
    /// </summary>
    public bool DefenseSpell()
    {
        var position = transform.position;
        position.y = 1;
        fieldInstance = Instantiate(forcefield, position, Quaternion.Euler(90, 0, 0));
        fieldInstance.transform.parent = transform;
        t = shieldDuration;
        defenseAudio.Play();
        return true;
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

    /// <summary>
    /// Destroy forcefield if expired
    /// </summary>
    private void FixedUpdate()
    {
        if (fieldInstance != null && (t -= Time.deltaTime) < 0)
        {
            Destroy(fieldInstance);
        }
    }
    public string GetAttackSpellName() => attackSpellName;
    public string GetDefenseSpellName() => defenseSpellName;
}
