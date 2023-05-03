using System;
using System.Windows.Forms;

namespace SAOT
{
    public partial class AddVendorForm : Form
    {
        User CurrentUser;

        public AddVendorForm(User currentUser)
        {
            CurrentUser = currentUser;
            InitializeComponent();
            InUSCheckbox.Checked = true;
        }

        private void OkButton_Click(object sender, EventArgs args)
        {
            try
            {
                if (!int.TryParse(IdTextbox.Text, out int id))
                    throw new Exception("Vendor Id must be an integer.");

                Vendor.AddVendor(CurrentUser,
                    NameTextbox.Text,
                    id,
                    StreetTextbox.Text,
                    CityTextbox.Text,
                    StateTextbox.Text,
                    ZipTextbox.Text,
                    InUSCheckbox.Checked
                    );

            }
            catch(Exception e)
            {
                MessageBox.Show(this, e.Message);
                return;
            }

            MessageBox.Show(this, "The vendor '" + NameTextbox.Text + "' has been added to the database.");
            this.Close();
        }
    }
}
