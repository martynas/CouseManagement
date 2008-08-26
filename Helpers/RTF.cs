using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Atrendia.CourseManagement.Helpers
{
    public class RTF
    {
        private string text;

        public RTF(string text)
        {
            this.text = text;
        }

        public string ToText()
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            else
            {
                string txt = new Regex(@"\{\*?\\[^{}]+}|[{}]|\\\n?[A-Za-z]+\n?(?:-?\d+)?[ ]?").Replace(text, "");
                return txt.Trim().Replace("\n", "<br>");
            }
        }
    }
}