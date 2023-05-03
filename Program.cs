using System;
using System.Windows.Forms;

namespace SAOT
{
    static class Program
    {
        static MainWindow MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MsgDispatch.AddListener<FatalErrorMessage>(HandleFatalError);
            Config.LoadConfig();
            //Config.WriteConfigStr("LabelPrinter", "ZDesigner ZD410-300dpi ZPL");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm = new MainWindow();
            MainForm.MenuMode = MainWindow.MenuModes.Unconfigured;
            MainForm.FormClosed += OnAppClose;

            if (Config.IsValidConfig)
                MainForm.MenuMode = MainWindow.MenuModes.ReadyForLogin;
            MainForm.AppBegin();
            Application.Run(MainForm);
        }

        static void OnAppClose(object sender, FormClosedEventArgs args)
        {
            Config.SaveConfig();

        }

        static void HandleFatalError(FatalErrorMessage msg)
        {
            MessageBox.Show(MainForm, "An unrecoverable error has occured and this application must now exit.\n\n" + msg.Details);
            Application.Exit();
            Environment.Exit(0);
        }
    }

    
}
