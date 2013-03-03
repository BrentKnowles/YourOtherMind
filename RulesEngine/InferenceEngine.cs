using System;
using System.Collections.Generic;
using System.Text;

namespace RulesMess
{
    public class InferenceEngine
    {
        public void Infer(List<Rule> rules, KnowledgeBase kb)
        {
            foreach (Rule rule in rules)
            {
                Relationship[] knowledgePieces =
                    kb.MatchRelations(rule.IfTrue.GetRelationKey());

                if (CanFireRule(rule.IfTrue, knowledgePieces))
                {
                    rule.Fire();
                }
            }
        }

        private bool CanFireRule(Relationship ifTrue, Relationship[] knowledgePieces)
        {
            foreach (Relationship r in knowledgePieces)
            {
                if (r.B == ifTrue.B)
                    return true;
            }

            return false;
        }
    }
}
