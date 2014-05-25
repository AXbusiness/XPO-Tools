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
    ///     Evaluation for object type 'Class'.
    /// </summary>
    class axtAppObjEval_Class : axtAppObjEval_Base
    {
        // ------------------------------ Member -------------------------------
        string m_ClassDeclaration;
        string m_Extends;
        string m_Implements;
        Dictionary<string, string> m_Methods;
        Dictionary<string, string> m_StaticMethods;


        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtAppObjEval_Class(axtAppObj _applicationObject)
            : base(_applicationObject)
        {
            m_ClassDeclaration = null;
            m_Extends = null;
            m_Implements = null;
            m_Methods = new Dictionary<string, string>();
            m_StaticMethods = new Dictionary<string, string>();
        }


        // ------------------------------ Methods ------------------------------
        public override void generate()
        {
            string myClassName = "??";
            string CR = Environment.NewLine;
            string[] lines = m_ApplicationObject.RawData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int lin = 0;
            string textClassDeclaration = "Class Declaration";
            string commentStars = "//****************************************************************************************";
            bool isStatic = false;

            //-- Find class name
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("CLASS") && !lines[lin].Trim().StartsWith("INTERFACE"))
            {
                lin++;
            }

            int pos = lines[lin].IndexOf("#");
            myClassName = lines[lin].Substring(pos + 1);


            //-- Find base class
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("Extends"))
            {
                lin++;
            }
            pos = lines[lin].IndexOf("#");
            m_Extends = lines[lin].Substring(pos + 1);


            //-- Find class declaration, methods and static methods
            m_ClassDeclaration = "";
            m_Methods.Clear();
            m_StaticMethods.Clear();
            string CurrentName = "";
            string CurrentSource = "";
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("METHODS"))
            {
                lin++;
            }

            lin++;
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDMETHODS"))
            {
                if (lines[lin].Trim().StartsWith("SOURCE"))
                {
                    // New source begins
                    pos = lines[lin].IndexOf("#");
                    CurrentName = lines[lin].Substring(pos + 1);
                    if (CurrentName.ToLower() == "classdeclaration")
                        CurrentName = textClassDeclaration;
                    CurrentSource = "";
                }
                else if (lines[lin].Trim().StartsWith("ENDSOURCE"))
                {
                    // Save
                    if (CurrentName == textClassDeclaration)
                    {
                        m_ClassDeclaration = CurrentSource;
                    }
                    else if (isStatic)
                    {
                        m_StaticMethods.Add(CurrentName, CurrentSource);
                    }
                    else
                    {
                        m_Methods.Add(CurrentName, CurrentSource);
                    }
                }
                else
                {
                    // Append to current source
                    pos = lines[lin].IndexOf("#");
                    if (CurrentSource == "")
                    {
                        isStatic = lines[lin].ToLower().Contains("static");
                    }
                    CurrentSource += (CurrentSource == "" ? "" : Environment.NewLine) + lines[lin].Substring(pos + 1);
                }
                lin++;
            }


            //-- Generate description and overview
            // TODO: Outline public interface of class in overview
            // TODO: Show hierarchy (if available)
            m_Description = myClassName;
            string overview = "/*" + CR + "    Class name: " + myClassName + CR;
            if (!string.IsNullOrEmpty(m_Extends))
            {
                overview += "    Base class: " + m_Extends + CR;
            }
            overview += CR + "    Methods: " + m_Methods.Count.ToString() + CR +
                "    Static Methods: " + m_StaticMethods.Count.ToString() + CR + "*/" + CR + CR;

            //-- Generate content
            m_Text = overview;

            m_Text += commentStars + CR + "// " + textClassDeclaration + CR + commentStars + CR + CR +
                m_ClassDeclaration + CR + CR + (m_ClassDeclaration.EndsWith(CR) ? "" : CR);

            if (m_Methods.Count > 0)
            {
                m_Text += commentStars + CR + "// Methods" + CR + commentStars + CR + CR;
            }
            Dictionary<string, string>.Enumerator it = m_Methods.GetEnumerator();
            while (it.MoveNext())
            {
                m_Text += "//" + CR + "// " + it.Current.Key + CR + "//" + CR +
                    it.Current.Value + CR + CR + (it.Current.Value.EndsWith(CR) ? "" : CR);
            }

            if (m_StaticMethods.Count > 0)
            {
                m_Text += commentStars + CR + "// Static Methods" + CR + commentStars + CR + CR;
            }
            it = m_StaticMethods.GetEnumerator();
            while (it.MoveNext())
            {
                m_Text += "//" + CR + "// " + it.Current.Key + CR + "//" + CR +
                    it.Current.Value + CR + CR + (it.Current.Value.EndsWith(CR) ? "" : CR);
            }

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
    }

}
