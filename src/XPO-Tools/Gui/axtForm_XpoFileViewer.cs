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
    ///     Form to view AOT.
    /// </summary>
    partial class axtForm_XpoFileViewer : Form
    {
        // ------------------------------ Member -------------------------------
        axtXpoViewerProject m_Project;
        Font m_Font;
        bool m_DidLoad;
        bool m_SingleXpoLoaded;


        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtForm_XpoFileViewer()
        {
            InitializeComponent();

            m_Project = new axtXpoViewerProject();
            m_Font = new Font("Courier New", 9);
            m_DidLoad = false;
            m_SingleXpoLoaded = false;
        }


        // ------------------------------ Methods ------------------------------


        // ------------------------- Internal Methods --------------------------
        private void fontSelected(Font myFont)
        {
            int prevStart = rtfContent.SelectionStart;
            int prevLength = rtfContent.SelectionLength;

            m_Font = myFont;
            rtfContent.SelectAll();
            rtfContent.SelectionFont = m_Font;
            rtfContent.SelectionStart = prevStart;
            rtfContent.SelectionLength = prevLength;
        }

        private void loadXpo(axtAppObj[] _applicationObjects)
        {
            m_Project.addApplicationObjectsRange(_applicationObjects);
        }

        private void showProject()
        {
            m_Project.populateTreeview(tvApplicationObjects, true);
        }

        private void xpoLoaded(string _filename, string _path)
        {
            if (!m_DidLoad)
            {
                m_DidLoad = true;
                m_SingleXpoLoaded = true;
                lblXpoFilename.Text = _filename;
                lblXpoFilelocation.Text = _path;
            }
            else if (m_SingleXpoLoaded)
            {
                m_SingleXpoLoaded = false;
                lblXpoFilename.Text = string.Format("({0}) and others", lblXpoFilename.Text);
                lblXpoFilelocation.Text = string.Format("({0}) and others", lblXpoFilelocation.Text);
            }
        }

        private void createCheckedNodes(TreeNode _node, List<axtAppObj> _collection)
        {
            if (_node.Checked && _node.Tag is axtAppObj)
            {
                _collection.Add(_node.Tag as axtAppObj);
            }

            foreach (TreeNode child in _node.Nodes)
            {
                createCheckedNodes(child, _collection);
            }
            return;
        }

        private void importMultipleXpoFiles(string _directory)
        {
            string[] files = System.IO.Directory.GetFiles(_directory, "*.xpo", System.IO.SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                MessageBox.Show(string.Format("No XPO files in folder '{0}' and subfolders.", _directory), "Import directory");
            }
            else
            {
                axtForm_XpoContentSelection frm = new axtForm_XpoContentSelection(files);
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    loadXpo(frm.ResultSet);
                    xpoLoaded("[multiple]", _directory);
                    showProject();
                }
            }
        }

        private List<axtAppObj> getCheckedNodes()
        {
            List<axtAppObj> obj = new List<axtAppObj>();
            createCheckedNodes(tvApplicationObjects.TopNode, obj);
            return obj;
        }


        // --------------------------- Eventhandler ----------------------------
        private void cmdImportXpo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string[] files = new string[] { dlg.FileName };
                axtForm_XpoContentSelection frm = new axtForm_XpoContentSelection(files);
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    loadXpo(frm.ResultSet);
                    System.IO.FileInfo fi = new System.IO.FileInfo(dlg.FileName);
                    xpoLoaded(fi.Name, fi.DirectoryName);
                    showProject();
                }
            }
        }

        private void cmdImportXpoMulti_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Select folder to import";
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                importMultipleXpoFiles(dlg.SelectedPath);
            }
        }

        private void cmdExportXpo_Click(object sender, EventArgs e)
        {
            List<axtAppObj> obj = getCheckedNodes();
            if (obj.Count == 0)
            {
                MessageBox.Show("No elements selected for export", "Export XPO");
                return;
            }

            axtXpoFile file = new axtXpoFile();
            file.addApplicationObjects(obj.ToArray());
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.OverwritePrompt = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                string filename = dlg.FileName;
                file.saveAs(filename);
            }
        }

        private void cmdFont_Click(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = m_Font;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                fontSelected(dlg.Font);
            }
        }

        private void tvApplicationObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            rtfContent.Clear();
            if (!(e.Node.Tag is axtAppObj))
            {
                rtfContent.Text = "No Application Object selected.";
                fontSelected(m_Font);
                return;
            }

            axtAppObj obj = e.Node.Tag as axtAppObj;

            Cursor prevCursor = Cursor;
            Cursor = Cursors.WaitCursor;
            obj.Evaluation.createRtf(); // TODO: Have gui option to let user decide, since it's a long running task doing only cosmetic
            Cursor = prevCursor;

            if (obj.Evaluation.RtfText == null)
            {
                rtfContent.Text = obj.Evaluation.Text;
            }
            else
            {
                rtfContent.Rtf = obj.Evaluation.RtfText;
            }
            fontSelected(m_Font);
        }

        private void chkSelectMode_CheckedChanged(object sender, EventArgs e)
        {
            bool newState = chkSelectMode.Checked;
            tvApplicationObjects.CheckBoxes = newState;
            cmdExportXpo.Enabled = newState;
        }

    }

}
