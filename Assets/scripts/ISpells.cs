/// <summary>
/// Interface for spell casters
/// </summary>
public interface ISpells
{
    void AttackSpell();
    bool DefenseSpell();
    void MasterySpell();
    string GetAttackSpellName();
    string GetDefenseSpellName();
}
