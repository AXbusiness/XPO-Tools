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
    /*
    XPO Overview
    ============

    USERTYPE #AccountCategory
    STRING
      PROPERTIES
        Keys....            #Values....
      ENDPROPERTIES
      
      TYPEELEMENTS
        n TYPEELEMENT ... sub-type   (evaluateTypeElement...)
      ENDTYPEELEMENTS
      
      TYPEREFERENCES
        n TYPEREFERENCETYPE ... sub-type   (evaluateTypeReference...)
      ENDTYPEREFERENCES
      
    ENDUSERTYPE
    */

    /// <summary>
    ///     Evaluation for object type 'Extended datatype'.
    /// </summary>
    class axtAppObjEval_EDT : axtAppObjEval_Base
    {
        // ----------------------- Internal Data Types -------------------------
        #region Nested data types
        private class axtTypeElement
        {
            public string Index;
            public string Label;
            public string HelpText;
            public List<axtTypeReference> TypeReferences;

            public axtTypeElement()
            {
                Index = "";
                Label = "";
                HelpText = "";
                TypeReferences = new List<axtTypeReference>();
            }
        }

        private class axtTypeReference
        {
            public string ReferenceType;
            public string Table;
            public string RelatedField;
            public string Value;

            public axtTypeReference()
            {
                ReferenceType = "";
                Table = "";
                RelatedField = "";
                Value = "";
            }
        }
        #endregion


        // ------------------------------ Member -------------------------------
        Dictionary<string, string> m_Properties;
        List<axtTypeElement> m_TypeElements;
        List<axtTypeReference> m_TypeReferences;


        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtAppObjEval_EDT(axtAppObj _applicationObject)
            : base(_applicationObject)
        {
            m_Properties = new Dictionary<string, string>();
            m_TypeElements = new List<axtTypeElement>();
            m_TypeReferences = new List<axtTypeReference>();
        }


        // ------------------------------ Methods ------------------------------
        public override void generate()
        {
            string edtName = "??";
            string edtVersion = "";
            string CR = Environment.NewLine;
            string[] lines = m_ApplicationObject.RawData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int lin = 0;

            //-- Find EDT version
            while (lin < lines.Length && !lines[lin].Trim().ToUpper().StartsWith("USERTYPEVERSION"))
            {
                lin++;
            }
            int pos = lines[lin].IndexOf(" ");
            edtVersion = lines[lin].Substring(pos + 1); // TODO: Have a member in base class to save version
            lin++;


            //-- Find EDT name
            while (lin < lines.Length && !lines[lin].Trim().ToUpper().StartsWith("USERTYPE"))
            {
                lin++;
            }
            pos = lines[lin].IndexOf("#");
            edtName = lines[lin].Substring(pos + 1);
            lin++;


            //-- EDT type
            string edtType = lines[lin].Trim();
            lin++;


            //-- Process Properties
            string tmpKey = "", tmpValue = "";
            m_Properties.Clear();
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


            //-- Process type elements (Array elements)
            List<string> tmpLines = new List<string>();
            m_TypeElements.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("TYPEELEMENTS"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDTYPEELEMENTS"))
            {
                if (lines[lin].Trim().ToUpper() == "ENDTYPEELEMENT")
                {
                    // Type element is finished
                    axtTypeElement tmpTypeElement = evaluateTypeElement(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_TypeElements.Add(tmpTypeElement);
                }
                else
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Process type references (Relations)
            tmpLines = new List<string>();
            m_TypeReferences.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("TYPEREFERENCES"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDTYPEREFERENCES"))
            {
                if (lines[lin].Trim().ToUpper() == "ENDPROPERTIES")
                {
                    // Type reference is finished
                    axtTypeReference tmpTypeReference = evaluateTypeReference(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_TypeReferences.Add(tmpTypeReference);
                }
                else
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Generate description and content
            m_Description = edtName;
            m_Text = "/*" + CR +
                "Type: " + edtType + CR;

            if (m_TypeElements.Count > 0)
            {
                m_Text += "Array elements:" + CR;
                foreach (axtTypeElement t in m_TypeElements)
                {
                    string helpText = t.HelpText == "" ? "" : " (" + t.HelpText + ")";
                    m_Text += string.Format("  - Index {0}) {1}{2}", t.Index, t.Label, helpText) + CR;
                    if (t.TypeReferences.Count > 0)
                    {
                        m_Text += "    Relations:" + CR;
                        foreach (axtTypeReference tref in t.TypeReferences)
                        {
                            string compare = tref.ReferenceType.ToUpper() == "EXTERNFIXED" ? " == " + tref.Value : "";
                            m_Text += string.Format("      - {0}.{1}{2} ({3})", tref.Table, tref.RelatedField, compare, tref.ReferenceType) + CR;
                        }
                    }
                }
            }
            if (m_TypeReferences.Count > 0)
            {
                m_Text += "Relations:" + CR;
                foreach (axtTypeReference tref in m_TypeReferences)
                {
                    string compare = tref.ReferenceType.ToUpper() == "EXTERNFIXED" ? " == " + tref.Value : "";
                    m_Text += string.Format("  - {0}.{1}{2} ({3})", tref.Table, tref.RelatedField, compare, tref.ReferenceType) + CR;
                }
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
        private axtTypeElement evaluateTypeElement(string[] _rows)
        {
            // In AOT, the "Array Elements" node

            /*
                TYPEELEMENTS
                  TYPEELEMENT
                    PROPERTIES
                      Index               #7
                      Label               #@SYS630
                      HelpText            #@SYS631
                    ENDPROPERTIES
        
                    TYPEREFERENCES
                      TYPEREFERENCETYPE DATASET
                      PROPERTIES
                        Table               #Dimensions
                        RelatedField        #Num
                      ENDPROPERTIES
          
                      TYPEREFERENCETYPE EXTERNFIXED
                      PROPERTIES
                        Table               #Dimensions
                        RelatedField        #DimensionCode
                        Value               #6
                      ENDPROPERTIES
          
                    ENDTYPEREFERENCES
        
                  ENDTYPEELEMENT
      
                ENDTYPEELEMENTS
            */

            axtTypeElement ret = new axtTypeElement();
            int lin = 0;
            while (lin < _rows.Length && !_rows[lin].Trim().ToUpper().StartsWith("TYPEELEMENT"))
            {
                lin++;
            }
            lin++;

            while (lin < _rows.Length && !_rows[lin].ToUpper().Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            Dictionary<string, string> properties = axtAppObjEval_Base.parseProperties(_rows, lin);

            ret.Index = properties.ContainsKey("Index") ? properties["Index"] : "";
            ret.Label = properties.ContainsKey("Label") ? properties["Label"] : "";
            ret.HelpText = properties.ContainsKey("HelpText") ? properties["HelpText"] : "";

            // Just behind 'PROPERTIES', there's a list of 'TYPEREFERENCES...ENDTYPEREFERENCES'
            while (lin < _rows.Length && !_rows[lin].Trim().ToUpper().StartsWith("ENDPROPERTIES"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().ToUpper().StartsWith("TYPEREFERENCES"))
            {
                lin++;
            }
            lin++;
            List<string> tmpLines = new List<string>();
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("ENDTYPEREFERENCES"))
            {
                if (_rows[lin].Trim().ToUpper() == "ENDPROPERTIES")
                {
                    axtTypeReference tmpTypeReference = evaluateTypeReference(tmpLines.ToArray());
                    tmpLines.Clear();
                    ret.TypeReferences.Add(tmpTypeReference);
                }
                else
                {
                    tmpLines.Add(_rows[lin]);
                }
                lin++;
            }

            return ret;
        }

        private axtTypeReference evaluateTypeReference(string[] _rows)
        {
            // In AOT, the "Relations" node
            // Also used for "TYPEREFERENCES" subnode inside "TYPEELEMENT" section

            /*
                TYPEREFERENCES
                    
                    TYPEREFERENCETYPE DATASET
                    PROPERTIES
                    Table               #Dimensions
                    RelatedField        #Num
                    ENDPROPERTIES
                    
                    TYPEREFERENCETYPE EXTERNFIXED
                    PROPERTIES
                    Table               #Dimensions
                    RelatedField        #DimensionCode
                    ENDPROPERTIES
                    
                    ..
                    
                ENDTYPEREFERENCES
            */

            axtTypeReference ret = new axtTypeReference();
            int lin = 0;
            while (lin < _rows.Length && !_rows[lin].Trim().ToUpper().StartsWith("TYPEREFERENCETYPE"))
            {
                lin++;
            }
            int pos = _rows[lin].TrimStart().IndexOf(" ");
            ret.ReferenceType = _rows[lin].TrimStart().Substring(pos + 1);
            lin++;

            while (lin < _rows.Length && !_rows[lin].ToUpper().Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            Dictionary<string, string> properties = axtAppObjEval_Base.parseProperties(_rows, lin);

            ret.Table = properties.ContainsKey("Table") ? properties["Table"] : "";
            ret.RelatedField = properties.ContainsKey("RelatedField") ? properties["RelatedField"] : "";
            ret.Value = properties.ContainsKey("Value") ? properties["Value"] : "";

            return ret;
        }

    }

}
