// mef_IBase.cs
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
namespace MefAddIns.Extensibility
{
	using System;
	using System.Collections.Generic;
	using System.Text;


 	



	/// <summary>
	/// Defines the functionality that third parties have to implement for my language plug in
	/// </summary>
	public interface mef_IBase
	{
		string Author { get; }
		string Version { get; }
		string Description { get; }
		string Name { get; }
		// used when loading AddIns to mark a second copy (a retained in memory only value)
		bool IsCopy {get;set;}

		// Where this plugin should appear
		PlugInAction CalledFrom { get; }
		// What happens when the menu is clicked or a hotkey called
		// using generci types http://stackoverflow.com/questions/772053/is-it-possible-to-make-a-parameter-implement-two-interfaces
		void RespondToMenuOrHotkey<T>(T form) where T: System.Windows.Forms.Form, MEF_Interfaces.iAccess ;
		// called by utlities such as SendTextAway
		void ActionWithParamForNoteTextActions(object param);
		// if any files need to be generated (such as with SendTextAway, then define this). Empty version created in base class
		string BuildFileNameForActionWithParam();


		// needed to modify the plugin in situation wherein we intentionally have a temporary copy
		void SetGuid(string s);
		//string Tester(string incoming);

		/// <summary>
		/// The hookups, where this addin has been added. This are disposed of when remove
		/// </summary>
		List<IDisposable> Hookups { get; set; }
		void RegisterType();


		void AssignHotkeys(ref List<HotKeys.KeyData> HotKeys, ref mef_IBase addin, Action<mef_IBase> Runner);


		// returns true if we have had to deregister a type
		bool DeregisterType();
		object Storage {get;set;}
		// hooking up a database, usually via string
		void SetStorage(object storage);

		// Defines what the application passes into the ADDIN
		int TypeOfInformationNeeded { get; }
		/// <summary>
		/// Gets the type of information sent back, from the ADDIN
		/// </summary>
		/// <value>
		/// The type of information sent back.
		/// </value>
		int TypeOfInformationSentBack { get; }

		// Passes needed information into the Addin
		void SetBeforeRespondInformation (object neededInfo);


		Action<object, int> DelegateTargetForGetAfterRespondInformation{ get; set; }



		// if QuickLink, then we set this to the ToolStriPButton we created
		object QuickLinkMenuItem{ get; set; }
		// any modifications or return values from the ADDIN, will be influneced by the TypeOfInformationSentBack field
		//void GetAfterRespondInformation ();
		// if this addin puts a QuickLink in the footer than this is the method used to retrieve the form to bring into focus when that link is clicked
		// note we return an object instead of a form to avoid direct reference to Windows.Forms
		object ActiveForm();

		// if set, the addin with this guid needs to be loaded BEFORE this Addin will be loaded
		string dependencyguid { get; }
		// a mininum version #, defaulting to 1.0.0.0  If the ACTUAL version number of the main app is lower then this plugin cannot be loaded
		string dependencymainapplicationversion {get;}

		// used for addins like the Picture one to know where to store captured files
		string path_filelocation { get; set; }



	}
}