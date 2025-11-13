
using UnityEngine;
using XNode;

public class StartNode : Node {
    
    [Output] public Node NextDialog;

    public override object GetValue(NodePort port) {
        return null;
    }
}

