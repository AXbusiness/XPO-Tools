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
using System.Windows.Forms;
using System.Text;

namespace AXbusiness.XpoTools
{
    /// <summary>
    ///     Represents a project which may contain many application objects.
    /// </summary>
    class axtXpoViewerProject
    {
        // ------------------------------ Member -------------------------------
        List<axtAppObj> m_ApplicationObjects;


        // ------------------------------ Fields -------------------------------
        public axtAppObj[] ApplicationObjects
        {
            get { return m_ApplicationObjects.ToArray(); }
        }


        // ---------------------------- Constructor ----------------------------
        public axtXpoViewerProject()
        {
            m_ApplicationObjects = new List<axtAppObj>();
        }


        // ------------------------------ Methods ------------------------------
        public void addApplicationObjectsRange(axtAppObj[] _applicationObjects)
        {
            m_ApplicationObjects.AddRange(_applicationObjects);
        }

        public void loadXpoFile(string _filename)
        {
            m_ApplicationObjects = new List<axtAppObj>();
            axtXpoFile xpo = new axtXpoFile(_filename);
            try
            {
                xpo.load();
            }
            catch (ApplicationException)
            {
                throw;
            }
            m_ApplicationObjects.Clear();
            m_ApplicationObjects.AddRange(xpo.ApplicationObjects);
        }

        public void populateTreeview(TreeView _tv, bool _expandAll)
        {
            axtAot aotSkeleton = new axtAot();
            _tv.Nodes.Clear();
            _tv.Nodes.Add(aotSkeleton.RootNode);

            foreach (axtAppObj obj in m_ApplicationObjects)
            {
                TreeNode nParent = aotSkeleton.findNode(obj.ApplicationObjectType);
                if (nParent != null)
                {
                    TreeNode nNew = new TreeNode(obj.Evaluation.Description);
                    nNew.Tag = obj;
                    nParent.Nodes.Add(nNew);
                }
            }

            if (_expandAll)
            {
                _tv.ExpandAll();
            }
            else
            {
                _tv.Nodes[0].Expand();
            }
        }

    }

}
