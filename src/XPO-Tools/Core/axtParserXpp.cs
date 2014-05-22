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
using System.Drawing;

namespace AXbusiness.XpoTools
{
    /// <summary>
    ///     Parses privided text and produces formatted RTF text for X++ language.
    /// </summary>
    class axtParserXpp
    {
        // ------------------------------ Member -------------------------------
        string m_Data;
        int m_CurrentPos;
        int m_Length;
        axtParserKindOfPhrase m_CurrentPhrase;
        string m_Stack;

        private readonly string[] KnownKeywords = {
            "if", "else", "for", "next", "while", "do", "break", "switch", "case", "default", "return",
            "true", "false", "null", "new",
            "throw", "try", "catch", "ttsbegin", "ttscommit", "ttsabort",
            "class", "public", "private", "protected", "server", "client", "this",
            "static", "super", "final", "extends",
            "void", "str", "int", "real", "date", "enum", "boolean", "container",
            "select", "update", "insert", "into", "delete", "from", "where",
            "firstonly", "index", "hint", "delete_from", "forupdate",
            "fieldNum", "tableNum", "enumNum",
            "window", "at", "print", "pause", "breakpoint",
            "#define", "#localmacro", "#endmacro"
        };

        private readonly string[] KnownSeparators = {
            " ", "\t", "\r", "\n", "\r\n", "=", ".", ":", ";", "+", "-", "*", "(", ")", "[", "]", "{", "}"  //, "/"
        };


        // ---------------------------- Data Types -----------------------------
        private enum axtParserKindOfPhrase
        {
            Nothing, Keyword, LongComment, ShortComment, StringValue, CharValue, IntValue, DoubleValue
        }


        // ---------------------------- Constructor ----------------------------
        public axtParserXpp(string _data)
        {
            m_Data = _data;
            m_CurrentPos = 0;
            m_Length = _data.Length;
            m_CurrentPhrase = axtParserKindOfPhrase.Nothing;
            m_Stack = string.Empty;
        }


        // ------------------------------ Methods ------------------------------
        public string generateRtf()
        {
            System.Windows.Forms.RichTextBox rtf = new System.Windows.Forms.RichTextBox();

            //-- Options - Colors
            Color cBlack = Color.Black;
            Color cLineNumber = Color.Gray;
            Color cKeyword = Color.Blue;
            Color cComment = Color.Green;
            Color cString = Color.Red;


            //-- Start
            rtf.Clear();
            string phrase = null;
            axtParserKindOfPhrase kind = axtParserKindOfPhrase.Nothing;
            string delimiter = null;

            while (nextPhrase(ref phrase, ref kind, ref delimiter))
            {
                switch (kind)
                {
                    case axtParserKindOfPhrase.Keyword:
                        rtf.SelectionColor = cKeyword;
                        break;

                    case axtParserKindOfPhrase.StringValue:
                    case axtParserKindOfPhrase.CharValue:
                        rtf.SelectionColor = cString;
                        break;

                    case axtParserKindOfPhrase.ShortComment:
                    case axtParserKindOfPhrase.LongComment:
                        rtf.SelectionColor = cComment;
                        break;

                    default:
                        rtf.SelectionColor = cBlack;
                        break;
                }
                rtf.SelectedText = phrase;
                rtf.SelectionColor = cBlack;
                rtf.SelectedText = delimiter;

                phrase = null;
                kind = axtParserKindOfPhrase.Nothing;
                delimiter = null;
            }

            rtf.SelectionStart = 0;
            rtf.SelectionLength = 0;
            rtf.Visible = true;

            return rtf.Rtf;
        }


        // ------------------------- Internal Methods --------------------------
        private string getNextChar(ref string _followingChar)
        {
            if (m_CurrentPos >= m_Length)
            {
                _followingChar = null;
                return null;
            }

            _followingChar = m_CurrentPos == m_Length - 1 ? null : m_Data[m_CurrentPos + 1].ToString();
            m_CurrentPos++;
            return m_Data[m_CurrentPos - 1].ToString();
        }

