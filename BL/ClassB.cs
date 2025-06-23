using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;

namespace LMS.BL
{
    internal class ClassB
    {
        public int classId {  get; set; }
        public string name { get; set; }
        ClassD classes = new ClassD();

        public Dictionary<int,string> load()
        {
            return classes.loadComboBoxClass();
        }
        public List<ClassB> loadList()
        {
            Dictionary<int, string> keyValuePairs = classes.loadComboBoxClass(); 
            List<ClassB> list = new List<ClassB>();
            foreach (var item in keyValuePairs)
            {
                list.Add(new ClassB()
                {
                    classId = item.Key,
                    name = item.Value,
                });
            }
            return list;
        }
        public bool InsertClass(ClassB classB)
        {
            return classes.Insert(classB);
        }
        public bool deleteData(int id)
        {
            return classes.Delete(id);
        }
    }
}
