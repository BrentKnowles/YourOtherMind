using System;
using System.Collections.Generic;
using System.Text;
//using interfaceTester;
using System.Collections;
using CoreUtilities;

namespace RulesMess
{
    public class KnowledgeBase
    {

        public static string DEDUCED_IS = "is"; // when deducing it is okay to write A 'is' B
        private SerializableDictionary<RelationKey, List<Relationship>> knowledge
            = new SerializableDictionary<RelationKey, List<Relationship>>();

        // Returns an array of relationships.
        public Relationship[] this[RelationKey key]
        {
            get
            {
                try
                {
                   
                        return knowledge[key].ToArray();
                   
                }
                catch (Exception)
                {
                    // key not found 
                    return null;
                }
            }
        }


        /// <summary>
        /// saves to an xml file
        /// </summary>
        /// <param name="sFile"></param>
         public void Save(string sFile)
        {
            FileUtils.Serialize(knowledge, sFile, Constants.BLANK);
        }

        /// <summary>
        /// iterates through knowledge base and returns a string array of
        ///  "A" is sDefinedFor
        /// 
        /// Collecting all the "A"'s
        /// 
        /// Original use: Generating a Pronoun list
        /// (Brent created)
        /// </summary>
        /// <param name="sDefinedFor"></param>
        /// <returns></returns>
        public string[] GetDefinedFor(string sDefinedFor)
        {
            ArrayList defined = new ArrayList();
            foreach (KeyValuePair<RelationKey, List<Relationship>> kvp in knowledge)
            {
                foreach (Relationship relationship in kvp.Value)
                {

                    List<Relationship> theBs = knowledge[relationship.GetRelationKey()];


                    for (int i = 0; i < theBs.Count; i++)
                    {
                        if (theBs[i].B == sDefinedFor && theBs[i].Relation == DEDUCED_IS)
                        {
                            if (defined.IndexOf(theBs[i].A) <= -1)
                            {
                                // uniqueness
                            defined.Add((string)theBs[i].A);
                            }
                        }
                    }
                    theBs = null;

                }
            }
            return (string[])defined.ToArray(typeof(string));
        }


        /// <summary>
        /// loads the internal dictionary
        /// </summary>
        /// <param name="sFile"></param>
        /// <returns></returns>
        public void Load(string sFile)
        {
            knowledge = (SerializableDictionary<RelationKey, List<Relationship>>)
                FileUtils.DeSerialize(sFile, typeof(SerializableDictionary<RelationKey, List<Relationship>>));


        }

        /// <summary>
        /// this will load the database assuming a table with each 'rule' on a row
        /// This is generally used by the fRulesInspector in edit mode
        /// </summary>
        /// <param name="sFile"></param>
        public void LoadFromXmlTable(string sFile)
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.ReadXml(sFile);
            
            // now load the rules
            foreach (System.Data.DataRow row in ds.Tables[0].Rows)
            {
                Add(new Relationship(row["A"].ToString(),
                    row["Relation"].ToString(),
                    row["B"].ToString(),
                    row["Comment"].ToString(),
                    (double)row["Weight"],
                     (int)row["Priority"],
                    false
                    
                ));
            }


            // try to make super generic with reflection
        }

        /// <summary>
        /// Adds a relationship to the list
        /// </summary>
        /// <param name="r"></param>
        public void Add(Relationship r)
        {
            RelationKey key = r.GetRelationKey();

            if (knowledge.ContainsKey(key))
            {
                bool bNew = true;
                //sadly will need to do a for-loop here to prevent duplicates
                foreach (Relationship oldrelationship in knowledge[key])
                {
                    if (oldrelationship.Relation == r.Relation
                        && oldrelationship.A == r.A
                        && oldrelationship.B == r.B
                        )
                    {
                        bNew = false;
                    }
                }

                // If the rules already added don't do it again.
                //if (knowledge[key].Contains(r)) return; DID NOT WORK (Probably because of the weights being different?)
                if (bNew == true)
                {
                    knowledge[key].Add(r);
                }
                return;
            }
            else
            {
                List<Relationship> relationList = new List<Relationship>();
                relationList.Add(r);
                knowledge.Add(key, relationList);
            }
        }

