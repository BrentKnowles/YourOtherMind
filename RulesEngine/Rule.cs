// Rule.cs
//
// Copyright (c) 2013 Brent Knowles (http://www.brentknowles.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// Review documentation at http://www.yourothermind.com for updated implementation notes, license updates
// or other general information/
// 
// Author information available at http://www.brentknowles.com or http://www.amazon.com/Brent-Knowles/e/B0035WW7OW
// Full source code: https://github.com/BrentKnowles/YourOtherMind
//###
using System;
using System.Collections.Generic;
using System.Text;
/*
 * 
 * FROM: http://gpwiki.org/index.php/Rule-Based-AI
 * 
 * 
 *There are many places you could take this rather basic code. The order here is roughly easy to hard. 

Allow rules to REMOVE knowledge from the knowledge base 
Negation such as if cheese is NOT visible then Cry() 
Introduce the AND keyword so if mouse is dead then OnMouseDead() AND mood is unhappy 
Variables in rules if x is a mouse then x is mortal 
Add a natural language parser so you can write the rules and knowledge in plain english. 
Consider efficency in even the vaguest sense 
Rules that add new rules to the rule base 
Pass knowledge into the C# functions for instance if x is dead then CommentOnDeath(x) 
Have active knowledge by using functions to check the state of the world. For instance if isMouseDead() then mouse is dead 
Actually tie the rule base to some type of game or simulation 
Meta rules that determine which order the rules should be fired 
Research GOAL searching, rather than our current knowledge generation approach 
Introduction fuzziness we're 50% sure a mouse is an animal. You may have to research fuzzy reasoning.  
 * 
 */
namespace RulesMess
{
    public delegate void RuleFunction();

    public class Rule
    {
        private event RuleFunction thenRule;
        private Relationship ifTrue;
        public Relationship IfTrue
        {
            get { return ifTrue; }
        }

        /// <summary>
        /// On rule being correct add some knowledge
        /// </summary>
        public Rule(Relationship ifTrue, Relationship addToKnowledgeBase,
            KnowledgeBase knowledgeBase)
        {
            this.ifTrue = ifTrue;

            thenRule += delegate()
            {
                knowledgeBase.Add(addToKnowledgeBase);
            };
        }

        public Rule(Relationship ifTrue, RuleFunction functionToFire)
        {
            this.ifTrue = ifTrue;

            thenRule += functionToFire;
        }

        public void Fire()
        {
            if (thenRule != null)
            {
                thenRule();
            }
        }

    }
}
