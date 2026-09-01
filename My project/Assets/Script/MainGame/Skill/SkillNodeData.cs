using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SkillNodeData",menuName = "SkillNodeData")]
public class SkillNodeData : ScriptableObject
{
    public const int levelCount = 10;
    public string nodeID;
    public string nodeText;
    public List<SkillNodeData> prerequisiteNodes;
    public Effect effect;
}

