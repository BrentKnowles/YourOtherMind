// TypeConverterYesNo.cs
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
namespace CoreUtilities
{
	public class YesNoTypeConverter : TypeConverter
	{
		
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			
			if (sourceType == typeof(string))
				
				return true;
			
			return base.CanConvertFrom(context, sourceType);
			
		}
		
		
		
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			
			if (destinationType == typeof(string))
				
				return true;
			
			return base.CanConvertTo(context, destinationType);
			
		}
		
		
		
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			
			if (value.GetType() == typeof(string))
			{
				
				if (((string)value).ToLower() == "yes")
					
					return true;
				
				if (((string)value).ToLower() == "no")
					
					return false;
				
				throw new Exception("Values must be \"Yes\" or \"No\"");
				
			}
			
			return base.ConvertFrom(context, culture, value);
			
		}
		
		
		
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			
			if (destinationType == typeof(string))
			{
				
				// - Any class I create will need to do this conversion inside of THAT class
				// for purposes of using the prettyObject
				if (value.GetType() == typeof(string))
				{
					// we need to convert you to boolean bub
					value = (bool) bool.Parse((string)value);
				}
				
				
				return (((bool)value) ? "Yes" : "No");
				
			}
			
			return base.ConvertTo(context, culture, value, destinationType);
			
		}
		
		
		
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			
			return true;
			
		}
		
		
		
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			
			bool[] bools = new bool[] { true, false };
			
			System.ComponentModel.TypeConverter.StandardValuesCollection svc = new System.ComponentModel.TypeConverter.StandardValuesCollection(bools);
			
			return svc;
			
		}
		
		
		
	} // - TypeConverter
}

