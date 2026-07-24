
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public class ExploreMapping {
	public Vector2 Location;
	public StoreGoodsData GoodsData;
}

public class ExploreNode : Node {

	[Input]public Node PreNode;

	public Sprite ExploreCG;
	public List<ExploreMapping> Mappings;
	
	[Output] public Node NextDialog;
	
	public override object GetValue(NodePort port) {
		return null;
	}
}

