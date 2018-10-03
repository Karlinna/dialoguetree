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
    }
}
