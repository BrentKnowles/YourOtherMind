using System;
using System.Collections.Generic;
using System.Text;

namespace RulesMess
{
    public struct RelationKey
    {
        public string a;
        public string relationship;

  

        public RelationKey(string a, string relationship)
        {
            this.a = a;
            this.relationship = relationship;
        }

        public override string ToString()
        {
            return String.Format("Relation a {0} // Relationship {1}", a, relationship);
        }
    }
}
