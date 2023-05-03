using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
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
            this.dataGridView1.ColumnWidthChanged += OnColumnWidthChanged;
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
                filter.TextChanged += HandleFilterTextChanged;
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

        void HandleFilterTextChanged(Object sender, EventArgs args)
        {
            //TODO: start timer and don't perform update until a brief time has passed since the last keypress
            var filter = sender as TextBox;
            if (!filter.ContainsFocus)
                return;

            DisposeTimer();
            Timer = new System.Threading.Timer(TimerElapsed, filter, FilterDelay, FilterDelay);

            
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
                filter.TextChanged -= HandleFilterTextChanged;
                this.Controls.Remove(filter);
                filter.Dispose();
            }
            Filters.Clear();
            
        }

        public void CreateColumns(params string[] headerNames)
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
                tb.TextChanged += HandleFilterTextChanged;
                Filters.Add(tb);
                this.Controls.Add(tb);
            }

            foreach (var name in headerNames)
            {
                var col = new DataGridViewColumn();
                col.SortMode = DataGridViewColumnSortMode.Automatic;
                col.HeaderText = name;
                col.CellTemplate = new DataGridViewTextBoxCell();
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


    }
}
