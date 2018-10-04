using System;
using System.Collections.Generic;
using System.Text;

namespace TearStar.DialogueSystem
{
    public class NodeOwner : INodeOwner
    {
        
        public string Name { get; set; }

        public NodeOwner(string n)
        {
            Name = n;
        }


        public static NodeOwner ParseNodeOwner(string Name)
        {
            if (Name == "null") return null;
            else return new NodeOwner(Name);
        }
        public static NodeOwner ParseNodeOwner(string Name, List<INodeOwner> i)
        {
            throw new NotImplementedException();
        }
    }
}
