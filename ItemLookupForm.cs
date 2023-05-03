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

namespace SAOT
{
    public partial class ItemLookupForm : Form
    {
        Project Proj;
        Warehouse Wh;

        public ItemLookupForm(Project proj, Warehouse wh)
        {
            InitializeComponent();
            Proj = proj;
            Wh = wh;
            dataGridView1.DragDrop += HandleDrop;
            dataGridView1.KeyDown += onKeyDown;
            dataGridView1.Rows.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public static List<string> SplitRawItemList(string raw)
        {
            return raw.Split(new[] { "\r" }, StringSplitOptions.None).Select(x => x.Trim()).ToList();

        }

        private void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string pasta = Clipboard.GetText();
                var idList = SplitRawItemList(pasta);

                foreach(var id in idList)
                {
                    if (string.IsNullOrEmpty(id))
                        continue;
                    var mat = Proj.GetMaterialFromUnknownId(id);
                    if(mat == null)
                    {
                        Dialog.Message($"No fucking clue what '{id}' is supposed to be, bro.");
                    }
                    else
                    {
                        var loc = Wh.FindMaterialLocations(mat.MatId);
                        //loc, sap, kelox, desc
                        dataGridView1.Rows.Add(loc == null ? "No Loc" : loc.LocationId, mat.MatId, mat.DocumentId, mat.Description);
                    }
                }
                
            }
        }

        private void HandleDrop(Object sender, DragEventArgs args)
        {
            //args.Data.GetData()
            //foreach(var s arg)
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
