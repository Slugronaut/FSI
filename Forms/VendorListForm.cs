using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SAOT
{
    public partial class VendorListForm : Form
    {
        User CurrentUser;
        List<Vendor> AllVendors = new List<Vendor>(6);

        public VendorListForm(User currentUser)
        {
            CurrentUser = currentUser;
            InitializeComponent();
            PopulateVendorList();
        }

        void PopulateVendorList()
        {
            this.VendorDataGrid.Rows.Clear();
            try
            {
                AllVendors = Vendor.RequestAllVendors();
                foreach (var vendor in AllVendors)
                {
                    this.VendorDataGrid.Rows.Add(DbAccess.Unsanitize(vendor.Name), vendor.Id.ToString(), vendor.TransferId.ToString(), vendor.InUSA, vendor.StreetAddress, vendor.City, vendor.State, vendor.Zip);
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void AddVendorButton_Click(object sender, EventArgs e)
        {
            var addVendorDialog = new AddVendorForm(CurrentUser);
            addVendorDialog.ShowDialog(this);
            PopulateVendorList();
        }

        private void EditVendorButton_Click(object sender, DataGridViewCellEventArgs args)
        {
            if (args.RowIndex < 0 || args.RowIndex > this.VendorDataGrid.Rows.Count)
                return;
            var row = this.VendorDataGrid.Rows[args.RowIndex];
            string id = row.Cells[0].Value as string;


            var modifyDialog = new EditVendorForm(CurrentUser, AllVendors.Where(v => v.Name == id).ToArray()[0]);
            modifyDialog.ShowDialog(this);
            PopulateVendorList();
        }
    }
}
