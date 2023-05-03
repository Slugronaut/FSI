using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using SAOT.Model;

namespace SAOT
{
    public partial class FilterableDataGridView : UserControl
    {
        public int FilterDelay { get; private set; } //time in milliseconds from the last time typing occured to the time the filters are applied
        public List<TextBox> Filters { get; private set; }
        public Dictionary<Object, Regex> Regexes = new Dictionary<Object, Regex>();
        public DataGridView GridView { get => dataGridView1; }
        int DefaultYLoc;
        System.Threading.Timer Timer = null;

        public FilterableDataGridView()
        {
            InitializeComponent();
            InitFilters();
            this.dataGridView1.MultiSelect = true;
            this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Bisque;
            dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.ColumnWidthChanged += OnColumnWidthChanged;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.CellMouseUp += dataGridView1_CellMouseUp;
        }

        /// <summary>
        /// Helper for initially creating filters based on preset design of this user control.
        /// </summary>
        void InitFilters()
        {
            if (!int.TryParse(Config.ReadConfigStr("FilterUpdateDelay"), out int delay))
                delay = 1500;
            FilterDelay = delay;

            Filters = new List<TextBox>(new TextBox[] { this.textBox1, this.textBox2, this.textBox3, this.textBox4 });
            foreach (var filter in Filters)
            {
                //filter.TextChanged += HandleFilterTextChanged;
                filter.KeyDown += HandleFilterTextChanged;
            }
            DefaultYLoc = Filters[0].Location.Y;
            UpdateRegexesWithFilters(Filters);
        }

        void DoFilter(TextBox filter)
        {
            //invoke so that it's performed on the UI thread
            this.Invoke(new Action(() =>
            {
                string text = filter.Text;
                Regexes[filter] = string.IsNullOrEmpty(text) ? null : new Regex(FilterUtil.WildCardToRegular(filter.Text, true), RegexOptions.IgnoreCase);
                var parent = this.Parent;
                MsgDispatch.PostMessage(new FilterUpdatedMessage(parent as Form));
            }));
        }

        /*
        void HandleFilterTextChanged(Object sender, EventArgs args)
        {
            //TODO: start timer and don't perform update until a brief time has passed since the last keypress
            var filter = sender as TextBox;
            if (!filter.ContainsFocus)
                return;

            DisposeTimer();
            Timer = new System.Threading.Timer(TimerElapsed, filter, FilterDelay, FilterDelay);

            
        }
        */
        void HandleFilterTextChanged(Object sender, KeyEventArgs args)
        {
            var tb = sender as TextBox;
            if (!tb.ContainsFocus)
                return;
            if (args.KeyCode == Keys.Enter)
            {
                //update ALL filters
                foreach(var filter in Filters)
                    DoFilter(filter);
                args.Handled = true;
                args.SuppressKeyPress = true;
            }
        }

        void TimerElapsed(Object obj)
        {
            DoFilter(obj as TextBox); //obj is the filter TextBox
            DisposeTimer();
        }

        void DisposeTimer()
        {
            if(Timer != null)
            {
                Timer.Dispose();
                Timer = null;
            }
        }

        void OnColumnWidthChanged(object sender, DataGridViewColumnEventArgs args)
        {
            for(int i = 0; i < Math.Min(Filters.Count, dataGridView1.ColumnCount); i++)
            {
                Filters[i].Width = dataGridView1.Columns[i].Width;

                int currLocX = (i == 0) ? 0 : Filters[i-1].Location.X + dataGridView1.Columns[i-1].Width;
                Filters[i].Location = new Point(currLocX, DefaultYLoc);
            }
        }

        void UpdateRegexesWithFilters(List<TextBox> filters)
        {
            Regexes.Clear();
            foreach (var filter in filters)
            {
                string text = filter.Text;
                Regexes.Add(filter, string.IsNullOrEmpty(text) ? null : new Regex(FilterUtil.WildCardToRegular(filter.Text, true)));
            }
        }

        public void ClearColumns()
        {
            this.dataGridView1.Columns.Clear();
            foreach(var filter in Filters)
            {
                //filter.TextChanged -= HandleFilterTextChanged;
                filter.KeyDown -= HandleFilterTextChanged;
                this.Controls.Remove(filter);
                filter.Dispose();
            }
            Filters.Clear();
            
        }

