using System.Collections.Generic;

// Sequence class
namespace BehaviorTree
{
    public class Sequence : BTNode
    {
        public Sequence() : base() { }
        public Sequence(List<BTNode> children) : base(children) { }
        public override BTNodeState Evaluate()
        {
            bool isChildRunning = false;

            foreach (BTNode node in children)
            {
                switch (node.Evaluate()) 
                {
                    case BTNodeState.FAILURE:
                        state = BTNodeState.FAILURE;
                        return state;
                    case BTNodeState.SUCCESS:
                        continue;
                    case BTNodeState.RUNNING:
                        isChildRunning = true;
                        continue;
                    default:
                        state = BTNodeState.SUCCESS;
                        return state;
                }
            }
            state = isChildRunning ? BTNodeState.RUNNING : BTNodeState.SUCCESS;
            return state;
        }
    }
}