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
    /*
    XPO Overview
    ============

    TABLE
        PROPERTIES
        ENDPROPERTIES

        FIELDS
            FIELD
            FIELD
            ...
        ENDFIELDS

        GROUPS
            GROUP
            ENDGROUP
        ENDGROUPS

        INDICES
        ENDINDICES

        REFERENCES
        ENDREFERENCES
    
        DELETEACTIONS
        ENDDELETEACTIONS
    
    ENDTABLE
    */

    /// <summary>
    ///     Evaluation for object type 'Table'.
    /// </summary>
    class axtAppObjEval_Table : axtAppObjEval_Base
    {
        // ----------------------- Internal Data Types -------------------------
        #region Nested data types
        private class axtTableField
        {
            public string Name;
            public string Type;
            public string Label;
            public string StringSize;
            public string ExtendedDatatype;

            public axtTableField()
            {
                Name = "";
                Type = "";
                Label = "";
                StringSize = "";
                ExtendedDatatype = "";
            }
        }


        private class axtTableGroup
        {
            public string Name;
            public string Label;
            public List<string> GroupFields;

            public axtTableGroup()
            {
                Name = "";
                Label = "";
                GroupFields = new List<string>();
            }
        }


        private class axtTableIndex
        {
            public string Name;
            public bool AllowDuplicates;
            public List<string> IndexFields;

            public axtTableIndex()
            {
                Name = "";
                AllowDuplicates = true; // AX standard value
                IndexFields = new List<string>();
            }
        }


        private class axtTableReferenceField
        {
            public string ReferenceType;
            public string Value;
            public string Table;
            public string Field;
            public string RelatedField;
        }


        private class axtTableReference
        {
            public string Name;
            public string Table;
            public List<axtTableReferenceField> FieldReferences;

            public axtTableReference()
            {
                FieldReferences = new List<axtTableReferenceField>();
            }
        }


        private class axtTableDeleteAction
        {
            public string Table;
            public string DeleteAction;
        }
        #endregion


        // ------------------------------ Member -------------------------------
        Dictionary<string, string> m_Properties;
        Dictionary<string, string> m_Methods;
        Dictionary<string, string> m_StaticMethods;
        Dictionary<string, axtTableField> m_Fields;
        Dictionary<string, axtTableGroup> m_FieldGroups;
        Dictionary<string, axtTableIndex> m_Indices;
        Dictionary<string, axtTableReference> m_References;
        Dictionary<string, axtTableDeleteAction> m_DeleteActions;


        // ------------------------------ Fields -------------------------------


        // ---------------------------- Constructor ----------------------------
        public axtAppObjEval_Table(axtAppObj _applicationObject)
            : base(_applicationObject)
        {
            m_Properties = new Dictionary<string, string>();
            m_Methods = new Dictionary<string, string>();
            m_StaticMethods = new Dictionary<string, string>();
            m_Fields = new Dictionary<string, axtTableField>();
            m_FieldGroups = new Dictionary<string, axtTableGroup>();
            m_Indices = new Dictionary<string, axtTableIndex>();
            m_References = new Dictionary<string, axtTableReference>();
            m_DeleteActions = new Dictionary<string, axtTableDeleteAction>();
        }


        // ------------------------------ Methods ------------------------------
        public override void generate()
        {
            string myTableName = "??";
            string CR = Environment.NewLine;
            string[] lines = m_ApplicationObject.RawData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int lin = 0;
            string commentStars = "//****************************************************************************************";
            bool isStatic = false;
            string tmpKey, tmpValue;
            List<string> tmpLines;
            axtTableField tmpTableField; ;
            axtTableGroup tmpTableGroup;
            axtTableIndex tmpTableIndex;
            axtTableReference tmpTableReference;
            axtTableDeleteAction tmpTableDeleteAction;


            // ---------------------------------------------------------------------
            //
            //  Part 1 of 2: Data gereration
            //
            // ---------------------------------------------------------------------


            //-- Find table name
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("TABLE "))
            {
                lin++;
            }
            int pos = lines[lin].IndexOf("#");
            myTableName = lines[lin].Substring(pos + 1);


            //-- Find properties            
            m_Properties.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            lin++;
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDPROPERTIES"))
            {
                lines[lin] = lines[lin].Trim();
                tmpKey = lines[lin].Trim().Substring(0, lines[lin].IndexOf(" "));
                tmpValue = lines[lin].Trim().Substring(lines[lin].IndexOf(" ")).Trim();
                if (tmpValue[0] == '#')
                {
                    tmpValue = tmpValue.Substring(1);
                }
                m_Properties.Add(tmpKey, tmpValue);
                lin++;
            }


            //-- Find fields
            m_Fields.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("FIELDS"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDFIELDS"))
            {
                if (lines[lin].Trim() == "") // Note: no "END FIELD" exists!
                {
                    // Field is finished
                    tmpTableField = evaluateField(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_Fields.Add(tmpTableField.Name, tmpTableField);
                }
                else
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Find groups
            m_FieldGroups.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("GROUPS"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDGROUPS"))
            {
                if (lines[lin].Trim().ToLower() == "endgroup")
                {
                    // Group is finished
                    tmpTableGroup = evaluateGroup(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_FieldGroups.Add(tmpTableGroup.Name, tmpTableGroup);
                }
                else
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Find indices
            m_Indices.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("INDICES"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDINDICES"))
            {
                if (lines[lin].Trim().ToLower() == "endindexfields")
                {
                    // Index is finished
                    tmpTableIndex = evaluateIndex(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_Indices.Add(tmpTableIndex.Name, tmpTableIndex);
                }
                else if (lines[lin].Trim() != "")
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Find references
            m_References.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("REFERENCES"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDREFERENCES"))
            {
                if (lines[lin].Trim().ToLower() == "endreference")
                {
                    // Reference is finished
                    tmpTableReference = evaluateReference(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_References.Add(tmpTableReference.Name, tmpTableReference);
                }
                else if (lines[lin].Trim() != "")
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Find delete actions
            m_DeleteActions.Clear();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("DELETEACTIONS"))
            {
                lin++;
            }
            lin++;
            tmpLines = new List<string>();
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDDELETEACTIONS"))
            {
                if (lines[lin].Trim().ToLower() == "endproperties")
                {
                    // Delete action is finished
                    tmpTableDeleteAction = evaluateDeleteAction(tmpLines.ToArray());
                    tmpLines.Clear();
                    m_DeleteActions.Add(tmpTableDeleteAction.Table, tmpTableDeleteAction);
                }
                else if (lines[lin].Trim() != "")
                {
                    tmpLines.Add(lines[lin]);
                }
                lin++;
            }


            //-- Find methods and static methods
            m_Methods.Clear();
            m_StaticMethods.Clear();
            string CurrentName = "";
            string CurrentSource = "";
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("METHODS"))
            {
                lin++;
            }

            lin++;
            while (lin < lines.Length && !lines[lin].Trim().StartsWith("ENDMETHODS"))
            {
                if (lines[lin].Trim().StartsWith("SOURCE"))
                {
                    // New source begins
                    pos = lines[lin].IndexOf("#");
                    CurrentName = lines[lin].Substring(pos + 1);
                    CurrentSource = "";
                }
                else if (lines[lin].Trim().StartsWith("ENDSOURCE"))
                {
                    // Save
                    if (isStatic)
                    {
                        m_StaticMethods.Add(CurrentName, CurrentSource);
                    }
                    else
                    {
                        m_Methods.Add(CurrentName, CurrentSource);
                    }
                }
                else
                {
                    // Append to current source
                    pos = lines[lin].IndexOf("#");
                    if (CurrentSource == "")
                    {
                        isStatic = lines[lin].ToLower().Contains("static");
                    }
                    CurrentSource += (CurrentSource == "" ? "" : Environment.NewLine) + lines[lin].Substring(pos + 1);
                }
                lin++;
            }




            // ---------------------------------------------------------------------
            //
            //  Part 2 of 2: Output
            //
            // ---------------------------------------------------------------------

            //-- Generate description and content
            //   Note: First try to print class declaration
            m_Description = myTableName;
            m_Text = "/*   Table " + myTableName + CR + CR;


            //-- Generate properties, fields, field groups, indices, delete actions,
            //   references, methods and static methods
            if (m_Properties.Count > 0)
            {
                m_Text += "Properties:" + CR + CR;
                Dictionary<string, string>.Enumerator it = m_Properties.GetEnumerator();
                while (it.MoveNext())
                {
                    m_Text += "   - " + it.Current.Key + ": " + it.Current.Value + CR;
                }
                m_Text += CR + CR;
            }

            if (m_Fields.Count > 0)
            {
                m_Text += "Fields:" + CR + CR;
                Dictionary<string, axtTableField>.Enumerator it = m_Fields.GetEnumerator();
                while (it.MoveNext())
                {
                    tmpTableField = it.Current.Value;
                    tmpValue = "Type=" + tmpTableField.Type;
                    if (tmpTableField.Type.ToLower() == "string")
                    {
                        tmpValue += ", Size=" + tmpTableField.StringSize;
                    }
                    if (tmpTableField.ExtendedDatatype.Trim() != "")
                    {
                        tmpValue += ", EDT=" + tmpTableField.ExtendedDatatype;
                    }
                    if (tmpTableField.Label.Trim() != "")
                    {
                        tmpValue += ", Label=" + tmpTableField.Label;
                    }
                    m_Text += "   - " + tmpTableField.Name + ": " + tmpValue + CR;
                }
                m_Text += CR + CR;
            }

            if (m_FieldGroups.Count > 0)
            {
                m_Text += "Field Groups:" + CR + CR;
                Dictionary<string, axtTableGroup>.Enumerator it = m_FieldGroups.GetEnumerator();
                while (it.MoveNext())
                {
                    tmpTableGroup = it.Current.Value;
                    tmpValue = string.Join(", ", tmpTableGroup.GroupFields.ToArray());
                    m_Text += "   - " + tmpTableGroup.Name + ": " + tmpValue + CR;
                }
                m_Text += CR + CR;
            }

            if (m_Indices.Count > 0)
            {
                m_Text += "Indices:" + CR + CR;
                Dictionary<string, axtTableIndex>.Enumerator it = m_Indices.GetEnumerator();
                while (it.MoveNext())
                {
                    tmpTableIndex = it.Current.Value;
                    tmpValue = tmpTableIndex.AllowDuplicates ? " [unique]" : "";
                    m_Text += "   - " + tmpTableIndex.Name + tmpValue + ": " +
                        string.Join(", ", tmpTableIndex.IndexFields.ToArray()) + CR;
                }
                m_Text += CR + CR;
            }

            if (m_References.Count > 0)
            {
                m_Text += "References:" + CR + CR;
                Dictionary<string, axtTableReference>.Enumerator it = m_References.GetEnumerator();
                while (it.MoveNext())
                {
                    tmpTableReference = it.Current.Value;
                    m_Text += "   - " + tmpTableReference.Name + " (Table: " + tmpTableReference.Table + ")" + CR;
                    foreach (axtTableReferenceField fld in tmpTableReference.FieldReferences)
                    {
                        tmpValue = fld.ReferenceType + " (";
                        switch (fld.ReferenceType.ToUpper())
                        {
                            /*
                               REFERENCETYPE EXTERNFIXED
                                 Value               #31
                                 Table               #ReqPO
                                 RelatedField        #RefType
               
                               REFERENCETYPE THISFIXED
                                 Table               #ReqTrans
                                 Field               #RefType
                                 Value               #32
               
                               REFERENCETYPE NORMAL
                                 Field               #ReqPlanId
                                 RelatedField        #ReqPlanId
                            */
                            // TODO: Validate proper creation for all three table relation types
                            case "EXTERNFIXED":
                                tmpValue += fld.Value + " == " + tmpTableReference.Table + "." + fld.RelatedField + ")";
                                break;

                            case "THISFIXED":
                                tmpValue += tmpTableReference.Table + "." + fld.Field + " == " + fld.Value + ")";
                                break;

                            case "NORMAL":
                                tmpValue += tmpTableReference.Table + "." + fld.RelatedField + " == " + fld.Table + "." + fld.RelatedField + ")";
                                break;

                            default:
                                tmpValue = "???";
                                break;
                        }
                        m_Text += "     " + tmpValue + CR;
                    }
                }
                m_Text += CR + CR;
            }

            if (m_DeleteActions.Count > 0)
            {
                m_Text += "Delete Actions:" + CR + CR;
                Dictionary<string, axtTableDeleteAction>.Enumerator it = m_DeleteActions.GetEnumerator();
                while (it.MoveNext())
                {
                    tmpTableDeleteAction = it.Current.Value;
                    m_Text += "   - " + tmpTableDeleteAction + ": " + tmpTableDeleteAction.DeleteAction + CR;
                }
                m_Text += CR + CR;
            }


            // Method overview
            m_Text += "Methods overview:" + CR + CR;
            Dictionary<string, string>.Enumerator overview = m_Methods.GetEnumerator();
            while (overview.MoveNext())
            {
                m_Text += "   - " + overview.Current.Key + "()" + CR;
            }
            overview = m_StaticMethods.GetEnumerator();
            while (overview.MoveNext())
            {
                m_Text += "   - static " + overview.Current.Key + "()" + CR;
            }
            m_Text += "*/" + CR + CR + CR;


            // Methods
            if (m_Methods.Count > 0)
            {
                m_Text += commentStars + CR + "// Methods" + CR + commentStars + CR + CR;
                Dictionary<string, string>.Enumerator it = m_Methods.GetEnumerator();
                while (it.MoveNext())
                {
                    m_Text += "//" + CR + "// " + it.Current.Key + CR + "//" + CR +
                        it.Current.Value + CR + CR + (it.Current.Value.EndsWith(CR) ? "" : CR);
                }
            }

            if (m_StaticMethods.Count > 0)
            {
                m_Text += commentStars + CR + "// Static Methods" + CR + commentStars + CR + CR;
                Dictionary<string, string>.Enumerator it = m_StaticMethods.GetEnumerator();
                while (it.MoveNext())
                {
                    m_Text += "//" + CR + "// " + it.Current.Key + CR + "//" + CR +
                        it.Current.Value + CR + CR + (it.Current.Value.EndsWith(CR) ? "" : CR);
                }
                m_Text += CR + CR;
            }

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


        // ------------------------- Internal Methods --------------------------
        private axtTableField evaluateField(string[] _rows)
        {
            axtTableField ret = new axtTableField();
            int lin = 0;
            /*
                  FIELD #ItemId
                    string
                    PROPERTIES
                      Name                #ItemId
                      Mandatory           #Yes
                      AllowEditOnCreate   #No
                      AllowEdit           #No
                      Table               #InventTxt
                      ExtendedDataType    
                        ARRAY 
                          #ItemId
                          #
                        ENDARRAY
                      stringSize          #20
                    ENDPROPERTIES
            */
            int pos = _rows[lin].IndexOf("#");
            ret.Name = _rows[lin].Substring(pos + 1);

            lin++;
            ret.Type = _rows[lin].Trim();
            if (ret.Type.ToLower() == "string")
            {
                ret.StringSize = "10"; // default
            }

            while (lin < _rows.Length)
            {
                if (_rows[lin].Trim().StartsWith("stringSize"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.StringSize = _rows[lin].Substring(pos + 1);
                }
                if (_rows[lin].Trim().StartsWith("Label"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.Label = _rows[lin].Substring(pos + 1);
                }
                if (_rows[lin].Trim().StartsWith("ExtendedDataType"))
                {
                    lin += 2;
                    pos = _rows[lin].IndexOf("#");
                    ret.ExtendedDatatype = _rows[lin].Substring(pos + 1);
                }
                lin++;
            }

            return ret;
        }


        private axtTableGroup evaluateGroup(string[] _rows)
        {
            axtTableGroup ret = new axtTableGroup();
            int lin = 0;
            /*
                GROUP #AutoReport
                  PROPERTIES
                    Name                #AutoReport
                    Label               #@SYS7576
                  ENDPROPERTIES
                  
                  GROUPFIELDS
                    #InventSizeId
                    #Name
                    #ItemId
                    #Description
                  ENDGROUPFIELDS
                ENDGROUP
            */
            int pos = _rows[lin].IndexOf("#");
            ret.Name = _rows[lin].Substring(pos + 1);

            lin += 3;
            pos = _rows[lin].IndexOf("#");
            if (_rows[lin].Trim().StartsWith("Label"))
            {
                pos = _rows[lin].IndexOf("#");
                ret.Label = _rows[lin].Substring(pos + 1);
            }

            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("GROUPFIELDS"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("ENDGROUPFIELDS"))
            {
                pos = _rows[lin].IndexOf("#");
                ret.GroupFields.Add(_rows[lin].Substring(pos + 1));
                lin++;
            }

            return ret;
        }


        private axtTableIndex evaluateIndex(string[] _rows)
        {
            axtTableIndex ret = new axtTableIndex();
            int lin = 0;
            /*
               #ItemSizeIdx
               PROPERTIES
                 Name                #ItemSizeIdx
                 AllowDuplicates     #No
               ENDPROPERTIES
               
               INDEXFIELDS
                 #ItemId
                 #InventSizeId
               ENDINDEXFIELDS
            */
            int pos = _rows[lin].IndexOf("#");
            ret.Name = _rows[lin].Substring(pos + 1);

            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("ENDPROPERTIES"))
            {
                if (_rows[lin].Trim().ToLower().StartsWith("allowduplicates"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.AllowDuplicates = (_rows[lin].Substring(pos + 1).ToLower() == "no" ? false : true);
                }
                lin++;
            }

            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("INDEXFIELDS"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("ENDINDEXFIELDS"))
            {
                pos = _rows[lin].IndexOf("#");
                ret.IndexFields.Add(_rows[lin].Substring(pos + 1));
                lin++;
            }

            return ret;
        }


        private axtTableReference evaluateReference(string[] _rows)
        {
            axtTableReference ret = new axtTableReference();
            axtTableReferenceField fld = new axtTableReferenceField();
            List<string> tmpLines = new List<string>();
            int lin = 0;
            /*
               REFERENCE #BOMLine
                 PROPERTIES
                   Name                #BOMLine
                   Table               #ReqPO
                 ENDPROPERTIES
                 
                 FIELDREFERENCES
                   REFERENCETYPE EXTERNFIXED
                   PROPERTIES
                     Value               #31
                     Table               #ReqPO
                     RelatedField        #RefType
                   ENDPROPERTIES
                   
                   REFERENCETYPE THISFIXED
                   PROPERTIES
                     Table               #ReqTrans
                     Field               #RefType
                     Value               #32
                   ENDPROPERTIES
                   
                   REFERENCETYPE NORMAL
                   PROPERTIES
                     Field               #RefId
                     RelatedField        #RefId
                   ENDPROPERTIES
                   
                   REFERENCETYPE NORMAL
                   PROPERTIES
                     Field               #ReqPlanId
                     RelatedField        #ReqPlanId
                   ENDPROPERTIES
                   
                 ENDFIELDREFERENCES
               ENDREFERENCE
            */
            int pos = _rows[lin].IndexOf("#");
            ret.Name = _rows[lin].Substring(pos + 1);
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("Table"))
            {
                lin++;
            }
            pos = _rows[lin].IndexOf("#");
            ret.Table = _rows[lin].Substring(pos + 1);

            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("FIELDREFERENCES"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("ENDFIELDREFERENCES"))
            {
                if (_rows[lin].Trim().StartsWith("ENDPROPERTIES"))
                {
                    fld = evaluateReferenceField(tmpLines.ToArray());
                    ret.FieldReferences.Add(fld);
                    tmpLines.Clear();
                }
                else
                {
                    if (_rows[lin].Trim() != string.Empty)
                        tmpLines.Add(_rows[lin]);
                }

                lin++;
            }

            return ret;
        }


        private axtTableReferenceField evaluateReferenceField(string[] _rows)
        {
            axtTableReferenceField ret = new axtTableReferenceField();
            int lin = 0;
            /*
               REFERENCETYPE EXTERNFIXED
               PROPERTIES
                 Value               #31
                 Table               #ReqPO
                 RelatedField        #RefType
               ENDPROPERTIES
               
               REFERENCETYPE THISFIXED
               PROPERTIES
                 Table               #ReqTrans
                 Field               #RefType
                 Value               #32
               ENDPROPERTIES
               
               REFERENCETYPE NORMAL
               PROPERTIES
                 Field               #ReqPlanId
                 RelatedField        #ReqPlanId
               ENDPROPERTIES
            */
            int pos = _rows[lin].IndexOf("REFERENCETYPE ");
            ret.ReferenceType = _rows[lin].Substring(pos + 14);

            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            lin++;
            while (lin < _rows.Length)
            {
                if (_rows[lin].Trim().ToLower().StartsWith("value"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.Value = _rows[lin].Substring(pos + 1);
                }

                if (_rows[lin].Trim().ToLower().StartsWith("table"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.Table = _rows[lin].Substring(pos + 1);
                }

                if (_rows[lin].Trim().ToLower().StartsWith("field"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.Field = _rows[lin].Substring(pos + 1);
                }

                if (_rows[lin].Trim().ToLower().StartsWith("relatedfield"))
                {
                    pos = _rows[lin].IndexOf("#");
                    ret.RelatedField = _rows[lin].Substring(pos + 1);
                }

                lin++;
            }

            return ret;
        }


        private axtTableDeleteAction evaluateDeleteAction(string[] _rows)
        {
            axtTableDeleteAction ret = new axtTableDeleteAction();
            int lin = 0;
            /*
               #smmTMCallListTable
               PROPERTIES
                 Table               #smmTMCallListTable
                 DeleteAction        #Restricted
               ENDPROPERTIES
               
               #DEL_smmContactPersonDetails
               PROPERTIES
                 Table               #DEL_smmContactPersonDetails
                 DeleteAction        #Cascade
               ENDPROPERTIES
             */
            int pos = _rows[lin].IndexOf("#");
            ret.Table = _rows[lin].Substring(pos + 1);

            lin++;
            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("PROPERTIES"))
            {
                lin++;
            }
            lin++;

            while (lin < _rows.Length && !_rows[lin].Trim().StartsWith("DeleteAction"))
            {
                lin++;
            }

            // TODO: Validate token "deleteaction" and check for default value
            if (_rows[lin].Trim().ToLower().StartsWith("deleteaction"))
            {
                pos = _rows[lin].IndexOf("#");
                ret.DeleteAction = _rows[lin].Substring(pos + 1);
            }

            return ret;
        }

    }

}
