using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.DL;
using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace LMS.BL
{
    class UserB
    {
        public int id {  get; set; }
        public string username {  get; set; }
        public string password { get; set; }
        public string role { get; set; }

        public UserB(string username, string password, string role = "coordinator")
        {
            this.username = username;
            this.password = password;
            this.role = role;
        }
        public UserB() { }
        
        public UserB getRole(string username, string password)
        {
            return UserD.GetRole(username, password);
        }
        public bool signInFunc()
        {
            
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Username and Password cannot be empty or Hides the Password.");
                return false;
            }

            bool flag = UserD.signIn(username, password);
            MessageBox.Show(flag ? "Login Successful." : "Invalid Username or Password.");
            return flag;
        }

        public bool AddUser()
        {
            
            if (string.IsNullOrWhiteSpace(username) || !username.Contains("@") || !username.Contains(".") || username.StartsWith("@"))
            {
                MessageBox.Show("Invalid email format.");
                return false;
            }

            bool hasUpper = false, hasLower = false, hasDigit = false;

            foreach (char ch in password)
            {
                if (char.IsUpper(ch)) hasUpper = true;
                else if (char.IsLower(ch)) hasLower = true;
                else if (char.IsDigit(ch)) hasDigit = true;
            }

            if (!(hasUpper && hasLower && hasDigit) || password.Length < 4)
            {
                MessageBox.Show("Password must be at least 4 characters long and contain uppercase, lowercase and digit.");
                return false;
            }

            bool success = UserD.AddUser(username, password, role);
            return success;
        }
    }
}
