using Syncfusion.WinForms.Core;
using Syncfusion.WinForms.ListView;
using Syncfusion.WinForms.ListView.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

namespace SfComboBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Events

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

        #endregion

        #region Data Setting 

        public ObservableCollection<Student> GetData()
        {
            ObservableCollection<Student> StudentList  = new ObservableCollection<Student>();
            StudentList.Add(new Student() { StudentName = "Amir", IsDraWSeparator = true });
            StudentList.Add(new Student() { StudentName = "Asif"});
            StudentList.Add(new Student() { StudentName = "Catherine", IsDraWSeparator = true });
            StudentList.Add(new Student() { StudentName = "Cindrella"});
            StudentList.Add(new Student() { StudentName = "David"});
            return StudentList;
        }

        public class Student
        {
            public string StudentName { get; set; }
            public bool IsDraWSeparator { get; set; }
        }

        #endregion
    }
}