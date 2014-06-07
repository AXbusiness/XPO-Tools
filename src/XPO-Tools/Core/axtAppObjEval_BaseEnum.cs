#region License
// The MIT License (MIT)
//
// Copyright (c) 2014 Stefan Ebert
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace AXbusiness.XpoTools
{
    /// <summary>
    ///     Evaluation for object type 'Base Enum'.
    /// </summary>
    class axtAppObjEval_BaseEnum : axtAppObjEval_Base
    {
        // ----------------------- Internal Data Types -------------------------
        #region Nested data types
        private class axtEnumElement
        {
            public string Name;
            public string Label;
            public string EnumValue;

            public axtEnumElement()
            {
                Name = "";
                Label = "";
                EnumValue = "";
            }
        }
        #endregion


        // ------------------------------ Member -------------------------------
        Dictionary<string, string> m_Properties;
        Dictionary<string, axtEnumElement> m_Elements;

        
        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtAppObjEval_BaseEnum(axtAppObj _applicationObject)
            : base(_applicationObject)
        {
            m_Properties = new Dictionary<string, string>();
            m_Elements = new Dictionary<string, axtEnumElement>();
        }


        // ------------------------------ Methods ------------------------------
        public override void generate()
        {
            string enumName = "??", version = "";
            string CR = Environment.NewLine;
            string[] lines = m_ApplicationObject.RawData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tmpLines;
            axtEnumElement tmpElement;
            string tmpKey, tmpValue;
            int lin = 0;


            //-- Enum version
            while (lin < lines.Length && !lines[lin].Trim().ToUpper().StartsWith("ENUMTYPEVERSION"))
            {
                lin++;
            }
            version = lines[lin].Trim().Split(new char[] { ' ' }, StringSplitOptions.None)[1];
            lin++;


            //-- Enum properties
            while (lin < lines.Length && !lines[lin].Trim().ToUpper().StartsWith("ENUMTYPE"))
            {
                lin++;
            }
            int pos = lines[lin].IndexOf("#");
            enumName = lines[lin].Substring(pos + 1);
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            lin++;
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDPROPERTIES"))
            {
                lines[lin] = lines[lin].Trim();
                tmpKey = lines[lin].Trim().Substring(0, lines[lin].IndexOf(" "));
                tmpValue = lines[lin].Trim().Substring(lines[lin].IndexOf(" ")).Trim();
                if (tmpValue[0] == '#')
                {
                    tmpValue = tmpValue.Substring(1);
                }
                m_Properties.Add(tmpKey, tmpValue);
                lin++;
            }
            lin++;


            //-- Elements
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("TYPEELEMENTS"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDTYPEELEMENTS"))
            {
                if (lines[lin].Trim() == "ENDPROPERTIES")
                {
                    // Element is finished
                    tmpElement = evaluateElement(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_Elements.Add(tmpElement.Name, tmpElement);
                }
                else if (lines[lin].Trim() != "")
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Generate description and content
            m_Description = enumName;
            string label = m_Properties.ContainsKey("Label") ? "Label: " + m_Properties["Label"] + CR : "";
            m_Text = "/*" + CR
                + "Enum: " + enumName + CR
                + label + CR
                + "Elements:" + CR;
            foreach (axtEnumElement el in m_Elements.Values)
            {
                m_Text += "- " + el.EnumValue + ": " + el.Name + " (" + el.Label + ")" + CR;
            }
            m_Text += "*/" + CR;
            resetRtf();
        }

        public override void createRtf()
        {
            if (rtfCreated())
            {
                return;
            }

            m_RtfText = new axtParserXpp(m_Text).generateRtf();
            m_RtfCreated = true;
        }


        // ------------------------- Internal Methods --------------------------
        private axtEnumElement evaluateElement(string[] _rows)
        {
            axtEnumElement ret = new axtEnumElement();
            string tmpKey, tmpValue, keyValue;
            int lin = 0;
            /*
                #None
                PROPERTIES
                  Name                #None
                  Label               #@SYS1369
                  EnumValue           #0
                ENDPROPERTIES
            */
            int pos = _rows[lin].IndexOf("#");
            ret.Name = _rows[lin].Substring(pos + 1);

            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("ENDPROPERTIES"))
            {
                keyValue = _rows[lin].Trim();
                tmpKey = keyValue.Substring(0, keyValue.IndexOf(" "));
                tmpValue = keyValue.Substring(keyValue.IndexOf(" ")).Trim();
                if (tmpValue[0] == '#')
                {
                    tmpValue = tmpValue.Substring(1);
                }
                if (tmpKey.ToLower() == "name")
                {
                    ret.Name = tmpValue;
                }
                else if (tmpKey.ToLower() == "label")
                {
                    ret.Label = tmpValue;
                }
                else if (tmpKey.ToLower() == "enumvalue")
                {
                    ret.EnumValue = tmpValue;
                }
                lin++;
            }

            return ret;
        }
    
    }

}
