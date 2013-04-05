// TypeConverterGenericListConverter.cs
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
using System.ComponentModel.Design.Serialization;
using System.ComponentModel;
using System.Collections.Generic;
namespace CoreUtilities
{
	/// January 15 2009 - the actual generic functiosn ahve been moved to their own project in App2010 framework
	/// since they should not reference any application sepcific code
	
	/// <summary>
	/// For adding dropdowns to fields in a propertygrid that are based
	/// on the ClassSubtypes
	/// Just override the Filename
	///         [TypeConverter(typeof(StatusTypeListConverter))]
	///    public string StatusType
	///
	/// </summary>
	public class GenericTypeListConverter : StringConverter
	{
		
		protected virtual List<string> Strings
		{
			get { return new List<string>(); }
		}
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;//return base.GetStandardValuesSupported(context);
		}
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			Strings.Sort ();
//			string[] list = ClassSubtypes.LoadUserInfo(FileName).Subtypes;
//			Array.Sort(list);
			return new StandardValuesCollection(Strings);
		}
		
	} // Listconverter
	
}
