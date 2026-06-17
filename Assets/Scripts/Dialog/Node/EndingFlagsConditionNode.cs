
using UnityEngine;
using XNode;

public class EndingFlagsConditionNode : Node {

    public EndingFlags ReferenceFlags;
    public int CompareValue;
    public Comparator Comparator;

    [Output] public Node TrueNode;
    [Output] public Node FalseNode;

    public override object GetValue(NodePort port) {
        return null;
    }
}