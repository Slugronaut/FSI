using System;
using System.Windows.Forms;

namespace SAOT
{
    public partial class CreateNewUserForm : Form
    {
        User CurrentUser;

        public CreateNewUserForm(User currentUser)
        {
            CurrentUser = currentUser;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs args)
        {
            var name = this.UsernameTextbox.Text;
            var pass = this.PasswordTextbox.Text;
            var email = this.EmailTextbox.Text;

            if(string.IsNullOrEmpty(name))
            {
                MessageBox.Show("You must specify a username.");
                return;
            }
            if(string.IsNullOrEmpty(pass))
            {
                MessageBox.Show(this, "You must specify a password.");
                return;
            }
            if(string.IsNullOrEmpty(email))
            {
                if (MessageBox.Show(this, "Are you sure you don't want to specify an email address with this user?", "Missing Email", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }

            if (MessageBox.Show(this, "Are you sure you want to create the new user '" + name + "'?", "Confirm New User", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var checks = AccessRightsCheckList.Items;
            uint rights = 0;
            for(int i = 0; i < checks.Count; i++)
            {
                if (AccessRightsCheckList.GetItemChecked(i)) rights |= (uint)(1 << i);
                else rights &= ((uint)(1 << i) ^ 0xffffffff);
            }

            try
            {
                User.Register(CurrentUser, name, pass, email, rights);
                MessageBox.Show(this, "The user '" + name + "' has been created successfully.");
                this.Close();
                return;
            }
            catch(Exception e)
            {
                MessageBox.Show(this, e.Message);
            }
        }
    }
}
