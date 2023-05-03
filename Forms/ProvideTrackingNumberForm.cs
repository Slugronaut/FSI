using SAOT.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAOT.Forms
{
    public partial class ProvideTrackingNumberForm : Form
    {
        int OrderId;
        int CarrierId;
        WeakReference CurrentUser;
        List<Carrier> Carriers;

        int ComboIndexFromCarrierName(ComboBox combo, string name)
        {
            if (string.IsNullOrEmpty(name)) return -1;

            for(int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.Items[i] as string == name)
                    return i;
            }

            return -1;
        }

        Carrier CarrierFromId(int id)
        {
            foreach(var carrier in Carriers)
            {
                if (carrier.Id == id)
                    return carrier;
            }

            return null;
        }

        Carrier CarrierFromName(string name)
        {
            foreach(var carrier in Carriers)
            {
                if (carrier.Name == name)
                    return carrier;
            }

            return null;
        }

        public ProvideTrackingNumberForm(User currentUser, int carrierId, string tracking, int orderId)
        {
            OrderId = orderId;
            CarrierId = carrierId;
            CurrentUser = new WeakReference(currentUser);

            InitializeComponent();
            this.textBoxTracking.Text = tracking;

            Carriers = Carrier.RequestAllCarriers();
            var car = CarrierFromId(carrierId);
            int selectedIndex = -1;
            int i = 0;
            foreach (var carrier in Carriers)
            {
                comboCarrier.Items.Add(carrier.Name);
                if (car != null && car.Name == carrier.Name)
                    selectedIndex = i;
                i++;
            }

            if(selectedIndex >= 0)
                comboCarrier.SelectedIndex = selectedIndex;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var user = CurrentUser.Target as User;
            if(!CurrentUser.IsAlive || user == null)
            {
                MessageBox.Show("This action cannot be performed whie not logged on.");
                this.Close();
                return;
            }

            var car = CarrierFromName(comboCarrier.Text);
            string carId = car == null ? string.Empty : car.Id.ToString();
            PickOrdersDatabase.SetTracking(user, OrderId, car.Id, this.textBoxTracking.Text);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
