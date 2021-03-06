// HotkeysTest.cs
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
using NUnit.Framework;
using HotKeys;
using appframe;
using System.Collections.Generic;
using CoreUtilities;
using System.Drawing;
using System.Windows.Forms;

namespace Testing
{
	[TestFixture]
	public class HotkeysTest
	{
		public HotkeysTest ()
		{
		}

//		[Test]
//		public void ModifyAKeyCombinationButThenCancelOutOfOptionsForm ()
//		{
//			Assert.True (false);
//		}
//		[Test]
//		public void ModifyAKeyCombination()
//		{
//			Assert.True (false);
//		}

		[Test]
		public void NoDupes()
		{
			string Storage=_TestSingleTon.TESTDATABASE;
			List<KeyData> Hotkeys = new List<KeyData>();
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguida"));
			
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
			hotKeyConfig.GetConfigPanel();
			
			Assert.AreEqual(0, hotKeyConfig.Duplicates);
		}
		[Test]
		[ExpectedException]
		public void BlankDatabase()
		{
			string Storage=CoreUtilities.Constants.BLANK;
			List<KeyData> Hotkeys = new List<KeyData>();
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguida"));
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
		}
		[Test]
		public void AssureNoDamageDone()
		{
			// lame test but you never know. Makes sure that nothing is lost after creating the panel in the original hotkeys
			string Storage=_TestSingleTon.TESTDATABASE;
			List<KeyData> Hotkeys = new List<KeyData>();
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguida"));
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguidb"));
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguidc"));
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguidd"));

			int count = Hotkeys.Count;
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
			hotKeyConfig.GetConfigPanel();
			
			Assert.AreEqual(count, Hotkeys.Count);
		}
		[Test]
		public void Dupes()
		{
			string Storage=_TestSingleTon.TESTDATABASE;
			List<KeyData> Hotkeys = new List<KeyData>();
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguida"));
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(0, hotKeyConfig.Duplicates,"1");

			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguidb"));
			hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(1, hotKeyConfig.Duplicates,"2");


			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.X, Constants.BLANK, false, "dualscreenguidc"));
			hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(1, hotKeyConfig.Duplicates,"2b");

			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.C, Constants.BLANK, false, "dualscreenguidd"));
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.C, Constants.BLANK, false, "dualscreenguide"));

			hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys, Layout.MasterOfLayouts.GetDatabaseType);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(2, hotKeyConfig.Duplicates,"3");
		}
	}
}

