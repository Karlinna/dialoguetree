using System;
using System.Collections.Generic;
using System.Text;

namespace TearStar.DialogueSystem
{
    abstract class Node
    {
        /// <summary>
        /// Start Node Value
        /// </summary>
        public const string StartValue = "*//*";
        /// <summary>
        /// End Node Value
        /// </summary>
        public const string EndValue = "/**/";
        /// <summary>
        /// Current Node's Value (e.g. it is a sentence in Dialogues)
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Unique identifier for this node
        /// </summary>
        public string NodeID { get; set; }
        /// <summary>
        /// Who is the owner of the Node (e.g. Dialogue who says the sentence)
        /// </summary>
        public INodeOwner NodeOwner { get; set; }
        /// <summary>
        /// ParentNodes list: Can be more than one
        /// </summary>
        private List<Node> ParentNodes;
        /// <summary>
        /// ChildNodes list: Possibly more than one
        /// </summary>
        private List<Node> ChildNodes;
        /// <summary>
        /// If this is true than this node will be executed
        /// </summary>
        public Func<bool> Condition;
        /// <summary>
        /// This is for safely save the method's name. Use SetSafeSave(string) for setting.
        /// </summary>
        public string SafeSave { get; private set; }
        /// <summary>
        /// How many parent Node has
        /// </summary>
        public int ParentCount
        {
            get
            {
                return ParentNodes == null ? 0 : ParentNodes.Count;
            }
        }
        /// <summary>
        /// How many children Node has
        /// </summary>
        public int ChildCount
        {
            get
            {
                return ChildNodes == null ? 0 : ChildNodes.Count;
            }
        }

        public void AddChild(Node v)
        {
            ChildNodes.Add(v);
        }
        public void AddParent(Node v)
        {
            ChildNodes.Add(v);
        }


        public List<Node> 

    }
}
