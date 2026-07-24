
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum ExploreType {
	Goods
}

[Serializable]
public class ExploreMapping {
	public ExploreType Type = ExploreType.Goods;
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

