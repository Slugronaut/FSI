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
            this.PasswordTextbox.KeyDown += OnKeyDown;
            this.UsernameTextbox.KeyDown += OnKeyDown;
        }

        void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                Login();
            }
        }

        private void LoginButton_Click(object sender, EventArgs args)
        {
            Login();
        }

        void Login()
        {
            try
            {
                User.Login(this.UsernameTextbox.Text, this.PasswordTextbox.Text);
                this.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
            }
        }
    }
}
