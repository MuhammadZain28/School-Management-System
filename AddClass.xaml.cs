﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Google.Protobuf.Compiler;
using LMS.BL;
using LMS.DL;
using MessageBox = System.Windows.MessageBox;

namespace LMS
{
    /// <summary>
    /// Interaction logic for AddClass.xaml
    /// </summary>
    public partial class AddClass : Page
    {
        public bool IsSaved = false;
        public AddClass()
        {
            InitializeComponent();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBoxResult.No;
            if (!IsSaved)
            {
                result = MessageBox.Show("Want to save data", "Unsaved Work", MessageBoxButton.YesNo);
            }
            if (result == MessageBoxResult.Yes)
            {
                AddData();
            }
            MainFrame.Navigate(new Class_course());
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            AddData();
        }
        private void AddData()
        {
            if (Name.Text != null)
            {
                ClassB classB = new ClassB();
                classB.name = Name.Text;
                if (classB.InsertClass(classB))
                {
                    MessageBox.Show("Success");
                    IsSaved = true;
                }
                else
                {
                    MessageBox.Show("Unsuccessful");
                }
            }
            else
            {
                MessageBox.Show("Unsuccessful");
            }

        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
