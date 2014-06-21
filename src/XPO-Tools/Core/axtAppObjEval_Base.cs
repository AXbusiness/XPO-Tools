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
    ///     Base class for application object evaluations.
    /// </summary>
    abstract class axtAppObjEval_Base
    {
        // ------------------------------ Member -------------------------------
        protected string m_Description;
        protected string m_Text;
        protected string m_RtfText;
        protected bool m_RtfCreated;
        protected axtAppObj m_ApplicationObject;


        // ------------------------------ Fields -------------------------------
        public string Description
        {
            get { return m_Description; }
        }

        public string Text
        {
            get { return m_Text; }
        }

        public string RtfText
        {
            get { return m_RtfText; }
        }


        // ---------------------------- Constructor ----------------------------
        protected axtAppObjEval_Base(axtAppObj _applicationObject)
        {
            m_Description = null;
            m_Text = null;
            m_RtfText = null;
            m_RtfCreated = false;
            m_ApplicationObject = _applicationObject;
        }


        // ------------------------------ Methods ------------------------------
        public virtual void generate()
        {
            m_Description = "???";
            m_Text = "???";
            m_RtfText = null;
        }

        public virtual void createRtf()
        {
            m_RtfText = null;
            m_RtfCreated = true;
        }


        // ------------------------- Internal Methods --------------------------
        protected bool rtfCreated()
        {
            return m_RtfCreated;
        }

        protected void resetRtf()
        {
            m_RtfText = null;
            m_RtfCreated = false;
        }


        // -------------------------- Static Methods ---------------------------
        public static axtAppObjEval_Base construct(axtAppObj _applicationObject)
        {
            switch (_applicationObject.ApplicationObjectType)
            {
                case axtApplicationObjectType.Job:
                    return new axtAppObjEval_Job(_applicationObject);

                case axtApplicationObjectType.Macro:
                    return new axtAppObjEval_Macro(_applicationObject);

                case axtApplicationObjectType.Class:
                    return new axtAppObjEval_Class(_applicationObject);

                case axtApplicationObjectType.Table:
                    return new axtAppObjEval_Table(_applicationObject);

                case axtApplicationObjectType.BaseEnum:
                    return new axtAppObjEval_BaseEnum(_applicationObject);

                default:
                    return new axtAppObjEval_Default(_applicationObject);
            }
        }

        public static Dictionary<string, string> parseProperties(string[] _lines, int _idxStart)
        {
            /*
                  PROPERTIES
                    Keys....            #Values....
                  ENDPROPERTIES
            */
            Dictionary<string, string> ret = new Dictionary<string, string>();
            int lin = _idxStart;
            string s = "", tmpKey = "", tmpValue = "";

            if (!_lines[lin].Trim().ToUpper().StartsWith("PROPERTIES"))
            {
                return ret;
            }
            lin++;
            while (lin < _lines.Length && !_lines[lin].ToUpper().Trim().StartsWith("ENDPROPERTIES"))
            {
                s = _lines[lin].Trim();
                tmpKey = s.Substring(0, s.IndexOf(" "));
                tmpValue = s.Substring(s.IndexOf(" ")).Trim();
                if (tmpValue[0] == '#')
                {
                    tmpValue = tmpValue.Substring(1);
                }
                ret.Add(tmpKey, tmpValue);
                lin++;
            }

            return ret;
        }

    }

}
