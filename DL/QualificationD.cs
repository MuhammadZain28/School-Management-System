using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.BL;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.DL
{
    class QualificationD
    {
        public bool Add_qualifications(QualificationB qualificationBs, int id)
        {
            try
            {
                string query = $@"INSERT INTO teacher_qualifications (degree_name, year, institute, teacher_id) VALUES
                                    ('{qualificationBs.degree}', {qualificationBs.year}, '{qualificationBs.institute}', {id});";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
                return false;
            }
        }
    }
}
