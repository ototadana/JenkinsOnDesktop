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
            if (string.IsNullOrWhiteSpace(format))
            {
                return null;
            }

            object[] args = EditFormatAndGetArgs(ref format, this.Hashtable);
            try
            {
                return string.Format(format, args);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        private static object[] EditFormatAndGetArgs(ref string format, Hashtable hashtable)
        {
            if (hashtable == null)
            {
                return new object[0];
            }

            List<object> args = new List<object>(hashtable.Count);
            int index = 0;
            foreach (string key in hashtable.Keys)
            {
                Regex regex = new Regex("\\{" + key + "\\}", RegexOptions.IgnoreCase);
                if (regex.IsMatch(format))
                {
                    format = regex.Replace(format, "{" + index++ + "}");
                    args.Add(hashtable[key]);
                }
            }
            return args.ToArray();
        }
    }
}
