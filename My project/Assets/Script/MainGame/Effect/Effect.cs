using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public string effectName;   // —á: "UŒ‚—ÍƒAƒbƒv"
    [TextArea] public string description; // —á: "‹ßÚUŒ‚—Í‚ª1.5”{‚É‚È‚é"

    public abstract void ApplyEffect(Player player, float multiplier);
    public abstract void RemoveEffect(Player player);
}