        public enum ColumnControlTypes
        {
            Text,
            Button,
        }

        public void CreateColumns(params string[] headerNames)
        {
            CreateColumns(ColumnControlTypes.Text, headerNames);
        }

        public void CreateColumns(ColumnControlTypes type, params string[] headerNames)
        {
            if(headerNames.Length < 1)
            {
                ClearColumns();
                return;
            }

            dataGridView1.AutoGenerateColumns = false;

            for(int i = 0; i < headerNames.Length; i++)
            {
                var tb = new TextBox();
                //tb.TextChanged += HandleFilterTextChanged;
                tb.KeyDown += HandleFilterTextChanged;
                Filters.Add(tb);
                this.Controls.Add(tb);
            }

            foreach (var name in headerNames)
            {
                var col = new DataGridViewColumn();
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.HeaderText = name;
                switch(type)
                {
                    case ColumnControlTypes.Text:
                        {
                            col.CellTemplate = new DataGridViewTextBoxCell();
                            break;
                        }
                    case ColumnControlTypes.Button:
                        {
                            col.CellTemplate = new DataGridViewButtonCell();
                            break;
                        }
                }
                
                this.dataGridView1.Columns.Add(col);
            }

            UpdateRegexesWithFilters(Filters);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="useFilters"></param>
        public void PushModelToView(List<List<string>> rows, bool useFilters)
        {
            GridView.Rows.Clear();
            foreach(var row in rows)
            {
                if (row.Count != Filters.Count)
                {
                    Dialog.Message("The data fed to this grid view has more cells than can be displayed.");
                    return;
                }

                if (useFilters)
                {
                    bool mismatched = false;
                    for (int i = 0; i < row.Count; i++)
                    {
                        if (row[i] == null) continue; //skip null values, this allows for non-string inputs for other controls
                        var cell = row[i];
                        var filter = Filters[i];
                        var reg = Regexes[(TextBox)(filter)];
                        if (cell != null)
                        {
                            if (reg != null && !reg.IsMatch(cell))
                            {
                                mismatched = true;
                                break;
                            }
                        }
                        else if(!string.IsNullOrEmpty(filter.Text))
                        {
                            mismatched = true;
                            break;
                        }
                    }

                    if (!mismatched)
                        GridView.Rows.Add(row.ToArray());
                }
                else GridView.Rows.Add(row.ToArray());
            }
        }

        #region Track Cell Selection
        int CurrentColIndex;
        int CurrentRowIndex;
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.Button == MouseButtons.Left) return;

            CurrentRowIndex = e.RowIndex;
            CurrentColIndex = e.ColumnIndex;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[CurrentRowIndex].Cells[CurrentColIndex].Selected = true;
        }

        DataGridViewCell SelectedCell()
        {
            return dataGridView1.SelectedCells[0];
            //return dataGridView1.Rows[CurrentRowIndex].Cells[CurrentColIndex]
        }

        DataGridViewRow SelectedRow()
        {
            return dataGridView1.SelectedRows[0];
        }

        Material SelectedMaterial(Project proj, string headerId)
        {
            var row = SelectedRow();
            int headerIndex = WindowUtils.GetHeaderIndex(dataGridView1, headerId);
            var matIdCell = row.Cells[headerIndex];
            return proj.FindMaterial(matIdCell.Value as string);
        }
        #endregion

        private void printLableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*
             * grid.Columns[colIndex].HeaderText.ToUpper();
             * */
            int colIndex = -1;
            var row = dataGridView1.Rows[CurrentRowIndex];
            for(int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                var colName = dataGridView1.Columns[i].HeaderText.ToUpper();
                if(colName == "SAP" || colName == "ID" || colName == "MATERIAL")
                {
                    colIndex = i;
                    break;
                }
            }

            if(colIndex < 0)
            {
                Dialog.Message("Could not identify the item clicked on.");
                return;
            }

            var cell = row.Cells[colIndex];

            //TODO: need to get parentform's Project provider interface!!
            var form = this.FindForm() as IProjectProvider;
            var mat = form.CurrentSelectedProject.FindMaterial(cell.Value.ToString());
            if(mat == null)
            {
                Dialog.Message($"Could not identify the material '{cell.Value.ToString()}'.");
                return;
            }
            WindowUtils.PrintMaterialLabel(mat);
        }
    }
}
