using SAOT.Model;
using System;
using System.Windows.Forms;


namespace SAOT.Forms
{
    public partial class CreateMaterialForm : Form
    {
        Project Proj;
        WeakReference User;

        public CreateMaterialForm(Project proj, User currentUser)
        {
            Proj = proj;
            User = new WeakReference(currentUser);

            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                var mat = CreateMaterial();
                if(mat == null)
                {
                    Dialog.Message("Failed to create the specific material.");
                    return;
                }

                var user = User.Target as User;
                if(!User.IsAlive || user == null || (!user.IsAdmin && !user.CanManageMaterials))
                {
                    Dialog.Message("Insufficient access rights. Cannot create the material.");
                    return;
                }
                try
                {
                    MaterialsDb.AddMaterial(user, mat);
                    Proj.UpdateMaterialsList();
                    Dialog.Message($"Material {mat.MatId} has been successfully created.");
                    Proj = null;
                    Close();
                    return;
                }
                catch(Exception ex)
                {
                    Dialog.Message(ex.ToString());
                }
            }
        }

        bool ValidateForm()
        {
            if (string.IsNullOrEmpty(textBoxDocId.Text)) return false;
            if (string.IsNullOrEmpty(textBoxSapId.Text)) return false;
            if(string.IsNullOrEmpty(textBoxDesc.Text)) return false;
            if (string.IsNullOrEmpty(textBoxUoM.Text)) return false;
            if (string.IsNullOrEmpty(textBoxGroupId.Text)) return false;
            return true;
        }

        Material CreateMaterial()
        {
            var mat = new Material(textBoxSapId.Text,
                textBoxDocId.Text,
                textBoxDesc.Text,
                textBoxUoM.Text,
                textBoxGroupId.Text,
                textBoxCustomerId.Text,
                null
                );

            return mat;
        }
    }
}
