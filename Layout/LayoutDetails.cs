using System;
using System.Collections;
using CoreUtilities;
namespace Layout
{
	/// <summary>
	/// Wiill hold info about NoteTypes, About the active layoutpanel
	/// </summary>
	public class LayoutDetails
	{
		#region variables
		protected static volatile LayoutDetails instance;
		protected static object syncRoot = new Object();
		

#endregion;
		public LayoutDetails ()
		{
			//TypeList = new Type[4] {typeof(NoteDataXML), typeof(NoteDataXML_RichText), typeof(NoteDataXML_NoteList), typeof(NoteDataXML_SystemOnly)};
			TypeList = new ArrayList();
			NameList = new ArrayList();
			AddToList(typeof(NoteDataXML),Loc.Instance.Cat.GetString("Label"));
			AddToList(typeof(NoteDataXML_RichText),Loc.Instance.Cat.GetString("Text"));
			AddToList (typeof(NoteDataXML_NoteList),Loc.Instance.Cat.GetString("List"));
			AddToList (typeof(NoteDataXML_SystemOnly),Loc.Instance.Cat.GetString("System**"));


		}


		// other PLACES will modify this list, when registering new types
		private ArrayList TypeList = null;
		private ArrayList NameList = null;

		/// <summary>
		/// Adds to both lists.
		/// Will not add it if it already esxists
		/// </summary>
		/// <param name='newType'>
		/// New type.
		/// </param>
		/// <param name='newAssembly'>
		/// New assembly.
		/// </param>
		public void AddToList (Type newType, string name)
		{
			if (TypeList.IndexOf (newType) == -1) {
				TypeList.Add (newType);
				NameList.Add (name);
			}

		}
		/// <summary>
		/// Gets the type of the name from.
		/// Assumes namelist and typelist are equal at all times
		/// </summary>
		/// <returns>
		/// The name from type.
		/// </returns>
		/// <param name='lookupType'>
		/// Lookup type.
		/// </param>
		public string GetNameFromType (Type lookupType)
		{
			if (NameList.Count != TypeList.Count) {
				throw new Exception("Must be same number of type names as types");
			}
			int index = TypeList.IndexOf (lookupType);
			if (index >= 0) {
				return NameList[index].ToString ();
			}
			return Constants.ERROR;
		}
		public Type[] ListOfTypesToStoreInXML ()
		{
			Type[] ArrayOfTypes = new Type[TypeList.Count];
			TypeList.CopyTo(ArrayOfTypes);
			return ArrayOfTypes;
		}

		public LayoutPanelBase CurrentLayout()
		{
			throw new Exception("not done");
		}

		public static LayoutDetails Instance
		{
			get
			{
				if (null == instance)
				{
					// only one instance is created and when needed
					lock (syncRoot)
					{
						if (null == instance)
						{
							instance = new LayoutDetails();
						}
					}
				}
				return (LayoutDetails)instance;
			}
		}
	}
}




