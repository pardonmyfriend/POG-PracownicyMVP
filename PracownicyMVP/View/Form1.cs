using PracownicyMVP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PracownicyMVP.View
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Public interface of view

        public event Func<System.Windows.Forms.TextBox, ErrorProvider, bool> ValidateFirstName;
        public event Func<System.Windows.Forms.TextBox, ErrorProvider, bool> ValidateLastName;
        public event Func<System.Windows.Forms.ComboBox, ErrorProvider, bool> ValidatePosition;
        public event Action<ListBox> Add;
        public event Action<ListBox> SaveList;
        public event Action<ListBox> LoadList;
        public event Action<Control> Clear;
        public event Action<ListBox> Delete;
        public event Action<ListBox> Edit;
        public event Action<ListBox, System.Windows.Forms.ComboBox> SelectItem;

        public string FirstName
        {
            get { return this.tbFirstName.Text; }
            set { this.tbFirstName.Text = value; }
        }

        public string LastName
        {
            get { return this.tbLastName.Text; }
            set { this.tbLastName.Text = value; }
        }

        public DateTime BirthDate
        {
            get { return this.dataBirthDate.Value; }
            set { this.dataBirthDate.Value = value; }
        }

        public decimal Salary
        {
            get { return this.dataSalary.Value; }
            set { this.dataSalary.Value = value; }
        }

        public Position Position
        {
            get
            {
                string position = dataPosition.Text;
                switch (position)
                {
                    case "Tester":
                        return Position.Tester;
                    case "Projektant":
                        return Position.Designer;
                    case "Inżynier":
                        return Position.Engineer;
                    case "Młodszy programista":
                        return Position.JuniorProgrammer;
                    case "Starszy programista":
                        return Position.SeniorProgrammer;
                    default:
                        return Position.Tester;
                }
            }
            set
            {
                switch (value)
                {
                    case Position.Tester:
                        this.dataPosition.Text = "Tester";
                        break;
                    case Position.Designer:
                        this.dataPosition.Text = "Projektant";
                        break;
                    case Position.Engineer:
                        this.dataPosition.Text = "Inżynier";
                        break;
                    case Position.JuniorProgrammer:
                        this.dataPosition.Text = "Młodszy programista";
                        break;
                    case Position.SeniorProgrammer:
                        this.dataPosition.Text = "Starszy programista";
                        break;
                    default:
                        this.dataPosition.Text = "Tester";
                        break;
                }
            }
        }

        public TypeOfContract Contract
        {
            get
            {
                if (rb3.Checked)
                    return TypeOfContract.MandateContract;
                else if (rb2.Checked)
                    return TypeOfContract.TimeContract;
                else
                    return TypeOfContract.FullContract;
            }
            set
            {
                switch (value)
                {
                    case TypeOfContract.FullContract:
                        this.rb1.Checked = true;
                        break;
                    case TypeOfContract.TimeContract:
                        this.rb2.Checked = true;
                        break;
                    case TypeOfContract.MandateContract:
                        this.rb3.Checked = true;
                        break;
                    default:
                        this.rb1.Checked = true; ;
                        break;
                }
            }
        }

        public void ShowMessage(string text, string caption, MessageBoxIcon icon) => MessageBox.Show(text, caption,
                    MessageBoxButtons.OK, icon);

        #endregion


        #region Data validation

        public bool ValidateAll()
        {
            bool a = ValidateFirstName.Invoke(this.tbFirstName, this.errorProvider);
            bool b = ValidateLastName.Invoke(this.tbLastName, this.errorProvider);
            bool c = ValidatePosition.Invoke(this.dataPosition, this.errorProvider);

            return a && b && c;
        }
        void tbFirstName_Validating(object sender, CancelEventArgs e)
        {
            var tBox = sender as System.Windows.Forms.TextBox;
            ValidateFirstName?.Invoke(tBox, this.errorProvider);
        }

        void tbLastName_Validating(object sender, CancelEventArgs e)
        {
            var tBox = sender as System.Windows.Forms.TextBox;
            ValidateLastName?.Invoke(tBox, this.errorProvider);
        }

        void dataPosition_Validating(object sender, CancelEventArgs e)
        {
            var cBox = sender as System.Windows.Forms.ComboBox;
            ValidatePosition?.Invoke(cBox, this.errorProvider);
        }

        #endregion


        #region Button configuration

        void btnAdd_Click(object sender, EventArgs e) => Add?.Invoke(this.employeesList);

        void btnSave_Click(object sender, EventArgs e) => SaveList?.Invoke(this.employeesList);

        void btnLoad_Click(object sender, EventArgs e) => LoadList?.Invoke(this.employeesList);

        private void btnClear_Click(object sender, EventArgs e) => Clear?.Invoke(this.groupBox);

        private void btnDelete_Click(object sender, EventArgs e) => Delete?.Invoke(this.employeesList);

        private void btnEdit_Click(object sender, EventArgs e) => Edit?.Invoke(this.employeesList);

        private void employeesList_SelectedValueChanged(object sender, EventArgs e) => 
            SelectItem?.Invoke(this.employeesList, this.dataPosition);

        #endregion
    }
}