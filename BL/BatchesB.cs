using System;
using System.Collections.Generic;
using System.Windows;
using LMS.DL;
using MySql.Data.MySqlClient;
using WinFormsApp1;
using MessageBox = System.Windows.MessageBox;

namespace LMS.BL
{
    public class BatchesB
    {
        public int branchId {  get; set; }
        public string BranchName { get; set; }

        BatchesD batchesD = new BatchesD();
        public bool addBatch(string branch)
        {
            if (string.IsNullOrWhiteSpace(branch))
            {
                MessageBox.Show("No Field can be empty or Fee less tha Rs.1500.");
                return false;
            }

            return new LMS.DL.BatchesD().addBatch(branch);
        }

        public Dictionary<int, string> loadBatch()
        {
            Dictionary<int, string> batchesB = batchesD.loadComboBoxBatch();
            batchesB.Add(-1, "---Select---");
            return batchesB;
        }
    }
}
