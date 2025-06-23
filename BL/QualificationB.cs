using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;

namespace LMS.BL
{
    class QualificationB
    {
        public string degree {  get; set; }
        public string institute { get; set; }
        public int year { get; set; }
        QualificationD qualification = new QualificationD();
        public bool AddData(List<QualificationB> qualificationBs, int id)
        {
            bool success = false;
            qualificationBs = RemoveDuplicates(qualificationBs);
            if (qualificationBs.Count > 0)
            {
                foreach (QualificationB b in qualificationBs)
                {
                    success = qualification.Add_qualifications(b, id);
                }
                return true;
            }
            else { return false; }
        }
        public List<QualificationB> RemoveDuplicates(List<QualificationB> qualificationBs)
        {
            var remove = new HashSet<string>();
            var result = new List<QualificationB>();

            foreach (var q in qualificationBs)
            {
                string key = $"{q.degree.ToLowerInvariant()}|{q.year}";

                if (remove.Add(key))
                {
                    result.Add(q);
                }
            }

            return result;
        }
    }
}
