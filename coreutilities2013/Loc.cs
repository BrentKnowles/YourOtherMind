using System;
using GNU.Gettext;
using  System.Globalization;

namespace CoreUtilities
{
	/// <summary>
	/// Wrapper singleton for Localization effort.
	/// 
	/// Did not want to implement like this but I was unable to get GetText.Net to create the dlls from the .PO files (it creates them
	/// but they crash). In case I need to move to a different solution I want to wrap it all, simply.
	/// </summary>
	public class Loc 
	{
		public GettextResourceManager Cat = null;
		protected static volatile Loc instance;
		protected static object syncRoot = new Object();

		public Loc()
		{
			Cat = new GettextResourceManager (); 
		}
		/// <summary>
		/// 
		/// </summary>
		public void ChangeLocale (string Locale)
		{
			Console.WriteLine ("needs finished imp");
			string locale = Locale; //"en-US";//"fr-FR";
			System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo (locale);

			// I think we have to reinitialize this now
			Cat = new GettextResourceManager (); 
		}


		public static Loc Instance
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
							instance = new Loc();
						}
					}
				}
				return (Loc)instance;
			}
		}
	}
}

