using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastReport.Barcode;
using System.Xml.Linq;

namespace LMS.BL
{
    internal class Person
    {
        public int id { get; set; }
        public string name { get; set; }
        public string contact { get; set; }

        public Person() { }
        public Person(int id, string name, string contact)
        {
            this.id = id;
            this.name = name;
            this.contact = contact;
        }
    }
}
