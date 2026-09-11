
using XNode;



public class StoryPropConditionNode : Node {
    
	[Input] public Node PreNode;

	[ScriptableObjectNameProp(typeof(StoreGoodsData), "GoodsName")]
	public string GoodsName;
	public GoodsHoldsCondition Condition;
	public int Count = 1;
	
	[Output] public Node TrueNode;
	[Output] public Node FalseNode;

	public override object GetValue(NodePort port) {
		return null;
	}
}