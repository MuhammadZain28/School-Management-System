using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LMS.DL;

namespace LMS.BL
{
    class FeeB
    {
        public int FeeId { get; set; }
        public string roll { get; set; }
        public int stdId { get; set; }
        public string Name { get; set; }
        public int AmountPaid { get; set; }
        public bool Status { get; set; }  
        public decimal DueAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int month { get; set; } 

        FeeD fees = new FeeD();
        public int Total()
        {
            if ((DateTime.Now.Day == 1) && (fees.Logged(DateTime.Now)))
                fees.startOfMonth();
            return fees.TotalFee();
        }
        public List<FeeB> getFees()
        {
            return fees.getFee();
        }
        public List<FeeB> getGraph()
        {
            return fees.graph();
        }

        public bool AddData(FeeB feeB)
        {
            int id = fees.stdId(feeB.roll);
            if (id > 0)
            {
                if ((int)(feeB.AmountPaid) > 0)
                    return fees.AddRecord(feeB, id, 1);
                else
                    return fees.AddRecord(feeB, id, 0);
            }
            else
            {
                return false;
            }
        }

        public bool deleteData(FeeB feeB)
        {
            int id = feeB.stdId;
            if (id > 0)
            {
                return fees.AddRecord(feeB, id, 0);
                
            }
            else
            {
                return false;
            }
        }
        public FeeB search(int id)
        {
            return fees.searchFee(id);
        }
        public List<FeeB> filter(int month)
        {
            return fees.filter(month);
        }
        public int filterFee(int month)
        {
            return fees.FilterTotalFee(month);
        }
        public DataTable GetDataTable(int month)
        {
            return fees.GetDataTable(month);
        }
        public DataTable GetDataTableMonth(int month)
        {
            return fees.GetDataTableMonTH(month);
        }

    }
}