        // Create new rules from existing rules.
        public void Deduction()
        {
            List<Relationship> potentialNewKnowledge = new List<Relationship>();
            // Iterate through all the rules
            foreach (KeyValuePair<RelationKey, List<Relationship>> kvp in knowledge)
            {
                foreach (Relationship relationship in kvp.Value)
                {
                    DeduceRule(relationship, potentialNewKnowledge);
                }
            }

            //Attempts to add new rules, won't add rules if they exist
            //already.
            foreach (Relationship r in potentialNewKnowledge)
            {
                 lg.Instance.Line("KnoweldgeBase->Deduction", ProblemType.MESSAGE,r.ToString() + " Adding new rule based on deduction");
                r.Deduction = true;
                Add(r);
            }
        }

        private void DeduceRule(Relationship relationship,
            List<Relationship> potentialNewKnowledge)
        {
            Relationship aIs = relationship;

            //Get the B's
            List<Relationship> theBs = knowledge[aIs.GetRelationKey()];

            // june 12 2009 - was having trouble with foreach working so I did a normal for


            //foreach (Relationship secondRelation in theBs)
            for (int i = 0 ; i < theBs.Count; i++)
            {
                Relationship secondRelation = theBs[i];
                // A is B1, B2, B3 ...
                string b = secondRelation.B;

                RelationKey bRelation = new RelationKey(b, DEDUCED_IS);

                if (!knowledge.ContainsKey(bRelation))
                {
                    // do nothing if we don't have any 'deductions' to make
                    // removed the break beacuse it was messing with looping?
                    //break;
                }
                else
                {

                    List<Relationship> theCs = knowledge[bRelation];

                    foreach (Relationship bIs in theCs)
                    {
                        double newWeight = (aIs.Weight + bIs.Weight) /2 ;
                        int newPriority = Math.Max(aIs.Priority, bIs.Priority);
                        //Create A is C, if it doesn't exist add it.
                        Relationship newRule
                            = new Relationship(aIs.A, DEDUCED_IS, bIs.B
                            , "", newWeight, newPriority, false);
                    

                        potentialNewKnowledge.Add(newRule);
                    }
                }
            }
        }

        /// <summary>
        /// used for "what is" types of questions?
        /// i.e., What is A? Returns array of matches
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Relationship[] MatchRelations(RelationKey key)
        {
            try
            {
                return knowledge[key].ToArray();
            }
            catch (Exception)
            {
                // key not found; return null
                return null;
            }
            
        }
        /// <summary>
        /// had to create this silly class for binding to an array list
        /// </summary>
        public class simplestring
        {
            private string s;
            public string name
            {
                get { return s; }
            }
            public simplestring(string svalue)
            {
                s = svalue;
            }
        }
        /// <summary>
        /// returns relationship in an aray
        /// </summary>
        /// <returns></returns>
        public ArrayList ToArray()
        {
            ArrayList output = new ArrayList();

            foreach (KeyValuePair<RelationKey, List<Relationship>> kvp in knowledge)
            {
                foreach (Relationship relationship in kvp.Value)
                {
                    simplestring simple = new simplestring(relationship.ToString());
                    
                    output.Add(simple);
                }
            }
            return output;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (knowledge.Count == 0)
                return "Empty";

            string output = "";

            foreach (KeyValuePair<RelationKey, List<Relationship>> kvp in knowledge)
            {
                foreach (Relationship relationship in kvp.Value)
                {
                    output += relationship.ToString() + "\r\n";
                }
            }

            return output;
        }
        /// <summary>
        /// overload for using with random name generation
        /// </summary>
        /// <param name="sSubject"></param>
        /// <param name="sContext"></param>
        /// <param name="nAffect"></param>
        /// 
        /// <returns></returns>
        public Relationship GetMostLikelyDeducedFact(string sSubject, string sContext, int nAffect)
        {
            return GetMostLikelyDeducedFact(sSubject, sContext, nAffect, false, null);
        }

