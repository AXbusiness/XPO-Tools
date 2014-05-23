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
    ///     Transforms raw lines of XPO file into application objects.
    /// </summary>
    class axtParserXpo
    {
        // ------------------------------ Member -------------------------------
        List<axtAppObj> m_ApplicationObjects;


        // ------------------------------ Fields -------------------------------
        public axtAppObj[] ApplicationObjects
        {
            get { return m_ApplicationObjects.ToArray(); }
        }


        // ---------------------------- Constructor ----------------------------
        public axtParserXpo()
        {
            m_ApplicationObjects = null;
        }


        // ------------------------------ Methods ------------------------------
        public bool run(string[] _lines)
        {
            m_ApplicationObjects = new List<axtAppObj>();
            bool ret = true;

            try
            {
                axtAppObj obj = new axtAppObj();
                obj.ApplicationObjectType = axtApplicationObjectType.MetaInformation;
                obj.MetaInformation = "META";
                foreach (string line in _lines)
                {
                    if (line.ToUpper().StartsWith("***ELEMENT:"))
                    {
                        obj.evaluate(); // [Task #0013]: Consider to evaluate application objects later in order to load faster
                        m_ApplicationObjects.Add(obj);
                        obj = getNewEntry(line);
                    }
                    else
                    {
                        obj.RawData += Environment.NewLine + line;
                    }
                }
            }
            catch
            {
                ret = false;
            }
            return ret;
        }


        // ------------------------- Internal Methods --------------------------
        private axtAppObj getNewEntry(string _typeInformation)
        {
            axtAppObj ret = new axtAppObj();
            ret.ApplicationObjectType = axtApplicationObjectType.Undefined;
            ret.MetaInformation = _typeInformation;

            if (!_typeInformation.ToUpper().StartsWith("***ELEMENT:"))
            {
                return ret;
            }

            string element = _typeInformation.Substring(11).Trim().ToUpper();
            switch (element)
            {
                case "DBT":
                    ret.ApplicationObjectType = axtApplicationObjectType.Table;
                    break;

                case "UTS": // EDT: String
                case "UTD": // EDT: Date
                case "UTT": // EDT: Time
                case "UTR": // EDT: Real
                case "UTE": // EDT: Enum
                    ret.ApplicationObjectType = axtApplicationObjectType.ExtendedDataType;
                    break;

                case "QUE":
                    ret.ApplicationObjectType = axtApplicationObjectType.Query;
                    break;

                case "DBE":
                    ret.ApplicationObjectType = axtApplicationObjectType.BaseEnum;
                    break;

                case "MCR":
                    ret.ApplicationObjectType = axtApplicationObjectType.Macro;
                    break;

                case "CLS":
                    ret.ApplicationObjectType = axtApplicationObjectType.Class;
                    break;

                case "FRM":
                    ret.ApplicationObjectType = axtApplicationObjectType.Form;
                    break;

                case "MNU":
                    ret.ApplicationObjectType = axtApplicationObjectType.Menu;
                    break;

                case "FTM":
                    ret.ApplicationObjectType = axtApplicationObjectType.DisplayMenuItem; // TODO: Validate 3 types of menu items
                    break;

                case "PRN":
                    ret.ApplicationObjectType = axtApplicationObjectType.Project;
                    break;

                case "JOB":
                    ret.ApplicationObjectType = axtApplicationObjectType.Job;
                    break;
            }

            return ret;
        }

    }

}
