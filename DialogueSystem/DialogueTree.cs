using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace TearStar.DialogueSystem
{
     public class DialogueTree
    {

        List<ConditionalDialogueNode> Nodes;

        public ConditionalDialogueNode GetStartNode()
        {
            return Nodes[0];
        }
        public ConditionalDialogueNode GetEndNode()
        {
            return Nodes[1];
        }        
        public bool Constant()
        {
            return true;
        }
        public DialogueTree()
        {
            Nodes = new List<ConditionalDialogueNode>();
            Nodes.Add(new ConditionalDialogueNode(DialogueNode.StartValue, Constant, "Constant", "start"));
            Nodes.Add(new ConditionalDialogueNode(DialogueNode.EndValue, Constant, "Constant", "end"));

            ReachedEnd = false;
        }
        public int Count => Nodes.Count;
        public void AddChild(ConditionalDialogueNode parent, ConditionalDialogueNode child)
        {
            if (Nodes.Count == 2)
            {
                if (child.ParentNode == null) child.ParentNode = Nodes[0];
                child.NodeID = AutoID();
                Nodes.Add(child);
                Nodes[0].Add(child);
            }
            else
            {
                int index = SearchNodeIndexByValue(parent.Value);
                if(index == -1) throw new Exception("No such parent node!");
                if(child.ParentNode != null)
                {
                    //Multiple Parents
                    child.MultipleParents = true;
                    child.AddParent(parent);
                    if (child.NodeID.Trim() == "") child.NodeID = AutoID();
                    Nodes[index].Add(child);
                }
                else
                {
                    if (child.NodeID.Trim() == "") child.NodeID = AutoID();
                    child.AddParent(parent);
                    Nodes.Add(child);
                    Nodes[index].Add(child);
                    //SingleParent
                }

            }
        }
        public void AddChild(ConditionalDialogueNode parent, ConditionalDialogueNode child, INodeOwner owner)
        {
            if (Nodes.Count == 2)
            {
                if (child.ParentNode == null) child.ParentNode = Nodes[0];
                child.NodeID = AutoID();
                child.SetNodeOwner(owner);
                Nodes.Add(child);
                Nodes[0].Add(child);
            }
            else
            {
                int index = SearchNodeIndexByValue(parent.Value);
                if (index == -1) throw new Exception("No such parent node!");
                if (child.ParentNode != null)
                {
                    //Multiple Parents
                    child.MultipleParents = true;
                    child.SetNodeOwner(owner);

                    child.AddParent(parent);
                    if (child.NodeID.Trim() == "") child.NodeID = AutoID();
                    Nodes[index].Add(child);
                }
                else
                {
                    if (child.NodeID.Trim() == "") child.NodeID = AutoID();
                    child.SetNodeOwner(owner);

                    child.AddParent(parent);
                    Nodes.Add(child);
                    Nodes[index].Add(child);
                    //SingleParent
                }

            }
        }

        public void AddChild(ConditionalDialogueNode parent, ConditionalDialogueNode child, string child_id)
        {
            if (Nodes.Count == 2)
            {
                if (child.ParentNode == null) child.ParentNode = Nodes[0];
                if (child.NodeID.Trim() == "") child.NodeID = child_id;
                Nodes.Add(child);
                Nodes[0].Add(child);
            }
            else
            {
                int index = SearchNodeIndexByValue(parent.Value);
                if (index == -1) throw new Exception("No such parent node!");
                if (child.ParentNode != null)
                {
                    //Multiple Parents
                    child.MultipleParents = true;
                    child.AddParent(parent);
                    if (child.NodeID.Trim() == "") child.NodeID = child_id;
                    Nodes[index].Add(child);
                }
                else
                {
                    if (child.NodeID.Trim() == "") child.NodeID = child_id;
                    child.AddParent(parent);
                    Nodes.Add(child);
                    Nodes[index].Add(child);
                    //SingleParent
                }


            }
        }
        public void AddChild(ConditionalDialogueNode parent, ConditionalDialogueNode child, string child_id, INodeOwner owner)
        {
            if (Nodes.Count == 2)
            {
                if (child.ParentNode == null) child.ParentNode = Nodes[0];
                if (child.NodeID.Trim() == "") child.NodeID = child_id;
                child.SetNodeOwner(owner);

                Nodes.Add(child);
                Nodes[0].Add(child);
            }
            else
            {
                int index = SearchNodeIndexByValue(parent.Value);
                if (index == -1) throw new Exception("No such parent node!");
                if (child.ParentNode != null)
                {
                    //Multiple Parents
                    child.MultipleParents = true;
                    child.SetNodeOwner(owner);
                    child.AddParent(parent);
                    if (child.NodeID.Trim() == "") child.NodeID = child_id;
                    Nodes[index].Add(child);
                }
                else
                {
                    if (child.NodeID.Trim() == "") child.NodeID = child_id;
                    child.SetNodeOwner(owner);
                    child.AddParent(parent);
                    Nodes.Add(child);
                    Nodes[index].Add(child);
                    //SingleParent
                }


            }
        }

        public bool DeleteNode(string Value)
        {
            int index = SearchNodeIndexByValue(Value);
            if (index == -1) return false;
            if (Nodes[index].HasChild) throw new Exception("This dialogue has children!");

            Nodes.Remove(Nodes[index]);
            return true;
        }

        public bool DeleteNode(ConditionalDialogueNode node)
        {
            int index = SearchNodeIndexByValue(node.Value);
            if (index == -1) return false;
            if (Nodes[index].HasChild) throw new Exception("This dialogue has children!");
            Nodes.Remove(Nodes[index]);
            return true;
        }

        public void EndTree()
        {
             if(Count == 2)
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

        public ConditionalDialogueNode[] GetChildren(ConditionalDialogueNode node)
        {
            return node.GetChildren();
        }

        public int SearchNodeIndexByValue(string Value)
        {
            int d = -1;
            d = Nodes.FindIndex(y => y.Value == Value);
            return d;
        }

        public int SearchNodeIndex(string id)
        {
            int i = -1;
            i = Nodes.FindIndex(y => y.NodeID == id);
            return i;
        }

        public ConditionalDialogueNode SearchNode(string id)
        {
            int i = SearchNodeIndex(id);
            if (i >= 0)
                return Nodes[i];
            else
                throw new Exception("No Such Node");
        }

        public ConditionalDialogueNode this[int index]
        {
            get { return Nodes[index]; }
            set { Nodes[index] = value; }
        }

        public JObject ToJson()
        {
            JObject o = JObject.FromObject(new
            {
                DialogueTree = new
                {
                    nodes = from n in Nodes
                            select n.ToJson()
                }
            });

            return o;
        }

        public static DialogueTree ParseJson<T>(string json, T library) where T : class
        {
            DialogueTree dt = new DialogueTree();


            return dt;
        }

        int id_num = -1;
        public string AutoID()
        {
            id_num++;
            return string.Format("auto_{0}", id_num);
        }


        public bool NotStartOrEnd(ConditionalDialogueNode node) => node.Value != ConditionalDialogueNode.EndValue && node.Value != ConditionalDialogueNode.StartValue;

        private int indexing = 0;
        public int Indexing { get
            {
                return indexing;
            } }
        public ConditionalDialogueNode GetNextNode()
        {
            
            if (indexing >= Count || indexing == 1) { ReachedEnd = true; return null; }
            ConditionalDialogueNode currentNode = Nodes[indexing];
            ConditionalDialogueNode[] childrens = currentNode.GetChildren();

            for (int i = 0; i < childrens.Length && !ReachedEnd; i++)
            {
                if (childrens[i].ConditionMet)
                {
                    int nextIndex = SearchNodeIndex(childrens[i].NodeID);

                    indexing = nextIndex;
                }
            }
           
            return currentNode;
        }

        public bool ReachedEnd { get; private set; }
    }
}
