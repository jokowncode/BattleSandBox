
using UnityEngine;
using XNode;

public class StartNode : Node {

    public bool CanSkip = true;
    [Output] public Node NextDialog;

    public override object GetValue(NodePort port) {
        return null;
    }
}

