/// <summary>
/// Interface for spell casters
/// </summary>
public interface ISpells
{
    void AttackSpell();
    bool DefenseSpell();
    void Potion();
    string GetAttackSpellName();
    string GetDefenseSpellName();
}
