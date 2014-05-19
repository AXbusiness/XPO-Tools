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
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AXbusiness.XpoTools
{
    /// <summary>
    ///     Form displaying content of a single XPO-file.
    /// </summary>
    partial class axtForm_XpoContentSelection : Form
    {
        // ------------------------------ Member -------------------------------
        axtXpoViewerProject m_Project;
        List<axtAppObj> m_ResultSet;
        bool m_LoadSuccess;
        string m_LoadErrorMessage;


        // ------------------------------ Fields -------------------------------
        public axtAppObj[] ResultSet
        {
            get { return m_ResultSet.ToArray(); }
        }


        // ---------------------------- Constructor ----------------------------
        public axtForm_XpoContentSelection(string _filename)
        {
            InitializeComponent();

            m_Project = new axtXpoViewerProject();
            m_ResultSet = new List<axtAppObj>();
            m_LoadErrorMessage = "";
            m_LoadSuccess = loadXpoFile(_filename);
            if (m_LoadSuccess)
            {
                m_Project.populateTreeview(tvXpoContent);
            }
        }


        // ------------------------------ Methods ------------------------------


        // ------------------------- Internal Methods --------------------------
        private bool loadXpoFile(string _filename)
        {
            bool ok = false;
            try
            {
                m_Project.loadXpoFile(_filename);
                ok = true;
            }
            catch (ApplicationException _ex)
            {
                m_LoadErrorMessage = _ex.Message;
            }

            cmdOK.Enabled = ok;
            chkSelectAll.Enabled = ok;
            txtFilename.Text = _filename;

            return ok;
        }

        private void checkAllChilds(TreeNode _node, bool _checked, bool _includeCurrentNode)
        {
            if (_includeCurrentNode)
            {
                _node.Checked = _checked;
            }

            foreach (TreeNode child in _node.Nodes)
            {
                checkAllChilds(child, _checked, true);
            }
        }

        private void createResultSet(TreeNode _node)
        {
            if (_node.Checked && _node.Tag is axtAppObj)
            {
                m_ResultSet.Add(_node.Tag as axtAppObj);
            }

            foreach (TreeNode child in _node.Nodes)
            {
                createResultSet(child);
            }
        }


        // --------------------------- Eventhandler ----------------------------
        private void frmXpoContentSelection_Load(object sender, EventArgs e)
        {
            if (!m_LoadSuccess)
            {
                Show();
                MessageBox.Show(m_LoadErrorMessage, "Problem while loading");
            }
        }

        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            checkAllChilds(tvXpoContent.Nodes[0], chkSelectAll.Checked, true);
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            m_ResultSet.Clear();
            createResultSet(tvXpoContent.Nodes[0]);
        }

        private void tvXpoContent_AfterCheck(object sender, TreeViewEventArgs e)
        {
            checkAllChilds(e.Node, e.Node.Checked, false);
        }

    }

}
