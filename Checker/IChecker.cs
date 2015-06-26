using System;
using System.Collections.Generic;
using System.Text;
using Cloud9.Parser.Html;
using System.IO;

namespace Cloud9.Checker
{
    public abstract class IChecker
    {

        public abstract bool Perform(CHtmlDocument doc);

        public abstract string GetCheckerName();

        public void AddReportItem(string url, string tag, string msg)
        {
            if (!Directory.Exists(@"c:\temp")) Directory.CreateDirectory(@"c:\temp");
            if (!Directory.Exists(@"c:\temp\log")) Directory.CreateDirectory(@"c:\temp\log");

            using (StreamWriter sw = new StreamWriter(@"c:\temp\log\"+GetCheckerName()+".log", true))
            {
                sw.Write("{0}\t{1}\t{2}\r\n", url, tag, msg);
                sw.Close();
            }
        }

    }
}
