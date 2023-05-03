using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SAOT.Controls
{
    public partial class AdjustableButtonGrid : SAOT.ButtonGrid
    {
        List<Button> ExpansionButtonsX = new List<Button>();
        List<Button> ExpansionButtonsY = new List<Button>();

        /// <summary>
        /// 
        /// </summary>
        public AdjustableButtonGrid() : base()
        {
            InitializeComponent();

        }

        protected override void OnDisposed(object sender, EventArgs args)
        {
            base.OnDisposed(sender, args);
            ExpansionButtonsX.Clear();
            ExpansionButtonsY.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="backgroundColor"></param>
        /// <param name="buttonColor"></param>
        public AdjustableButtonGrid(string groupId)
        {
            InitializeComponent();
            this.Disposed += OnDisposed;
            GroupId = groupId;
        }

        /// <summary>
        /// 
        /// </summary>
        public AdjustableButtonGrid(string groupId, Color buttonColor)
        {
            InitializeComponent();
            this.Disposed += OnDisposed;
            GroupId = groupId;
            ButtonColor = buttonColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTooltip"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public override Button AddItem(string itemId, string itemTooltip, uint column, uint row, Color color)
        {
            this.SuspendLayout();
            int oldColCount = Columns;
            int oldRowCount = Rows;
            Columns = (int)Math.Max(this.Columns, column);
            Rows = (int)Math.Max(this.Rows, row);


            //if columns changed we need to add new column buttons as needed and shift all row buttons to the right
            if (oldColCount != Columns)
            {
                //out with the old
                foreach(var expButton in this.ExpansionButtonsX)
                {
                    this.Controls.Remove(expButton);
                    expButton.Dispose();
                }

                foreach (var expButton in this.ExpansionButtonsY)
                {
                    this.Controls.Remove(expButton);
                    expButton.Dispose();
                }

                //in with the new
                for (int i = 0; i < Columns; i++)
                {

                }
            }
            //if rows changed we need to add new row buttons as needed and shift all row buttons as needed
            if(oldRowCount != Rows)
            {

            }


            int yOffset = IdLabel.Size.Height + (int)(IdLabel.Size.Height * 0.15);
            int xOffset = IdLabel.Location.X + GroupInset;
            var button = new Button
            {
                Text = itemId,
                Size = new Size(CellWidth, CellHeight),
                BackColor = color,
            };
            button.Location = new Point(
                                    xOffset + ((int)column * CellWidth) + ((int)column * CellMarginX),
                                    yOffset + ((int)row * CellHeight) + ((int)row * CellMarginY)
                                    );
            Tooltip.SetToolTip(button, itemTooltip);
            button.Click += OnItemClicked;

            this.Controls.Add(button);
            this.ResumeLayout();
            return button;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemTooltip"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Button AddExpandButton(string itemId, string itemTooltip, uint column, uint row, Color color)
        {
            this.SuspendLayout();

            int yOffset = IdLabel.Size.Height + (int)(IdLabel.Size.Height * 0.15);
            int xOffset = IdLabel.Location.X + GroupInset;
            var button = new Button
            {
                Text = itemId,
                Size = new Size(CellWidth, CellHeight),
                BackColor = color,
            };
            button.Location = new Point(
                                    xOffset + ((int)column * CellWidth) + ((int)column * CellMarginX),
                                    yOffset + ((int)row * CellHeight) + ((int)row * CellMarginY)
                                    );
            Tooltip.SetToolTip(button, itemTooltip);
            button.Click += OnItemClicked;

            this.Controls.Add(button);
            this.ResumeLayout();
            return button;
        }
    }
}
