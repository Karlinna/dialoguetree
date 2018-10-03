using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TearStar.DialogueSystem
{
    public class LinearDialogueTree
    {
        List<DialogueNode> Nodes;

        public void AddChild(DialogueNode parent, DialogueNode child)
        {
            if (Nodes.Count == 2)
            {
                if (child.ParentNode == null) child.ParentNode = Nodes[0];
                Nodes.Add(child);
                Nodes[0].Add(child);
            }
            else
            {
                if (child.ParentNode == null) child.ParentNode = parent;
                int index = SearchNode(parent.Value);
                if (index == -1) throw new Exception("No such parent node!");
                else
                {
                    Nodes.Add(child);
                    Nodes[index].Add(child);
                }

            }
        }

        public void AddNode(string Value)
        {
            DialogueNode node = new DialogueNode(null, Value);

            Nodes.Add(node);
        }

        public void AddNode(DialogueNode node)
        {
            Nodes.Add(node);
        }

        public bool DeleteNode(string Value)
        {
            int index = SearchNode(Value);
            if (index == -1) return false;
            if (Nodes[index].HasChild) throw new Exception("This dialogue has children!");

            Nodes.Remove(Nodes[index]);
            return true;
        }

        public bool DeleteNode(DialogueNode node)
        {
            int index = SearchNode(node.Value);
            if (index == -1) return false;
            if (Nodes[index].HasChild) throw new Exception("This dialogue has children!");
            Nodes.Remove(Nodes[index]);
            return true;
        }

        public void EndTree()
        {
            if (Count == 2)
            {
                //IT's a chidrön
                Nodes[0].Add(Nodes[1]);
                return;
            }
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (Nodes[i].Value != "/**/")
                {
                    bool countOfChildren = Nodes[i].HasChild;
                    if (!countOfChildren)
                    {
                        Nodes[i].Add(Nodes[1]);
                    }
                }
            }



        }

        public DialogueNode GetChild(DialogueNode node)
        {
            return node[0];
        }

        public int SearchNode(string Value)
        {
            int d = -1;
            d = Nodes.FindIndex(y => y.Value == Value);
            return d;
        }

        public DialogueNode GetNode(string Value)
        {
            int d = SearchNode(Value);
            if (d == -1) throw new Exception("No such node!");
            return Nodes[d];
        }
        public LinearDialogueTree()
        {
            Nodes = new List<DialogueNode>();
            DialogueNode start = new DialogueNode(null, "*//*");
            Nodes.Add(start);
            DialogueNode end = new DialogueNode(null, "/**/");
            Nodes.Add(end);
        }


        public DialogueNode this[int index]
        {
            get { return Nodes[index]; }
            set { Nodes[index] = value; }
        }

        public int Count => Nodes.Count;


        public JObject ToJson()
        {
            JObject o = JObject.FromObject(new
            {
                LinearDialogueTree = new
                {
                    nodes = from n in Nodes
                            select n.ToJson()
                }
            });

            return o;
        }
    }

}
