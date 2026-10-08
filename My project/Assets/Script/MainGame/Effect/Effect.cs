using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public string effectName;
    public Sprite icon;                   // Å© í«â¡
    [TextArea] public string description;

    public abstract void ApplyEffect(Player player, float multiplier);
    public abstract void RemoveEffect(Player player);
}