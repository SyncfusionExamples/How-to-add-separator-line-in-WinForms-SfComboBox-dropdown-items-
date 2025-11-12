# How to Add Separator Line in WinForms SfComboBox Dropdown Items
## Overview
This example demonstrates how to add a separator line to the dropdown items in a WinForms SfComboBox using Syncfusion controls. This customization is useful for visually grouping items or improving readability in complex dropdown lists.

## Implementation Details
The requirement can be achieved by handling:
- SfComboBox.DropDownListView.DrawItem event (to draw custom UI elements)
- MouseMove event (to highlight hovered items)

### Key Steps:
- Use reflection to set CheckBoxStyle dynamically.
- Draw the checkbox, item text, and separator line manually.
- Handle hover and selection states for better UX.

``` C#
private void DropDownListView_DrawItem(object sender, Syncfusion.WinForms.ListView.Events.DrawItemEventArgs e)
{
    bool isEnableDraw = (sfComboBox1.ComboBoxMode == ComboBoxMode.MultiSelection &&
                         sfComboBox1.AllowSelectAll &&
                         sfComboBox1.DropDownListView.ShowCheckBoxes &&
                         e.ItemIndex == 0)
                         ? false
                         : (e.ItemData as Student).IsDraWSeparator;

    if (isEnableDraw)
    {
        e.Handled = true;
        var listView = sender as SfListView;

        // Access internal layout and checkbox style using reflection
        var linearLayout = listView.GetType().GetProperty("LinearLayout", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(listView);
        var items = linearLayout.GetType().GetProperty("Items").GetValue(linearLayout) as List<ListViewItemInfo>;
        var checkBoxStyle = items[e.ItemIndex].GetType().GetProperty("CheckBoxStyle", BindingFlags.NonPublic | BindingFlags.Instance);
        checkBoxStyle.SetValue(items[e.ItemIndex], listView.Style.CheckBoxStyle);

        // Highlight hover and selected items
        if (e.ItemIndex == mouseHoverItemIndex)
            e.Graphics.FillRectangle(new SolidBrush(listView.Style.SelectionStyle.HoverBackColor), e.Bounds);
        if (sfComboBox1.SelectedIndex == e.ItemIndex)
            e.Graphics.FillRectangle(new SolidBrush(listView.Style.SelectionStyle.SelectionBackColor), new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height - 2));

        RectangleF bounds = new RectangleF(e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height);

        // Draw checkbox if enabled
        if (sfComboBox1.ComboBoxMode == ComboBoxMode.MultiSelection && sfComboBox1.DropDownListView.ShowCheckBoxes)
        {
           (e.Bounds.X + 2, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2),
                                         listView.CheckedItems.Contains(e.ItemData) ? CheckState.Checked : CheckState.Unchecked,
                                         listView.Style.CheckBoxStyle);
            bounds = new RectangleF(e.Bounds.X + 20, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Height - 2);
        }

        // Draw item text
        e.Graphics.DrawString((e.ItemData as Student).StudentName, this.Font, new SolidBrush(Color.FromArgb(255, 51, 51, 51)), bounds);

        // Draw separator line
        e.Graphics.DrawLine(new Pen(Color.Gray, 1) { DashPattern = new[] { 2f, 2f } }, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
    }
}

int mouseHoverItemIndex = -1;

private void SfComboBox1_MouseMove(object sender, MouseEventArgs e)
{
    var list = sender as SfListView;
    mouseHoverItemIndex = list.GetRowIndexAtPoint(e.Location);
    list.Invalidate();
}
️
```
## Referece
For more details, refer to the official KB article: https://www.syncfusion.com/kb/11498/how-to-add-separator-line-in-winforms-sfcombobox-dropdown-iems

## Screenshot
![Separator in SfComboBox](SfComboBox/SfComboBox/Image/Add%20Separator%20to%20ComboBox.png)
