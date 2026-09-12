using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public abstract void ApplyEffect(Player player, float multiplier);
    public abstract void RemoveEffect(Player player);
}