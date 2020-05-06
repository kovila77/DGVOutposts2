using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace DGVOutposts
{
    public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn() : base(new CalendarCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class CalendarCell : DataGridViewTextBoxCell
    {
        public DateTime? MyMinDate = null;
        public DateTime? MyMaxDate = null;

        public CalendarCell()
            : base()
        {
            // Use the short date format.
            this.Style.Format = "d";
            this.Style.NullValue = null;
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            CalendarEditingControl ctl =
                DataGridView.EditingControl as CalendarEditingControl;

            DateTime cellValue;

            if (MyMinDate.HasValue)
                ctl.MinDate = MyMinDate.Value;
            else
                ctl.MinDate = DateTimePicker.MinimumDateTime;
            if (MyMaxDate.HasValue)
                ctl.MaxDate = MyMaxDate.Value;
            else
                ctl.MaxDate = DateTimePicker.MaximumDateTime;

            if (this.Value == null || this.Value == DBNull.Value)
            {
                cellValue = ctl.MinDate.Date > DateTime.Now.Date ? ctl.MinDate.Date : (ctl.MaxDate.Date < DateTime.Now.Date ? ctl.MaxDate.Date : DateTime.Now.Date);
                //this.Value = DBNull.Value;
            }
            else
            {
                cellValue = (DateTime)this.Value;
            }

            //ctl.EditingControlValueChanged = false;

            ctl.LookForChanges = false;
            ctl.Value = ctl.MinDate == cellValue ? ctl.MaxDate : ctl.MinDate;
            ctl.LookForChanges = true;
            ctl.cellValue = cellValue;
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(CalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.

                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                // return DateTime.Now;
                //return null;
                return DBNull.Value;
                //return DBNull.Value;
            }
        }
    }

    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;
        private bool lookForChanges;
        public DateTime cellValue;

        public CalendarEditingControl()
        {
            this.Format = DateTimePickerFormat.Short;
        }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        // This will throw an exception of the string is 
                        // null, empty, or not in the format of a date.
                        this.Value = DateTime.Parse((String)value);
                    }
                    catch
                    {
                        // In the case of an exception, just use the 
                        // default value so we're not left with a null
                        // value.
                        this.Value = this.Value = this.MinDate.Date > DateTime.Now.Date ? this.MinDate.Date : (this.MaxDate.Date < DateTime.Now.Date ? this.MaxDate.Date : DateTime.Now.Date);
                    }
                }
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
            //this.Value = cellValue;
            this.Value = cellValue;
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        public bool LookForChanges
        {
            get
            {
                return lookForChanges;
            }
            set
            {
                lookForChanges = value;
            }
        }

        //public DateTime CellValue
        //{
        //    get
        //    {
        //        return cellValue;
        //    }
        //    set
        //    {
        //        cellValue = value;
        //    }
        //}

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            if (LookForChanges)
            {
                //valueChanged = true;
                //this.EditingControlDataGridView.NotifyCurrentCellDirty(false);
                //this.EditingControlDataGridView.CurrentCell.Value = this.Value;
                this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                base.OnValueChanged(eventargs);
            }
        }
    }

    class tresh : IDataGridViewEditingControl
    {
        public DataGridView EditingControlDataGridView { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object EditingControlFormattedValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int EditingControlRowIndex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool EditingControlValueChanged { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Cursor EditingPanelCursor => throw new NotImplementedException();

        public bool RepositionEditingControlOnValueChange => throw new NotImplementedException();

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            throw new NotImplementedException();
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            throw new NotImplementedException();
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            throw new NotImplementedException();
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            throw new NotImplementedException();
        }
    }
}
