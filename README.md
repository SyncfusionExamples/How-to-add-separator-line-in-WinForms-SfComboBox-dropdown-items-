# How-to-add-separator-line-in-WinForms-SfComboBox-dropdown-items-
This example demonstrates how to add a separator line to a drop-down menu in a WinForms SfComboBox.
This requirement can be achieved by handling SfComboBox.DropDownListView.DrawItem and MouseMove events. for more details please refer [How to add seperator line in winforms SfComboBoxDropDown](https://www.syncfusion.com/kb/11498/how-to-add-separator-line-in-winforms-sfcombobox-dropdown-iems)

In the below code, need to set the CheckBoxStyle using reflection concepts to handle the state of checkbox at runtime, then draw the checkbox, item text and separator line manually. The MouseMove event handling is get the mouse hovering item index to highlight the dropdown item.

# C#

    private void DropDownListView_DrawItem(object sender, Syncfusion.WinForms.ListView.Events.DrawItemEventArgs e)
    {
        bool isEnableDraw = (this.sfComboBox1.ComboBoxMode == ComboBoxMode.MultiSelection && this.sfComboBox1.AllowSelectAll && this.sfComboBox1.DropDownListView.ShowCheckBoxes && e.ItemIndex == 0) ? false : (e.ItemData as Student).IsDraWSeparator;
        if (isEnableDraw)
        {
            e.Handled = true;
            var listView = sender as SfListView;
            var linearLayout = listView.GetType().GetProperty("LinearLayout", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(listView);
            var items = linearLayout.GetType().GetProperty("Items").GetValue(linearLayout) as List<ListViewItemInfo>;
            var checkBoxStyle = items[e.ItemIndex].GetType().GetProperty("CheckBoxStyle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            checkBoxStyle.SetValue(items[e.ItemIndex], listView.Style.CheckBoxStyle);
            //Fill the background color of mouse hover item.
            if (e.ItemIndex == mouseHoverItemIndex)
                e.Graphics.FillRectangle(new SolidBrush(listView.Style.SelectionStyle.HoverBackColor), e.Bounds);
            //Fill the background color of Selecteditem.
            if (sfComboBox1.SelectedIndex == e.ItemIndex)
                e.Graphics.FillRectangle(new SolidBrush(listView.Style.SelectionStyle.SelectionBackColor), new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 2));
            RectangleF bounds = new RectangleF(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height);
            //Draw the checkbox if MultiSelction and showcheckboxes are enabled.
            if (sfComboBox1.ComboBoxMode == ComboBoxMode.MultiSelection && this.sfComboBox1.DropDownListView.ShowCheckBoxes)
            {
                CheckBoxPainter.DrawCheckBox(e.Graphics, new Rectangle(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2), listView.CheckedItems.Contains(e.ItemData) ? CheckState.Checked : CheckState.Unchecked, listView.Style.CheckBoxStyle);
                bounds = new RectangleF(e.Bounds.X + 20, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);
            }
            //Draw the listview item text.
            e.Graphics.DrawString((e.ItemData as Student).StudentName, this.Font, new SolidBrush(Color.FromArgb(255, 51, 51, 51)), bounds);
            //Draw the separator line.
            e.Graphics.DrawLine(new Pen(Color.Gray, width: 1) { DashPattern = new[] { 2f, 2f } }, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        }
    }
    
    int mouseHoverItemIndex = -1;
    
    private void SfComboBox1_MouseMove(object sender, MouseEventArgs e)
    {
        var list = (sender as SfListView);
        //Get the mouse over item index using mouse location.
        mouseHoverItemIndex = (sender as SfListView).GetRowIndexAtPoint(e.Location);
        list.Invalidate();
    }

![Seperator to SfComboBox](SfComboBox/SfComboBox/Image/Add%20Seperator%20to%20ComboBox.png)
