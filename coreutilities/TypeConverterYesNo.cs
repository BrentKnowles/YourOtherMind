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

