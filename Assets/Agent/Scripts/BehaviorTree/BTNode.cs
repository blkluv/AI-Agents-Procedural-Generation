using System.Collections.Generic;

// A Node class for the behavior tree
namespace BehaviorTree
{
    public enum BTNodeState 
    {
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class BTNode
    {
        protected BTNodeState state;

        public BTNode parent;
        protected List<BTNode> children = new List<BTNode>();

        private Dictionary<string, object> dict = new Dictionary<string, object>();

        public BTNode() 
        {
            parent = null;
        }

        public BTNode(List<BTNode> children) 
        {
            foreach (BTNode child in children) 
            {
                Attach(child);
            }
        }

        private void Attach(BTNode node)
        {
            node.parent = this;
            children.Add(node);
        }

        public virtual BTNodeState Evaluate() => BTNodeState.FAILURE;

        public void SetData(string key, object value) 
        {
            dict[key] = value;
        }

        public object GetData(string key) 
        {
            object value = null;
            if (dict.TryGetValue(key, out value)) 
            {
                return value;
            }

            BTNode node = parent;
            while (node != null) 
            {
                value = node.GetData(key);
                if (value != null) 
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (dict.ContainsKey(key))
            {
                return dict.Remove(key);
            }

            BTNode node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }
                node = node.parent;
            }
            return false;
        }
    }
}
