// KeyData.cs
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
using System.Windows.Forms;
using System.Xml.Serialization;
using CoreUtilities;

namespace HotKeys
{
	/// <summary>
	/// This class contains details about a specific key-press event.
	/// 
	/// It is a heavy modification of an older system I wrote.
	/// * Key changes: Source Keys are defined VIA code and then Overridden by User preferences [original system had both being defined by XML but doing it this way
	///   allows me to actually specificy Commands direclty instead of a convuluted redirection system
	/// </summary>
	[Serializable()]
	public class KeyData : IComparable , IDisposable
	{
		public KeyData(string _label, Action<bool> _command, Keys _modifyingKey, Keys _key, string _formname, bool _defaultinput, string guid)
		{
			Label = _label;
			Command = _command;
			ModifyingKey = _modifyingKey;
			Key = _key;
			FormName= _formname;
			Defaultinput = _defaultinput;
			GUID =guid ;
		}

		// will be used by the storage system to know if the user has overriden the key
		private string GUID;
		public string GetGUID()
		{
			return GUID;
		}

		public int CompareTo(object obj)
		{
			KeyData u = (KeyData)obj;
			
			return this.Label.CompareTo(u.Label);

		} 
		public void Dispose ()
		{
		}

		private bool defaultinput = false;
		/// <summary>
		/// What is passed to the Function, if called via hotkey
		/// </summary>
		/// <value>
		/// <c>true</c> if defaultinput; otherwise, <c>false</c>.
		/// </value>
		public bool Defaultinput {
			get {
				return defaultinput;
			}
			set {
				defaultinput = value;
			}
		}

		public Action<bool> Command;



		private string label = Loc.Instance.GetString("<enter label here>");
		public string Label
		{
			get { return label; }
			set { label = value; }
		}
		
		private Keys _modifyingKey;
		/// <summary>
		/// 0 - Nothing
		/// 1 - Control
		/// </summary>
		public Keys ModifyingKey
		{
			get { return _modifyingKey; }
			set { _modifyingKey = value; }
		}
		private Keys myKey;
		
		/// <summary>
		/// The key passed
		/// </summary>
		public Keys Key
		{
			get { return myKey; }
			set { myKey = value; }
		}
		
//		private int functionCode;
//		
//		/// <summary>
//		/// The reference to the calling engine of which function to be called
//		/// </summary>
//		public int FunctionCode
//		{
//			get { return functionCode; }
//			set { functionCode = value; }
//		}
		
		private string formName;
		
		/// <summary>
		/// If Form = "" then this will be a global response
		/// If Form is the name of a form then it will only apply to that form
		/// </summary>
		public string FormName
		{
			get { return formName; }
			set { formName = value; }
		}
		
		
		public override string ToString ()
		{
			return string.Format ("[KeyData: Defaultinput={0}, Label={1}, ModifyingKey={2}, Key={3}, FormName={4}]", Defaultinput, Label, ModifyingKey, Key, FormName);
		}

	}
}