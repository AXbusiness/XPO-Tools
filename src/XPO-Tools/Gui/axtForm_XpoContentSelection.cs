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
        List<string> m_LoadErrorMessages;


        // ------------------------------ Fields -------------------------------
        public axtAppObj[] ResultSet
        {
            get { return m_ResultSet.ToArray(); }
        }


        // ---------------------------- Constructor ----------------------------
        public axtForm_XpoContentSelection(string[] _filenames)
        {
            InitializeComponent();
            int planned = 0, success = 0;

            m_Project = new axtXpoViewerProject();
            m_ResultSet = new List<axtAppObj>();
            m_LoadErrorMessages = new List<string>();

            foreach (string file in _filenames)
            {
                planned++;
                success += loadXpoFile(file) ? 1 : 0;
            }
            m_LoadSuccess = planned == success;
            m_Project.populateTreeview(tvXpoContent, true);

            cmdOK.Enabled = success > 0;
            chkSelectAll.Enabled = success > 0;
            txtFilename.Text = _filenames.Length == 1 ? _filenames[0] : "[multiple]";
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
                m_LoadErrorMessages.Add(_ex.Message);
            }

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
                int i = 0;
                string errors = "", exceed5 = "";

                foreach (string msg in m_LoadErrorMessages)
                {
                    errors = errors + msg + Environment.NewLine;
                    i++;
                    if (i >= 5)
                    {
                        break;
                    }
                }
                if (m_LoadErrorMessages.Count > 5)
                {
                    exceed5 = string.Format("{0}... in summary, {1} errors occured. Only first 5 are displayed.", Environment.NewLine, m_LoadErrorMessages.Count);
                }
                MessageBox.Show("Load errors:" + Environment.NewLine + errors + exceed5, "Problems while loading");
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
