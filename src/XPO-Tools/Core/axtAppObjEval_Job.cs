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
    ///     Evaluation for object type 'Job'.
    /// </summary>
    class axtAppObjEval_Job : axtAppObjEval_Base
    {
        // ------------------------------ Member -------------------------------


        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtAppObjEval_Job(axtAppObj _applicationObject)
            : base(_applicationObject)
        {
        }


        // ------------------------------ Methods ------------------------------
        public override void generate()
        {
            string myJobName = "??";
            string CR = Environment.NewLine;
            string[] lines = m_ApplicationObject.RawData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int lin = 0;

            //-- Find job name
            while (lin < lines.Length && !lines[lin].Trim().ToUpper().StartsWith("SOURCE"))
            {
                lin++;
            }
            int pos = lines[lin].IndexOf("#");
            myJobName = lines[lin].Substring(pos + 1);


            //-- Process source code
            string CurrentSource = "";
            lin++;
            while (lin < lines.Length && !lines[lin].Trim().ToUpper().StartsWith("ENDSOURCE"))
            {
                // Append to current source
                pos = lines[lin].IndexOf("#");
                CurrentSource += (CurrentSource == "" ? "" : Environment.NewLine) + lines[lin].Substring(pos + 1);
                lin++;
            }


            //-- Generate description and content
            m_Description = myJobName;
            m_Text = CurrentSource + (CurrentSource.EndsWith(CR) ? "" : CR);
            resetRtf();
        }

        public override void createRtf()
        {
            if (rtfCreated())
            {
                return;
            }

            m_RtfText = new axtParserXpp(m_Text).generateRtf();
            m_RtfCreated = true;
        }

    }

}
