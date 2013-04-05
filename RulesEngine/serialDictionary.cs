// serialDictionary.cs
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
using System.Xml.Serialization;

// the base dictionary class is now serializable. This one is.

[XmlRoot("dictionary")]

public class SerializableDictionary<TKey, TValue>

    : Dictionary<TKey, TValue>, IXmlSerializable
{

    #region IXmlSerializable Members

    public System.Xml.Schema.XmlSchema GetSchema()
    {

        return null;

    }



    public void ReadXml(System.Xml.XmlReader reader)
    {

        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));



        bool wasEmpty = reader.IsEmptyElement;

        reader.Read();



        if (wasEmpty)

            return;



        while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
        {

            reader.ReadStartElement("item");



            reader.ReadStartElement("key");

            TKey key = (TKey)keySerializer.Deserialize(reader);

            reader.ReadEndElement();



            reader.ReadStartElement("value");

            TValue value = (TValue)valueSerializer.Deserialize(reader);

            reader.ReadEndElement();



            this.Add(key, value);



            reader.ReadEndElement();

            reader.MoveToContent();

        }

        reader.ReadEndElement();

    }



    public void WriteXml(System.Xml.XmlWriter writer)
    {

        XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));

        XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));



        foreach (TKey key in this.Keys)
        {

            writer.WriteStartElement("item");



            writer.WriteStartElement("key");

            keySerializer.Serialize(writer, key);

            writer.WriteEndElement();



            writer.WriteStartElement("value");

            TValue value = this[key];

            valueSerializer.Serialize(writer, value);

            writer.WriteEndElement();



            writer.WriteEndElement();

        }

    }

    #endregion

}

