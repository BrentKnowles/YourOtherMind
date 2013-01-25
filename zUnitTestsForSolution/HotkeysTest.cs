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
			
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
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
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
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
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
			hotKeyConfig.GetConfigPanel();
			
			Assert.AreEqual(count, Hotkeys.Count);
		}
		[Test]
		public void Dupes()
		{
			string Storage=_TestSingleTon.TESTDATABASE;
			List<KeyData> Hotkeys = new List<KeyData>();
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguida"));
			HotKeyConfig hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(0, hotKeyConfig.Duplicates,"1");

			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.W, Constants.BLANK, false, "dualscreenguidb"));
			hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(1, hotKeyConfig.Duplicates,"2");


			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.X, Constants.BLANK, false, "dualscreenguidc"));
			hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(1, hotKeyConfig.Duplicates,"2b");

			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.C, Constants.BLANK, false, "dualscreenguidd"));
			Hotkeys.Add (new KeyData ("Dual Screen", null, Keys.Control, Keys.C, Constants.BLANK, false, "dualscreenguide"));

			hotKeyConfig = new HotKeyConfig(Storage, ref Hotkeys);
			hotKeyConfig.GetConfigPanel();
			Assert.AreEqual(2, hotKeyConfig.Duplicates,"3");
		}
	}
}

