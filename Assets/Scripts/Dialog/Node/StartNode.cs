
using UnityEngine;
using XNode;

public class StartNode : Node {
    public AudioClip DialogBGM;
    [Output] public Node NextDialog;

    public override object GetValue(NodePort port) {
        return null;
    }
}

