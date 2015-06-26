using System;
using System.Collections.Generic;
using System.Text;

namespace Cloud9.Reporter
{
    public class Reporter
    {
        private static Reporter instance = null;
        public Reporter getInstance()
        {
            if(instance == null)
            {
                instance = new Reporter();
            }
            return instance;
        }
        
    }
}
