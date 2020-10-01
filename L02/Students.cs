using System;

namespace L02 
{
    public class Students 
    {
        public int id { get; set; }

        public string name { get; set; }

        public string faculty { get; set; }

        public Students () {}
        public Students (int id_ = -1, string name_ = "NONE", string faculty_ = "NONE") {
            id = id_;
            name = name_;
            faculty = faculty_;
        }
    }
}