        /// <summary>
        /// I found the random thing I added for GetMostLikelyDeducedFact did not work well, tending to pick
        /// items at the end of the list, instead of fairly weighting them, once I had a full alphabet fro erarulelanguage.cs
        /// 
        /// So, I am building a weighted array, wherein we assigned spaces based on the weight
        /// </summary>
        /// <param name="sSubject"></param>
        /// <param name="nr"></param>
        /// <returns></returns>
        public Relationship GetRandomWeightedFact(string sSubject, Random nr)
        {
              Relationship bestRelationship = null;
            // then we look this up
            Relationship[] relationships =
                 this[new RelationKey(sSubject, KnowledgeBase.DEDUCED_IS)];


            if (relationships != null && relationships.Length > 0)
            {




                int totalweight = 0;
                // totalweight
                foreach (Relationship relationship in relationships)
                {
                    totalweight += (int)relationship.Weight;
                }


                // now grab
                int randomnum = nr.Next(0, totalweight);
                foreach (Relationship relationship in relationships)
                {
                    if (randomnum < relationship.Weight)
                    {
                        bestRelationship = relationship;
                        break; // we found one
                    }
                    randomnum = randomnum - (int)relationship.Weight;
                }



                /*
                //FAIL: Without Weighting, lose all grammar. This doesn't really have weighting


                // just grab 2 random positions, compare weight and then move on?
                int spot1 = nr.Next(0, relationships.Length);
                int spot2 = nr.Next(0, relationships.Length);
                if (nr.Next((int)relationships[spot1].Weight) > nr.Next((int)relationships[spot2].Weight))
                {
                    // choose spot 1
                    bestRelationship = relationships[spot1];
                }
                else
                {
                    bestRelationship = relationships[spot2];
                }
                */
            }

            return bestRelationship;
        }

        /// <summary>
        /// Returns a likely rule
        /// * Will only return deductions
        /// </summary>
        /// <param name="sSubject"></param>
        /// <param name="sContext">If not blank and nAffect >0 then if a Rule.C is
        ///         in sConext it increases the likelihood of this rule being selected</param>
        /// <param name="nAffect">The weightt to apply to the rule picking
        /// of context-influenced rules</param>
        /// <param name="bDoesNotHaveToBeADeduction">if true will allow non deductions to qualify AND introduce randomness</param>
        /// <returns>the most acceptable relationship </returns>
        public Relationship GetMostLikelyDeducedFact(string sSubject, string sContext, int nAffect, bool bDoesNotHaveToBeADeduction, Random nr)
        {
            Relationship bestRelationship = null;
            // then we look this up
            Relationship[] relationships =
                 this[new RelationKey(sSubject, KnowledgeBase.DEDUCED_IS)];
            if (relationships != null && relationships.Length > 0)
            {
                // we are picking the most weighted 
                // relation
                // but we are also adjusting the weight of relationship 'near' us
                // I might be able to wrap this into a kb.GetLikely(sBefore+sAfter, 20)
                double nWeight = 0;
              
                string sCurrentRelationship = "";

                foreach (Relationship relationship in relationships)
                {


                    // promote a rule temporarily
                    if ((relationship.Deduction == true || true == bDoesNotHaveToBeADeduction)&& (sCurrentRelationship != relationship.B))
                    {
                        double nPotentialNewWeight = relationship.Weight;

                        if (sContext.IndexOf(relationship.B) > -1)
                        {
                            // the term was located in the previous sentence
                            nPotentialNewWeight = relationship.Weight + nAffect; ;
                        }

                        if (false == bDoesNotHaveToBeADeduction)
                        {
                            // pick heaviest rule
                            if (nPotentialNewWeight > nWeight)
                            {


                                bestRelationship = relationship;
                                nWeight = nPotentialNewWeight;
                                sCurrentRelationship = bestRelationship.B;



                            }
                        }
                        else
                        {
                            // do some randomness here dude
                           // Random nr = new Random(DateTime.Now.Millisecond);
                            int weight1 = nr.Next((int)nWeight);
                            int weight2 = nr.Next((int)nPotentialNewWeight);
                            if (weight2 > weight1)
                            {
                                bestRelationship = relationship;
                                nWeight = nPotentialNewWeight +1; // early letters are at a disadvantage becuase they are tested so many times
                                sCurrentRelationship = bestRelationship.B;
                            }

                        }
                            
                    }

                } //foreach relationship check



            }
            return bestRelationship;
        } // GetMostLikely


        /// <summary>
        /// restores an array of deducted nknowledge
        /// </summary>
        /// <returns></returns>
        public string[] GetDeductions()
        {
            ArrayList arrknow = new ArrayList();
            if (knowledge.Count == 0)
                return new string[1] { "no deductions" };

            //string output = "";

            foreach (KeyValuePair<RelationKey, List<Relationship>> kvp in knowledge)
            {
                foreach (Relationship relationship in kvp.Value)
                {
                    if (relationship.Deduction == true)
                        arrknow.Add(relationship.ToString());
                }
            }

            return (string[]) arrknow.ToArray(typeof(string));
        }

    }
}
