using System;
using System.Windows.Forms;

namespace SAOT
{
    public partial class EditVendorForm : Form
    {
        User CurrentUser;

        public EditVendorForm(User currentUser, Vendor moddedVendor)
        {
            CurrentUser = currentUser;
            InitializeComponent();
            PopulateControls(moddedVendor);
        }

        void PopulateControls(Vendor vendor)
        {
            this.NameTextbox.Text = vendor.Name;
            this.IdTextbox.Text = vendor.Id.ToString();
            this.TransferIdTextbox.Text = vendor.TransferId.ToString();
            this.StreetTextbox.Text = vendor.StreetAddress;
            this.CityTextbox.Text = vendor.City;
            this.StateTextbox.Text = vendor.State;
            this.ZipTextbox.Text = vendor.Zip;
            this.InUSCheckbox.Checked = vendor.InUSA;
        }

        private void OkButton_Click(object sender, EventArgs args)
        {
            if (MessageBox.Show(this, "Are you sure you want to update the vendor '" + NameTextbox.Text + "'?", "Edit Vendor Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            try
            {
                if (!int.TryParse(IdTextbox.Text, out int id))
                    throw new Exception("Vendor Id must be an integer.");

                if (!int.TryParse(TransferIdTextbox.Text, out int transferId))
                    throw new Exception("Vendor Transfer Id must be an integer.");

                Vendor.UpdateVendor(CurrentUser,
                    NameTextbox.Text,
                    id,
                    transferId,
                    StreetTextbox.Text,
                    CityTextbox.Text,
                    StateTextbox.Text,
                    ZipTextbox.Text,
                    InUSCheckbox.Checked
                    );
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message);
                return;
            }

            MessageBox.Show(this, "The vendor '" + NameTextbox.Text + "' has been updated.");
            this.Close();
        }
    }
}