        private bool phraseTerminates(string _char, string _nextChar, ref string _terminator)
        {
            switch (m_CurrentPhrase)
            {
                case axtParserKindOfPhrase.LongComment:
                    if (_char == "*" && _nextChar == "/")
                    {
                        _terminator = "";
                        return true;
                    }
                    break;

                case axtParserKindOfPhrase.ShortComment:
                    if (_char == "\n" || (_char == "\r" && _nextChar == "\n"))
                    {
                        _terminator = Environment.NewLine;
                        return true;
                    }
                    break;

                case axtParserKindOfPhrase.StringValue:
                    if (_char == "\"")
                    {
                        _terminator = "";
                        return true;
                    }
                    break;

                case axtParserKindOfPhrase.CharValue:
                    if (_char == "'")
                    {
                        _terminator = "";
                        return true;
                    }
                    break;

                //case cofKindOfPhrase.IntValue:
                //    break;

                //case cofKindOfPhrase.DoubleValue:
                //    break;

            }

            return false;
        }

        private axtParserKindOfPhrase kindOfPhrase(string _char, string _nextChar)
        {
            if (_char == "/" && _nextChar == "*")
            {
                return axtParserKindOfPhrase.LongComment;
            }
            else if (_char == "/" && _nextChar == "/")
            {
                return axtParserKindOfPhrase.ShortComment;
            }
            else if (_char == "\"")
            {
                return axtParserKindOfPhrase.StringValue;
            }
            else if (_char == "'")
            {
                return axtParserKindOfPhrase.CharValue;
            }
            //else if (numbers...)
            //{
            //}
            else
            {
                return axtParserKindOfPhrase.Nothing;
            }
        }

        private bool isKeyword(string _phrase)
        {
            foreach (string s in KnownKeywords)
            {
                if (_phrase.ToLower().Equals(s.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isSeparator(string _phrase)
        {
            foreach (string s in KnownSeparators)
            {
                if (_phrase.Equals(s))
                {
                    return true;
                }
            }
            return false;
        }

        private bool nextPhrase(ref string _phrase, ref axtParserKindOfPhrase _kind, ref string _delimiter)
        {
            string theChar = null;
            string nextChar = null;
            string terminator = null;
            m_Stack = string.Empty;
            m_CurrentPhrase = axtParserKindOfPhrase.Nothing;

            while (true)
            {
                theChar = getNextChar(ref nextChar);
                if (theChar == null) return false;

                // Handle special char
                if (theChar == "\\")
                {
                    theChar += getNextChar(ref nextChar);
                }

                // Test for separator
                if (m_CurrentPhrase == axtParserKindOfPhrase.Nothing && isSeparator(theChar))
                {
                    _phrase = m_Stack;
                    if (_kind == axtParserKindOfPhrase.Nothing && isKeyword(_phrase))
                    {
                        _kind = axtParserKindOfPhrase.Keyword;
                    }
                    else
                    {
                        _kind = m_CurrentPhrase;
                    }
                    _delimiter = theChar == "\r" ? "" : theChar;
                    return true;
                }

                // Put char to stack
                m_Stack += theChar;

                // Will current char terminate action?
                if (phraseTerminates(theChar, nextChar, ref terminator))
                {
                    _phrase = m_Stack + terminator;
                    _kind = m_CurrentPhrase;
                    _delimiter = "";
                    return true;
                }

                // Handle special chars
                if (m_CurrentPhrase == axtParserKindOfPhrase.Nothing)
                {
                    axtParserKindOfPhrase tmpKind = kindOfPhrase(theChar, nextChar);
                    if (tmpKind != axtParserKindOfPhrase.Nothing)
                    {
                        m_CurrentPhrase = tmpKind;
                    }
                }
            }
        }

    }

}
