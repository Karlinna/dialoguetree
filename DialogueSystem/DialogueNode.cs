using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TearStar.DialogueSystem
{
    public class DialogueNode
    {
        public const string StartValue = "*//*";
        public const string EndValue = "/**/";
        public const int StartIndex = 0;
        public const int EndIndex = 1;
        public string Value { get; set; }

        public DialogueNode ParentNode { get; internal set; }

        private List<DialogueNode> ChildrenNodes;

        public DialogueNode(DialogueNode parent, string value)
        {
            ParentNode = parent;
            this.Value = value;
            ChildrenNodes = new List<DialogueNode>();
        }

        public DialogueNode(string value)
        {
            this.Value = value;
            ChildrenNodes = new List<DialogueNode>();
        }
        public DialogueNode this[int index]
        {
            get { return ChildrenNodes[index]; }
            set { ChildrenNodes[index] = value; }
        }
        public void Add(DialogueNode node)
        {
            ChildrenNodes.Add(node);
        }

        public bool HasChild =>
             ChildrenNodes.Count > 0;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ChildrenNodes.Count; i++)
            {
                sb.Append(ChildrenNodes[i].Value);
                sb.Append(",");
            }


            return string.Format("NodeValue: {0} ; ParentNodeValue: {1} ; ChildrenNodeValues : [{2}] ;",
                Value, ParentNode == null ? "null" : ParentNode.Value, sb.ToString());
        }

        public JObject ToJson()
        {
            JObject o = JObject.FromObject(new
            {
                DialogueNode = new
                {
                    value = Value,
                    parentNode = ParentNode == null ? "null" : ParentNode.Value,
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

        public bool RandomSheit() { return true; }
    }
}
