using Ookii.Dialogs;
using SAOT.Model;
using System;
using System.Windows.Forms;


namespace SAOT.ConfigWindow
{

    /// <summary>
    /// 
    /// </summary>
    public partial class DatabaseSourceWindow : Form
    {


        public DatabaseSourceWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var progress = new ProgressDialog())
            {
                DoWorkEventHandler workHandler = (object s, DoWorkEventArgs workArgs) =>
                {
                    ListViewUpdatePending = true;
                    Thread.Sleep(100);
                    this.Invoke(new Action(() =>
                    {
                        //READ SROUCE FILES HERE

                        ListViewUpdatePending = false;
                    }));
                    while (ListViewUpdatePending)
                        Thread.Sleep(1);
                };

                RunWorkerCompletedEventHandler workCompleteHandler = (object s, RunWorkerCompletedEventArgs workArgs) =>
                {
                };

                progress.WindowTitle = "Working...";
                progress.Text = "Reading source files...";
                progress.DoWork += workHandler;
                progress.RunWorkerCompleted += workCompleteHandler;
                progress.ShowCancelButton = false;
                Owner.Invoke(new Action(() => progress.ShowDialog(this))); //this ensures the dialog is parented to this form
            }

        }

        private void buttonCreateUserDatabase(object sender, EventArgs e)
        {
            if (User.DatabaseExists)
            {
                using (var dialog = new TaskDialog())
                {
                    var okButton = new TaskDialogButton(ButtonType.Ok);
                    dialog.Buttons.Add(okButton);
                    dialog.MainInstruction = "A user account database already exists in the database source directory. Delete that file first before attempting to create a new one.";
                    dialog.ShowDialog(this);
                }
            }

            else
            {
                User.CreateNewDatabase();

                using (var dialog = new TaskDialog())
                {
                    var okButton = new TaskDialogButton(ButtonType.Ok);
                    dialog.Buttons.Add(okButton);
                    dialog.MainInstruction = "A new user database has been created with a default user.\n\nUsername: admin\nPassword: password";
                    dialog.ShowDialog(this);
                }
            }

        }

        private void buttonCreateWarehouseDB_Click(object sender, EventArgs e)
        {
            if (Warehouse.DatabaseExists)
            {
                using (var dialog = new TaskDialog())
                {
                    var okButton = new TaskDialogButton(ButtonType.Ok);
                    dialog.Buttons.Add(okButton);
                    dialog.MainInstruction = "A warehouse database already exists in the database source directory. Delete that file first before attempting to create a new one.";
                    dialog.ShowDialog(this);
                }
            }

            else
            {
                Warehouse.ConfirmDatabaseExists();

                using (var dialog = new TaskDialog())
                {
                    var okButton = new TaskDialogButton(ButtonType.Ok);
                    dialog.Buttons.Add(okButton);
                    dialog.MainInstruction = "A new warehouse database has been created with a default warehouse named \"WH\".";
                    dialog.ShowDialog(this);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
