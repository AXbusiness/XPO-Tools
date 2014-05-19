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
using System.Windows.Forms;

namespace AXbusiness.XpoTools
{
    /// <summary>
    ///     Represents tree containing root nodes for AOT, Projects and Meta information.
    /// </summary>
    class axtAot
    {
        // ------------------------------ Member -------------------------------
        TreeNode m_RootNode;
        Dictionary<axtApplicationObjectType, TreeNode> m_ApplicationObjectDict;


        // ------------------------------ Fields -------------------------------
        public TreeNode RootNode
        {
            get { return m_RootNode; }
        }


        // ---------------------------- Constructor ----------------------------
        public axtAot()
        {
            buildAotSkeleton();
        }


        // ------------------------------ Methods ------------------------------
        public TreeNode findNode(axtApplicationObjectType _objectType)
        {
            if (m_ApplicationObjectDict.ContainsKey(_objectType))
            {
                return m_ApplicationObjectDict[_objectType];
            }
            return null;
        }


        // ------------------------- Internal Methods --------------------------
        private void buildAotSkeleton()
        {
            m_ApplicationObjectDict = new Dictionary<axtApplicationObjectType, TreeNode>();

            // Root nodes
            m_RootNode = new TreeNode("\\");

            TreeNode aotRoot = new TreeNode("AOT");
            addTree(m_RootNode, "Meta information", axtApplicationObjectType.MetaInformation);
            addTree(m_RootNode, "Projects", axtApplicationObjectType.Project);
            m_RootNode.Nodes.Add(aotRoot);

            // AOT nodes
            TreeNode dataDictionary = new TreeNode("Data Dictionary");
            aotRoot.Nodes.Add(dataDictionary);
            addTree(dataDictionary, "Tables", axtApplicationObjectType.Table);
            addTree(dataDictionary, "Enums", axtApplicationObjectType.BaseEnum);
            addTree(dataDictionary, "Extended Data Types", axtApplicationObjectType.ExtendedDataType);

            addTree(aotRoot, "Forms", axtApplicationObjectType.Form);
            addTree(aotRoot, "Classes", axtApplicationObjectType.Class);
            addTree(aotRoot, "Jobs", axtApplicationObjectType.Job);
            addTree(aotRoot, "Macros", axtApplicationObjectType.Macro);
            addTree(aotRoot, "Queries", axtApplicationObjectType.Query);
        }

        private void addTree(TreeNode _parentNode, string _name, axtApplicationObjectType _objectType)
        {
            TreeNode t = new TreeNode(axtAppObj.applicationObjectTypeToString(_objectType));
            _parentNode.Nodes.Add(t);
            m_ApplicationObjectDict.Add(_objectType, t);
        }

    }

}
