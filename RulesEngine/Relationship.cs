using System;
using System.Collections.Generic;
using System.Text;

namespace RulesMess
{
    public class Relationship
    {
        string relation;
        string a;
        string b;
        bool deduction; // set to true if this rule added based on deduction
        double weight; // ToDo: Not used but there for the future
        int priority; // ToDo: arbitary but the AI might use this in the future for relative ranking. Simliar to weight but (possibly) not the same
        string comment; // for us with rule editor for understanding why rules are there


        /// <summary>
        /// constructor for serializing
        /// </summary>
        public Relationship()
        {
        }

        public string A
        {
            get { return a; }
            set { a = value; }
        }

        public string Relation
        {
            get { return relation; }
            set { relation = value; }
        }
        public bool Deduction
        {
            get { return deduction; }
            set { deduction = value; }
        }
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
        public double Weight
        {
            get { return weight; }
            set { weight = value; }
        }
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
        public string B
        {
            get { return b; }
            set { b = value; }
        }

        public RelationKey GetRelationKey()
        {
            return new RelationKey(a, relation);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="relation"></param>
        /// <param name="b"></param>
        /// <param name="comment"></param>
        /// <param name="weight">effects likelhiood</param>
        /// <param name="priority">arbitary but the AI might use this in the future for relative ranking. Simliar to weight but (possibly) not the same</param>
        /// <param name="deduction">set to true if this rule added based on deduction -- done automatically when Deduce is called</param>
        public Relationship(string a, string relation, string b, string comment, double weight, int priority, bool deduction)
        {
            this.a = a;
            this.relation = relation;
            this.b = b;
            this.comment = comment;
            this.weight = weight;
            this.priority = priority;
            this.deduction = deduction;
        }

        public override string ToString()
        {
            return String.Format("{0} || {1} || {2} ||  deduction?:{3} || {4} || weight:{5} || priority{6} "
            , a, relation, b, deduction, comment, weight.ToString(), priority.ToString());
           
        }
    }
}
