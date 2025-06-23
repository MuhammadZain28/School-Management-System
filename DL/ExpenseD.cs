using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.BL;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using WinFormsApp1;

namespace LMS.DL
{
    class ExpenseD
    {
        public int TotalExpense()
        {
            string query = $"Select sum(amount_paid) from payments;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int sum = 0;
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    sum = reader.GetInt32(0);
                }
                else
                {
                    sum = 0;
                }
            }
            reader.Close();
            return sum;
        }
        public List<ExpenseB> ExpenseList()
        {
            List<ExpenseB> list = new List<ExpenseB>();
            string query = $"SELECT  PaymentID, PaymentType, Name, ToEntity, Contact, PaymentDate, Amount_paid FROM payments;";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                list.Add(new ExpenseB 
                {
                    PaymentID = reader.GetInt32(0),
                    PaymentType = reader.GetString(1),
                    Name = reader.GetString(2),
                    ToEntity = reader.GetString(3),
                    Contact = reader.GetString(4),
                    PaymentDate = reader.GetDateTime(5),
                    Amount_paid = reader.GetDecimal(6),
                });
            }
            reader.Close ();
            return list;
        }
        public int FilterTotalExpense(int month)
        {
            string query = $"Select sum(amount_paid) from payments where month(paymentdate) = {month};";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            int sum = 0;
            if (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    sum = reader.GetInt32(0);
                }
                else
                {
                    sum = 0;
                }
            }
            reader.Close();
            return sum;
        }
        public List<ExpenseB> FilterExpenseList(int month)
        {
            List<ExpenseB> list = new List<ExpenseB>();
            string query = $"SELECT  PaymentID, PaymentType, Name, ToEntity, Contact, PaymentDate, Amount_paid FROM payments  where month(paymentdate) = {month};";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            while (reader.Read())
            {
                list.Add(new ExpenseB
                {
                    PaymentID = reader.GetInt32(0),
                    PaymentType = reader.GetString(1),
                    Name = reader.GetString(2),
                    ToEntity = reader.GetString(3),
                    Contact = reader.GetString(4),
                    PaymentDate = reader.GetDateTime(5),
                    Amount_paid = reader.GetDecimal(6),
                });
            }
            reader.Close();
            return list;
        }
        public ExpenseB ExpenseData(int id)
        {
            ExpenseB list = new ExpenseB();
            string query = $"SELECT  PaymentID, PaymentType, Name, ToEntity, Contact, PaymentDate, Amount_paid FROM payments WHERE Paymentid = {id};";
            SqliteDataReader reader = DatabaseHelper.Instance.getData(query);
            if (reader.Read())
            {
                list = new ExpenseB
                {
                    PaymentID = reader.GetInt32(0),
                    PaymentType = reader.GetString(1),
                    Name = reader.GetString(2),
                    ToEntity = reader.GetString(3),
                    Contact = reader.GetString(4),
                    PaymentDate = reader.GetDateTime(5),
                    Amount_paid = reader.GetDecimal(6),
                };
            }
            reader.Close();
            return list;
        }
        public bool addExpense(ExpenseB payment)
        {
            try
            {
                string query = $"INSERT INTO payments (PaymentType, Name, ToEntity, Contact, PaymentDate, Amount_paid) VALUES ('{payment.PaymentType}', '{payment.Name}', '{payment.ToEntity}', '{payment.Contact}', '{payment.PaymentDate:yyyy-MM-dd}', {payment.Amount_paid});";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool deleteExpense(int id)
        {
            try
            {
                string query = $"delete from payments where paymentid = {id};";
                DatabaseHelper.Instance.Update(query);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
