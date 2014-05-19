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
    public partial class axtForm_XpoFileViewer : Form
    {
        // ------------------------------ Member -------------------------------
        axtXpoViewerProject m_Project;
        Font m_Font;


        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtForm_XpoFileViewer()
        {
            InitializeComponent();

            m_Project = new axtXpoViewerProject();
            m_Font = new Font("Courier New", 9);
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
            m_Project.populateTreeview(tvApplicationObjects);
        }


        // --------------------------- Eventhandler ----------------------------
        private void cmdImportXpo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.CheckFileExists = true;
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                axtForm_XpoContentSelection frm = new axtForm_XpoContentSelection(dlg.FileName);
                frm.ShowDialog(this);
                if (frm.DialogResult == DialogResult.OK)
                {
                    loadXpo(frm.ResultSet);
                    showProject();
                }
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
            rtfContent.Text = e.ToString();
        }

    }

}
