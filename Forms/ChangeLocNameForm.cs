using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT.Forms
{
    public partial class ChangeLocNameForm : Form
    {
        WeakReference CurrentUser;
        readonly Warehouse Wh;
        Project Proj;
        readonly StorageSpace LocStorage;

        public ChangeLocNameForm(User currentUser, Warehouse warehouse, Project proj, string locName)
        {
            CurrentUser = new WeakReference(currentUser);
            Wh = warehouse;
            Proj = proj;
            InitializeComponent();

            this.textBoxOldLoc.Text = locName;
            LocStorage = warehouse.FindStorageLocation(locName);
            if(LocStorage == null)
            {
                MessageBox.Show(this, $"The location '{locName}' could not be found in the warehouse '{warehouse.WarehouseId}'.");
                this.Close();
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!User.IsUserLoggedIn(CurrentUser) || !(CurrentUser.Target as User).CanManageLocations)
            {
                MessageBox.Show(this, "You do not currently have authorization to do that.");
                this.Close();
                return;
            }

            
            //TODO: validate stuff was typed not stupid
            //Location


            //confirm user's action
            var newLocFullId = textBoxAisle.Text + textBoxRack.Text + textBoxLevel.Text + textBoxBin.Text; 
            if (MessageBox.Show(this, $"Are you sure you want to rename the location '{LocStorage.LocationId}' to '{newLocFullId}'?\nDoing so will automatically move any contents into the new location.", "Rename Confirmation", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }
 

            //validate that we don't already have a location with the given name
             var existingLoc = Wh.FindStorageLocation(newLocFullId);
            if(existingLoc != null)
            {
                MessageBox.Show(this, $"There is already a location in the system with the id '{newLocFullId}'.", "Location Already Exists");
                return;
            }

            //TODO: update every single item's location in the system as well as the location itself.
            //Note: this transaction should rollback if either part fails!!
            WindowUtils.ProgressWrapper(this, "Doin' stuff n things....", () =>
            {
                Thread.Sleep(5);
            });
        }
    }
}
