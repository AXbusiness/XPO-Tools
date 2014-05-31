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
    ///     Represents an AOT object.
    /// </summary>
    class axtAppObj
    {
        // ------------------------------ Member -------------------------------
        string m_MetaInformation;
        string m_RawData;
        axtApplicationObjectType m_ApplicationObjectType;
        axtAppObjEval_Base m_Evaluation;


        // ------------------------------ Fields -------------------------------
        public string MetaInformation
        {
            get { return m_MetaInformation; }
            set { m_MetaInformation = value; }
        }

        public string RawData
        {
            get { return m_RawData; }
            set { m_RawData = value; }
        }

        public axtApplicationObjectType ApplicationObjectType
        {
            get { return m_ApplicationObjectType; }
            set { m_ApplicationObjectType = value; }
        }

        public axtAppObjEval_Base Evaluation
        {
            get { return m_Evaluation; }
            set { m_Evaluation = value; }
        }

        public string Path
        {
            get { return applicationObjectPath(ApplicationObjectType); }
        }


        // ---------------------------- Constructor ----------------------------
        public axtAppObj()
        {
            m_MetaInformation = null;
            m_RawData = null;
            m_ApplicationObjectType = axtApplicationObjectType.Undefined;
            m_Evaluation = null;
        }


        // ------------------------------ Methods ------------------------------
        public void evaluate()
        {
            if (m_Evaluation == null)
            {
                m_Evaluation = axtAppObjEval_Base.construct(this);
                m_Evaluation.generate();
            }
        }


        // ------------------------- Internal Methods --------------------------


        // -------------------------- Static Methods ---------------------------
        public static string applicationObjectTypeToString(axtApplicationObjectType _objectType)
        {
            // Return specific name or fallback to ToString()
            switch (_objectType)
            {
                case axtApplicationObjectType.MetaInformation:
                    return "Meta Information";

                case axtApplicationObjectType.BaseEnum:
                    return "Base Enum";

                case axtApplicationObjectType.ExtendedDataType:
                    return "Extended Data Type";

                default:
                    return _objectType.ToString();
            }
        }

        public static string applicationObjectPath(axtApplicationObjectType _objectType)
        {
            string path = "";

            switch (_objectType)
            {
                case axtApplicationObjectType.Table:
                case axtApplicationObjectType.BaseEnum:
                case axtApplicationObjectType.ExtendedDataType:
                    path = "Data Dictionary";
                    break;

                default:
                    path = "";
                    break;
            }

            if (path != "")
            {
                path += "\\";
            }
            return path + applicationObjectTypeToString(_objectType);
        }

    }

}
