using System.Collections.Generic;

// Selector class
namespace BehaviorTree 
{
    public class Selector : BTNode
    {
        public Selector() : base() { }
        public Selector(List<BTNode> children) : base(children) { }
        public override BTNodeState Evaluate()
        {
            foreach (BTNode node in children)
            {
                switch (node.Evaluate())
                {
                    case BTNodeState.FAILURE:
                        continue;
                    case BTNodeState.SUCCESS:
                        state = BTNodeState.SUCCESS;
                        return state;
                    case BTNodeState.RUNNING:
                        state = BTNodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }
            state = BTNodeState.FAILURE;
            return state;
        }
    }
}
