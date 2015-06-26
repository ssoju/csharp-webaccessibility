using System;
using System.Collections.Generic;
using System.Text;
using Cloud9.Threading;
using Cloud9.Parser.Html;
using System.Collections;
using Checker.Checkers;
using System.IO;

namespace Cloud9.Checker
{
    public class RoleChecker : WorkItem
    {

        private ArrayList checkerList = new ArrayList();
        private static RoleChecker instance = null;
        public static RoleChecker getInstance()
        {
            if (instance == null)
            {
                instance = new RoleChecker();
                // config 파일에서 읽어와서 add
                instance.checkerList.Add(new ImageChecker());
            }

            return instance;
        }


        public override void Perform()
        {
            //Start();
        }

        public void Start(CHtmlDocument doc)
        {
            string[] files = Directory.GetFiles(@"c:\temp\log\");
            foreach(string file in files)
            {
                File.Delete(file);
            }

            foreach(IChecker checker in checkerList)
            {
                checker.Perform(doc);
            }
        }

    }
}