using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace SAOT.Forms
{

    public partial class SelectLabelPrinterForm : Form
    {
        public const string LabelHorOffset = "LabelPrinterHorOffset";

        public SelectLabelPrinterForm()
        {
            this.InitializeComponent();

            List<string> names = new List<string>();
            foreach (var printer in PrinterSettings.InstalledPrinters)
            {
                this.comboPrinterNames.Items.Add(printer);
                names.Add(printer as string);
            }

            var temp = Config.ReadConfigStr(LabelHorOffset);
            decimal horOffset = temp == null ? 0 : decimal.Parse(temp);
            this.numericUpDown1.Value = horOffset;


            var index = names.IndexOf(Config.ReadConfigStr("LabelPrinter"));
            if (index >= 0)
                this.comboPrinterNames.SelectedIndex = index;

            this.FormClosed += OnClose;
        }

        private void comboPrinterNames_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var printerName = this.comboPrinterNames.Text;
            Config.WriteConfigStr("LabelPrinter", printerName);
            Config.SaveConfig();
        }

        void OnClose(object sender, System.EventArgs e)
        {
            Config.WriteConfigStr(LabelHorOffset, this.numericUpDown1.Value.ToString());
            Config.SaveConfig();
        }
    }
}
