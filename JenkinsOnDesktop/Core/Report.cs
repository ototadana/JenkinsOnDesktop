using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XPFriend.JenkinsOnDesktop.Core
{
    public class Report
    {
        private Hashtable table;

        public Hashtable Hashtable
        {
            get
            {
                if (table == null)
                {
                    table = new Hashtable();
                }
                return table;
            }

            set
            {
                table = value;
            }
        }

        public object this[string name]
        {
            get { return Hashtable[name]; }
            set { Hashtable[name] = value; }
        }

        public bool IsUpdated
        {
            get
            {
                bool? value = (bool?)this["IsUpdated"];
                if (value == null)
                {
                    return false;
                }
                return (bool)value;
            }

            set
            {
                this["IsUpdated"] = value;
            }
        }

        public string SourceUrl
        {
            get
            {
                object value = this["SourceUrl"];
                if (value == null)
                {
                    return null;
                }
                return value.ToString();
            }

            set
            {
                this["SourceUrl"] = value;
            }
        }

        internal string Format(string format)
        {
            if (format == null || this.Hashtable == null || this.Hashtable.Count == 0)
            {
                return null;
            }

            List<object> args = new List<object>(this.Hashtable.Count);
            int index = 0;
            foreach (string key in this.Hashtable.Keys)
            {
                Regex regex = new Regex("\\{" + key + "\\}", RegexOptions.IgnoreCase);
                if (regex.IsMatch(format))
                {
                    format = regex.Replace(format, "{" + index++ + "}");
                    args.Add(this.Hashtable[key]);
                }
            }

            try
            {
                return string.Format(format, args.ToArray());
            }
            catch (FormatException)
            {
                return "";
            }
        }
    }
}
