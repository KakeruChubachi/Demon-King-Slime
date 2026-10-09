using UnityEngine;

[CreateAssetMenu(fileName = "CircleMagic", menuName = "Effects/Circle Magic")]
public class CircleMagicEffect : Effect
{
    public GameObject magicPrefab;   // 見た目用（任意）。直径1ユニット基準で作る
    public float baseDamage = 10f;
    public float interval = 2f;
    public float radius = 3f;

    public override void ApplyEffect(Player player, float multiplier)
    {
        var magic = player.GetComponent<CircleMagicAttacker>();
        if (magic == null) magic = player.gameObject.AddComponent<CircleMagicAttacker>();
        magic.Setup(magicPrefab, baseDamage * multiplier, interval, radius);
    }

    public override void RemoveEffect(Player player)
    {
        var magic = player.GetComponent<CircleMagicAttacker>();
        if (magic != null) Destroy(magic);
    }
}