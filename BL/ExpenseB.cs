using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;

namespace LMS.BL
{
    class ExpenseB
    {
        public int PaymentID { get; set; }              
        public string PaymentType { get; set; }         
        public string Name { get; set; }                
        public string ToEntity { get; set; }            
        public string Contact { get; set; }             
        public DateTime PaymentDate { get; set; }       
        public Decimal Amount_paid { get; set; }            

        ExpenseD expense = new ExpenseD();
        public int Total()
        {
            return expense.TotalExpense();
        }
        public List<ExpenseB> ExpenseList ()
        {
            return expense.ExpenseList();
        }
        public bool add(ExpenseB expenseB)
        {
            return expense.addExpense(expenseB);
        }
        public ExpenseB GetExpense(int id)
        {
            return expense.ExpenseData(id);
        }
        public bool remove(int id)
        {
            return expense.deleteExpense(id);
        }
        public int filter(int month)
        {
            return expense.FilterTotalExpense(month);
        }
        public List<ExpenseB> FilterList(int month)
        {
            return expense.FilterExpenseList(month);
        }
    }
}
