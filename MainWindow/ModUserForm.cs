using Ookii.Dialogs;
using System;
using System.Windows.Forms;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ModUserForm : Form
    {
        User CurrentUser;
        User ModdedUser;

        public ModUserForm(User currentUser, User userToMod)
        {
            CurrentUser = currentUser;
            ModdedUser = userToMod;
            InitializeComponent();
            PopulateControls(userToMod);
        }

        void PopulateControls(User user)
        {
            this.UsernameTextbox.Text = DbAccess.Unsanitize(user.Id);
            this.EmailTextbox.Text = DbAccess.Unsanitize(user.Email);

            var checks = this.AccessRightsCheckList.Items;
            uint rights = user.RightsFlags;
            for (int i = 0; i < checks.Count; i++)
                this.AccessRightsCheckList.SetItemChecked(i, (rights & (uint)(1 << i)) != 0);
        }

        private void ChangePasswordButton_Click(object sender, EventArgs args)
        {
            using (var dialog = new TaskDialog())
            {
                var yesButton = new TaskDialogButton(ButtonType.Yes);
                var noButton = new TaskDialogButton(ButtonType.No);
                dialog.Buttons.Add(yesButton);
                dialog.Buttons.Add(noButton);
                dialog.MainInstruction = "Do you want to change the user's password to '"+this.PasswordTextbox.Text+"'?";
                if (dialog.ShowDialog(this) == yesButton)
                {
                    try
                    {
                        User.UpdatePassword(CurrentUser, ModdedUser.Id, this.PasswordTextbox.Text);
                        MessageBox.Show(this, "The user '" + ModdedUser.Id + "' has had their password successfully updated.");
                    }
                    catch(Exception e)
                    {
                        MessageBox.Show(this, e.Message);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs args)
        {
            if (string.IsNullOrEmpty(this.EmailTextbox.Text))
            {
                if (MessageBox.Show(this, "Are you sure you don't want to specify an email address with this user?", "Missing Email", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            if (MessageBox.Show(this, "Are you sure?", "Confirm Changes to User", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            var checks = AccessRightsCheckList.Items;
            uint rights = 0;
            for (int i = 0; i < checks.Count; i++)
            {
                if (AccessRightsCheckList.GetItemChecked(i)) rights |= (uint)(1 << i);
                else rights &= ((uint)(1 << i) ^ 0xffffffff);
            }

            try
            {
                User.ApplyUserSettings(CurrentUser, ModdedUser.Id, DbAccess.Sanitize(this.EmailTextbox.Text), rights);
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
                return;
            }

            MessageBox.Show(this, "The user '" + ModdedUser.Id + "' was successfully updated.");
            ModdedUser = null;
            this.Close();
        }
    }
}
