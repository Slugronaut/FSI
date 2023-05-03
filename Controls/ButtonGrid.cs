using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SAOT.Model;
using System.Text.RegularExpressions;

namespace SAOT
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ButtonGrid : UserControl
    {

        //public static ControlPool<Button> PooledBinButtons = new ControlPool<Button>();
        public delegate void GridButtonEvent(Button sender, ButtonGrid group);

        public Label IdLabel { get => label1;}
        public ToolTip Tooltip { get => toolTip1; }
        public string GroupId { get => label1.Text; protected set => label1.Text = value; }
        public event GridButtonEvent ItemClicked;
        public int CellWidth { get; set; } = 55;
        public int CellHeight { get; set; } = 24;
        public int CellMarginX { get; set; } = 4;
        public int CellMarginY { get; set; } = 4;
        public int GroupInset { get; set; } = 18;
        public int Columns { get; protected set; }
        public int Rows { get; protected set; }
        protected Color ButtonColor;

        protected Form OwnerForm;

        const int Fudge = 2;
        /// <summary>
        /// Returns the required minimum height for containing all controls within this grid.
        /// </summary>
        public int RequiredHeight
        {
            get
            {
                int yOffset = label1.Size.Height + (int)(label1.Size.Height * 0.15);
                return yOffset + ((int)(Rows+Fudge) * CellHeight) + ((int)(Rows+Fudge) * CellMarginY);
            }
        }

        public int RequiredWidth
        {
            get
            {
                int xOffset = label1.Location.X + GroupInset;
                return xOffset +((int)(Columns+Fudge) * CellWidth) + ((int)(Columns+Fudge) * CellMarginX);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonGrid()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="buttonColor"></param>
        public ButtonGrid(string groupId)
        {
            InitializeComponent();
            this.Disposed += OnDisposed;
            GroupId = groupId;
        }

        /// <summary>
        /// 
        /// </summary>
        public ButtonGrid(string groupId, Color buttonColor)
        {
            InitializeComponent();
            this.Disposed += OnDisposed;
            GroupId = groupId;
            ButtonColor = buttonColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentControl"></param>
        /// <param name="aisles"></param>
        /// <param name="binFilter"></param>
        public static List<ButtonGrid> PropogateAisleView(Warehouse wh, Control parentControl, List<Aisle> aisles, Regex binFilter, bool resizeToFitContents, bool topToBottom, GridButtonEvent itemClicked)
        {
            List<ButtonGrid> grids = new List<ButtonGrid>();
            parentControl.SuspendLayout();

            foreach (var aisle in aisles)
            {
                foreach (var rack in aisle.Racks)
                {
                    var rows = rack.Levels.Count;
                    var columns = rack.MaxBins;
                    string groupId = aisle.Id + rack.Id;
                    var group = new ButtonGrid(groupId, Color.AliceBlue);
                    grids.Add(group);
                    group.Width = parentControl.Width;
                    group.Height = group.RequiredHeight;

                    foreach (var level in rack.Levels)
                    {
                        int row;
                        if(topToBottom)
                            row = (int)level.Id[0] - (int)'A';
                        else row = rows - ((int)level.Id[0] - (int)'A');

                        foreach (var bin in level.Bins)
                        {
                            int col = int.Parse(bin.Id) - 1;
                            string locId = level.Id + bin.Id;
                            string fullId = groupId + locId;
                            var loc = wh.FindStorageLocation(fullId);

                            var color = StorageSpace.TypeToColor(loc.Type);
                            if (loc.Items.Count < 1 && loc.Type == StorageSpace.LocationTypes.storage)
                                color = Color.LightGray;
                            
                            var button = group.AddItem(locId, fullId, (uint)col, (uint)row, color);
                            button.AccessibleName = fullId;
                        }
                    }

                    group.ItemClicked += itemClicked;
                    if (resizeToFitContents)
                    {
                        group.Width = group.RequiredWidth;
                        group.Height = group.RequiredHeight;
                    }
                }
            }

            parentControl.Controls.AddRange(grids.ToArray());

            for(int i = 0; i < grids.Count; i++)
            {
                grids[i].Location = new Point(0, i < 1 ? 0 : grids[i - 1].Location.Y + grids[i - 1].Height + 2);
            }

            parentControl.ResumeLayout(true);
            parentControl.PerformLayout();
            return grids;
        }

        public virtual Button AddItem(string itemId, string itemTooltip, uint column, uint row, Color color)
        {
            Columns = (int)Math.Max(this.Columns, column);
            Rows = (int)Math.Max(this.Rows, row);

            this.SuspendLayout();

            int yOffset = label1.Size.Height + (int)(label1.Size.Height * 0.15);
            int xOffset = label1.Location.X + GroupInset;
            var button = new Button
            {
                Text = itemId,
                Size = new Size(CellWidth, CellHeight),
                BackColor = color,
            };
            button.Location = new Point(
                                    xOffset + ((int)column*CellWidth) + ((int)column*CellMarginX), 
                                    yOffset + ((int)row*CellHeight)   + ((int)row*CellMarginY)
                                    );
            toolTip1.SetToolTip(button, itemTooltip);
            button.Click += OnItemClicked;

            this.Controls.Add(button);
            this.ResumeLayout();
            return button;
        }

        protected void OnParented(Object sender, EventArgs e)
        {
            OwnerForm = this.FindForm();
            OwnerForm.LostFocus += OnLostFocus;
        }

        protected virtual void OnDisposed(Object sender, EventArgs args)
        {
            //OwnerForm.LostFocus -= OnLostFocus;
        }

        protected void OnLostFocus(Object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        protected void OnItemClicked(Object sender, EventArgs args)
        {
            var button = sender as Button;
            if (button != null)
                ItemClicked?.Invoke(button, this);
        }


    }
}
