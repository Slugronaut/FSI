using System;
using System.Windows.Forms;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LoginWindow : Form
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs args)
        {
            try
            {
                User.Login(this.UsernameTextbox.Text, this.PasswordTextbox.Text);
                this.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show(this, e.Message);
            }
        }
    }
}
