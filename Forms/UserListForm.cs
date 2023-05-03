using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAOT
{
    public partial class UserListForm : Form
    {
        User CurrentUser;
        List<User> AllUsers;

        public UserListForm(User currentUser)
        {
            CurrentUser = currentUser;
            InitializeComponent();
            PopulateUserList();
        }

        void PopulateUserList()
        {
            this.UserDataGrid.Rows.Clear();

            try
            {
                AllUsers = User.RequestAllUsers();
                foreach (var user in AllUsers)
                {
                    this.UserDataGrid.Rows.Add(DbAccess.Unsanitize(user.Name), DbAccess.Unsanitize(user.Email), user.RightsFlags);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AddUserButton_Click(object sender, System.EventArgs args)
        {
            var createUserDialog = new CreateNewUserForm(CurrentUser);
            createUserDialog.ShowDialog(this);
            PopulateUserList();

        }

        private void UserDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs args)
        {
            if (args.RowIndex < 0 || args.RowIndex > this.UserDataGrid.Rows.Count)
                return;
            var row = this.UserDataGrid.Rows[args.RowIndex];
            string id = row.Cells[0].Value as string;


            var modifyDialog = new ModUserForm(CurrentUser, AllUsers.Where(u => u.Name == id).ToArray()[0]);
            modifyDialog.ShowDialog(this);
            PopulateUserList();
        }
    }
}
