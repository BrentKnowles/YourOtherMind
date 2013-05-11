// PlugInBase.cs
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
using CoreUtilities;
namespace MefAddIns
{
	// Does not Use the Interface! This is intentional.
	public class PlugInBase
	{
		// for a consistent folder name for AddIns
		public const string AddInFolderName = "AddIns";

		public PlugInBase ()
		{
		}

		bool iscopy = false;
		
		public bool IsCopy {
			get{ return iscopy;}
			set{   iscopy = value;}
		}

		List<IDisposable> hookups = null;
		public List<IDisposable>  Hookups {
			get {
				if (null == hookups)
					hookups = new List<IDisposable> ();
				return hookups;
			}
			set { hookups = value;}
			
		}


		protected string guid;
		protected string GUID { 
			get { return  guid; }
			set { guid = value;}
		}
		/// <summary>
		/// Sets the GUID. Only used when overriding to mark it as a 'copy'
		/// </summary>
		/// <param name='s'>
		/// S.
		/// </param>
		public void SetGuid(string s)
		{
			guid = s;
		}
		public object storage = null;
		// This string is set by Addins.cs, when the AddIn, is loaded, passing the preferred storage to the AddIn, in case it needs to write data
		// In most cases this will be a string with the name of the database table
		public object Storage {
			get { return storage;}
			set { storage = value;}
		}
		public void SetStorage (object storage)
		{
			Storage = storage;
		}
		/// <summary>
		/// Registers the type, if this is a NOTETYPE. Other uses will just ignore it.
		/// </summary>
		public virtual void RegisterType()
		{
		}

		public virtual bool DeregisterType()
		{
			return false;
		}
		public virtual string BuildFileNameForActionWithParam()
		{
			return "";
		}

		/// <summary>
		/// Assigns the hotkeys. (Intended to overriden by children, if necessary)
		/// </summary>
		public virtual void AssignHotkeys(ref List<HotKeys.KeyData> HotKeys, ref MefAddIns.Extensibility.mef_IBase addin, Action<MefAddIns.Extensibility.mef_IBase> Runner)
		{
		}
		// default both these to 0 because most plugins will not use them
		public virtual int TypeOfInformationNeeded { get { return 0; } }
		public virtual int TypeOfInformationSentBack { get { return 0; } }
		public virtual void SetBeforeRespondInformation (object neededInfo)
		{
		}
		private Action<object, int> delegateTargetForGetAfterRespondInformation;
		// this is set by MainFormBase, when initializing AddIns
		public Action<object, int> DelegateTargetForGetAfterRespondInformation {
			get { return delegateTargetForGetAfterRespondInformation;}
			set { delegateTargetForGetAfterRespondInformation = value;}
		}
		// Override ride this in target but use this code as skeleton for what is needed
		protected virtual void GetAfterRespondInformation ()
		{
//			if (null == DelegateTargetForGetAfterRespondInformation) {
//
//			} else {
//
//			}


		}
		// the footster toolitem that is created if a quicklink is requeested
		object quickLinkMenuItem=null;
		public object QuickLinkMenuItem {
			get { return quickLinkMenuItem;}
			set { quickLinkMenuItem = value;}
		}

		public  virtual object ActiveForm()
		{
			return null;
		}
		// we set this to blank, by default. Few addins will/should use this
		public virtual string dependencyguid { get { return ""; } }


		public virtual string dependencymainapplicationversion { get { return "1.0.0.0"; }}

		private string _path_filelocation = CoreUtilities.Constants.BLANK;
		// used for addins like the Picture one to know where to store captured files
		public string path_filelocation { 
			get {
			return _path_filelocation	;
			}
			set {
				_path_filelocation = value	;
			}
		}
		protected void RemoveQuickLinks ()
		{
			//NewMessage.Show ("hookups = " + Hookups.Count);
			// remove any 'quicklink hookup';
			IDisposable removeme = null;
			foreach (IDisposable item in Hookups) {
				if (item is System.Windows.Forms.ToolStripButton) {
					//NewMessage.Show ("Found quicklinkA=" + (item as System.Windows.Forms.ToolStripButton).Name);
					//if ( (item as System.Windows.Forms.ToolStripButton).Name != null)
					{
						if ("quicklink" == (item as System.Windows.Forms.ToolStripButton).Name) {
							//NewMessage.Show ("Found quicklinkB");
							// we had a quicklink, created, dispose of it
							lg.Instance.Line ("mef_Addin_Lookup_Word", ProblemType.MESSAGE, "Will be removing Lookup from quicklinks");
							removeme = item;
							break;
						}
					}
					
					
				}
				
			}
			if (removeme != null) {
				removeme.Dispose ();
				Hookups.Remove (removeme);
			}
		}
	}
}

