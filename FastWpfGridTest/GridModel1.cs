using FastWpfGrid;
using System;
using System.Collections.Generic;
using System.Windows;

namespace FastWpfGridTest
{
    public class RowData
    {
        public int Id;
        public int[] DoubleData = new int[20];
        public string[] StringData = new string[21];
        static Random rd = new Random((int)DateTime.Now.Ticks);
        public RowData(int id)
        {
            Id = id;

            for (int i = 0; i < 20; i++)
            {
                DoubleData[i] = rd.Next(100000);
            }
            for (int i = 0; i < 20; i++)
            {
                StringData[i] = "str" + rd.Next(100000);
            }
            StringData[20] = DateTime.Now.ToLongDateString()+" "+ DateTime.Now.ToLongTimeString();
        }
    }
    public class GridModel1 : FastGridModelBase
    {
        private Dictionary<Tuple<int, int>, string> _editedCells = new Dictionary<Tuple<int, int>, string>();
        private static string[] _columnBasicNames = new[] { "", "Value:", "Long column value:" };
        private List<RowData> rows = new List<RowData>();

        private void LoadData()
        {
            for (int i = 0; i < 2000; i++)
            {
                rows.Add(new RowData(i));
            }
        }
        public GridModel1()
        {
            LoadData();
        }
        public override int ColumnCount
        {
            get { return 41; }
        }

        public override int RowCount
        {
            get { return 2000; }
        }
        public override string GetColumnHeaderText(int column)
        {
            if (column >= 20)
            {
                return $"Str{column - 20}";
            }
            else
            {
                return $"    Num{column}";
            }
        }
        public override string GetRowHeaderText(int row)
        {
            return rows[row].Id.ToString();
        }
        public class DoubleSort : IComparer<RowData>
        {
            bool isAsending;
            int column;
            public DoubleSort(bool isAsending, int column)
            {
                this.isAsending = isAsending;
                this.column = column;
            }
            public int Compare(RowData x, RowData y)
            {
                if (isAsending)
                {
                    if (column > 20)
                    {
                        return x.StringData[column - 20].CompareTo(y.StringData[column - 20]);
                    }
                    else
                    {
                        return x.DoubleData[column].CompareTo(y.DoubleData[column]);
                    }
                }
                else
                {
                    if (column > 20)
                    {
                        return y.StringData[column - 20].CompareTo(x.StringData[column - 20]);
                    }
                    else
                    {
                        return y.DoubleData[column].CompareTo(x.DoubleData[column]);
                    }


                }

            }
        }
        public void Sort(bool isAsending, int column)
        {
            rows.Sort(new DoubleSort(isAsending, column));
        }

        public override string GetCellText(int row, int column)
        {
            var rowData = rows[row];
            if (column >= 20)
            {
                return rowData.StringData[column - 20];
            }
            else
            {
                return rowData.DoubleData[column].ToString();
            }

        }


        public override IFastGridCell GetGridHeader(IFastGridView view)
        {
            var impl = new FastGridCellImpl();
            var btn = impl.AddImageBlock(view.IsTransposed ? "/Images/flip_horizontal_small.png" : "/Images/flip_vertical_small.png");
            btn.CommandParameter = FastWpfGrid.FastGridControl.ToggleTransposedCommand;
            btn.ToolTip = "Swap rows and columns";
            //impl.AddImageBlock("/Images/foreign_keysmall.png").CommandParameter = "FK";
            //impl.AddImageBlock("/Images/primary_keysmall.png").CommandParameter = "PK";
            return impl;
        }

        public override void HandleCommand(IFastGridView view, FastGridCellAddress address, object commandParameter, ref bool handled)
        {
            base.HandleCommand(view, address, commandParameter, ref handled);
            if (commandParameter is string) MessageBox.Show(commandParameter.ToString());
        }

        public override int? SelectedRowCountLimit
        {
            get { return 100; }
        }

        public override void HandleSelectionCommand(IFastGridView view, string command)
        {
            MessageBox.Show(command);
        }
        public void UpdateCell(int row, int column)
        {
            if (column >= 20)
            {
                rows[row].StringData[column - 20] = "12345";
            }
            else
            {
                rows[row].DoubleData[column] = 12345;
            }

        }
        public override int GetValue(int row, int column)
        {
            if (column >= 20)
            {
                return 0;
            }
            else
            {
                return rows[row].DoubleData[column];
            }
        }

    }
}
