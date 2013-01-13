using System;
using System.Collections.Generic;
namespace MefAddIns
{
	// Does not Use the Interface! This is intentional.
	public class PlugInBase
	{
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
		public virtual string BuildFileName()
		{
			return "";
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
		public virtual void GetAfterRespondInformation ()
		{
//			if (null == DelegateTargetForGetAfterRespondInformation) {
//
//			} else {
//
//			}


		}
		public  virtual object ActiveForm()
		{
			return null;
		}
		// we set this to blank, by default. Few addins will/should use this
		public virtual string dependencyguid { get { return ""; } }
	}
}

