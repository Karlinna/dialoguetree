using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace TearStar.DialogueSystem
{
  

    public class ConditionalDialogueNode
    {
        public const string StartValue = "*//*";
        public const string EndValue = "/**/";
        public const int StartIndex = 0;
        public const int EndIndex = 1;
        public string Value { get; set; }

        internal string NodeID = "";

        INodeOwner NodeOwner;

        public ConditionalDialogueNode ParentNode { get; internal set; }

        public bool MultipleParents { get; set; }


        internal List<ConditionalDialogueNode> ParentNodes;

        internal Func<bool> Condition;
        internal string methodName;
        List<ConditionalDialogueNode> ChildrenNodes;
        public ConditionalDialogueNode(string V, Func<bool> function, string methodName, string nodeId)
        {
            Value = V;
            Condition = function;
            ChildrenNodes = new List<ConditionalDialogueNode>();
            NodeID = nodeId;
            MultipleParents = false;
            ParentNodes = new List<ConditionalDialogueNode>();
        }
        public ConditionalDialogueNode(string V, Func<bool> function)
        {
            Value = V;
            Condition = function;
            ChildrenNodes = new List<ConditionalDialogueNode>();
            MultipleParents = false;
            ParentNodes = new List<ConditionalDialogueNode>();
        }
        public bool ConditionMet => Condition();


        public void AddParent(ConditionalDialogueNode parent)
        {
            if (MultipleParents)
            {
                 if(ParentCount > 0)
                {
                    if(ParentNodes[0].NodeID != parent.NodeID)
                    {
                        ParentNodes.Add(parent);
                    }
                }
            }
            else
            {
                ParentNode = parent;
                ParentNodes.Add(ParentNode);
            }
        }
        public ConditionalDialogueNode[] GetChildren() => ChildrenNodes.ToArray();
        public ConditionalDialogueNode[] GetParents() => MultipleParents ? new ConditionalDialogueNode[] { ParentNode } : ParentNodes.ToArray();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ChildrenNodes.Count; i++)
            {
                sb.Append(ChildrenNodes[i].Value);
                sb.Append(",");
            }
            
            MethodInfo mi = Condition.GetMethodInfo();

            methodName = mi.Name;

            string ParentNodesStrings = "";
            if (MultipleParents)
            {
                StringBuilder sb2 = new StringBuilder();
                for (int i = 0; i < ParentNodes.Count; i++)
                {
                    sb2.Append(ParentNodes[i].Value);
                    sb2.Append(",");
                }
                ParentNodesStrings = sb2.ToString();
            }
            else
            {
                ParentNodesStrings = ParentNode == null ? "null" : ParentNode.Value;
            }
            

            return string.Format("NodeValue: {0} ; ParentNodeValue: [{1}] ; ChildrenNodeValues : [{2}] ; FunctionName: {3}, (SAFE){4} ; NodeID: {5}",
                Value, ParentNodesStrings, sb.ToString(), mi.Name, methodName, NodeID);
        }
        public void Add(ConditionalDialogueNode node)
        {
            ChildrenNodes.Add(node);
        }
        public bool HasChild =>
                ChildrenNodes.Count > 0;

        public int ParentCount => MultipleParents ? ParentNodes.Count : 1;

        public ConditionalDialogueNode this[int index]
        {
            get { return ChildrenNodes[index]; }
            set { ChildrenNodes[index] = value; }
        }

        public void SetNodeOwner(INodeOwner owner)
        {
            NodeOwner = owner;
        }


        public JObject ToJson()
        {
            MethodInfo mi = Condition.GetMethodInfo();
            if (MultipleParents)
            {
                JObject o = JObject.FromObject(new
                {
                    DialogueNode = new
                    {
                        value = Value,
                        parentNode = from p in ParentNodes select new { nodeID = p.NodeID},
                        method = mi.Name,
                        safeSave = mi.Name,
                        nodeID = NodeID,
                        owner = NodeOwner == null ? "null" : NodeOwner.Name,
                        childrenNodes =
            from p in ChildrenNodes
            select new
            {
                value = p.Value
            }
                    }
                });
                return o;
            }
            else
            {
                JObject o = JObject.FromObject(new
                {
                    DialogueNode = new
                    {
                        value = Value,
                        parentNode = ParentNode == null ? "null" : ParentNode.Value,
                        method = mi.Name,
                        safeSave = mi.Name,
                        nodeID = NodeID,
                        owner = NodeOwner == null ? "null" : NodeOwner.Name,
                        childrenNodes =
            from p in ChildrenNodes
            select new
            {
                nodeID = p.NodeID
            }
                    }
                });
                return o;
            }

        }

        public string ToDialogueLike()
        {
            return string.Format("{0} : '{1}'", NodeOwner == null ? "" : NodeOwner.Name, Value);
        }

    }
}
