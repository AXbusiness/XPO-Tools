﻿#region License
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
using System.IO;

namespace AXbusiness.XpoTools
{
    /// <summary>
    ///     Represents a XPO file. If loaded successfully, provides the application objects.
    /// </summary>
    class axtXpoFile
    {
        // ------------------------------ Member -------------------------------
        string m_Filename;
        List<axtAppObj> m_ApplicationObjects;


        // ------------------------------ Fields -------------------------------
        public string Filename
        {
            get { return m_Filename; }
            set { m_Filename = value; }
        }

        public List<axtAppObj> ApplicationObjects
        {
            get { return m_ApplicationObjects; }
            set { m_ApplicationObjects = value; }
        }


        // ---------------------------- Constructor ----------------------------
        public axtXpoFile()
            : this(null)
        {
        }
        public axtXpoFile(string _filename)
        {
            m_Filename = _filename;
            m_ApplicationObjects = new List<axtAppObj>();
        }


        // ------------------------------ Methods ------------------------------
        public void load()
        {
            if (!File.Exists(m_Filename))
            {
                throw new ApplicationException(string.Format("File '{0}' does not exist", m_Filename));
            }

            string[] lines = null;
            try
            {
                lines = File.ReadAllLines(m_Filename);
            }
            catch
            {
                throw new ApplicationException(string.Format("Error while loading file '{0}'", m_Filename));
            }

            axtParserXpo parser = new axtParserXpo();
            if (!parser.run(lines))
            {
                throw new ApplicationException(string.Format("Error while parsing file '{0}'", m_Filename));
            }
            m_ApplicationObjects.Clear();
            foreach (axtAppObj obj in parser.ApplicationObjects)
            {
                if (obj.ApplicationObjectType != axtApplicationObjectType.MetaInformation)
                {
                    m_ApplicationObjects.Add(obj);
                }
            }
        }

        public void addApplicationObjects(axtAppObj[] _applicationObjects)
        {
            m_ApplicationObjects.AddRange(_applicationObjects);
        }

        public void saveAs(string _filename)
        {
            // Remove META information elements, if any
            cleanupMetaData();

            // Build content
            StringBuilder s = new StringBuilder(getXpoMetaHeader());
            foreach (axtAppObj o in m_ApplicationObjects)
            {
                s.AppendLine();
                s.Append(o.MetaInformation);
                s.Append(o.RawData);
            }
            s.Append(getXpoMetaFooter());

            // Save file, update filename
            File.WriteAllText(_filename, s.ToString(), Encoding.Default);
            m_Filename = _filename;
        }


        // ------------------------- Internal Methods --------------------------
        private void cleanupMetaData()
        {
            List<axtAppObj> obj = new List<axtAppObj>();
            foreach (axtAppObj o in m_ApplicationObjects)
            {
                if (o.ApplicationObjectType != axtApplicationObjectType.MetaInformation)
                {
                    obj.Add(o);
                }
            }
            m_ApplicationObjects = obj;
        }

        private string getXpoMetaHeader()
        {
            string header = "Exportfile for AOT version 1.0 or later" + Environment.NewLine +
                "Formatversion: 1" + Environment.NewLine;
            return header;
        }

        private string getXpoMetaFooter()
        {
            string footer = Environment.NewLine + "***Element: END" + Environment.NewLine;
            return footer;
        }

    }

}
