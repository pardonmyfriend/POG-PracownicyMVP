using PracownicyMVP.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PracownicyMVP.Presenter
{
    class EmployeePresenter
    {
        private View.Form1 _view;
        private Model.Employee _model;

        public EmployeePresenter(View.Form1 view, Model.Employee model)
        {
            _view = view;
            _model = model;

            _view.ValidateFirstName += _view_ValidateFN;
            _view.ValidateLastName += _view_ValidateLN;
            _view.ValidatePosition += _view_ValidateP;
            _view.Add += _view_Add;
            _view.SaveList += _view_SaveList;
            _view.LoadList += _view_LoadList;
            _view.Clear += _view_Clear;
            _view.Delete += _view_Delete;
            _view.Edit += _view_Edit;
            _view.SelectItem += _view_SelectItem;
        }

        private bool _view_ValidateFN(System.Windows.Forms.TextBox tBox, ErrorProvider errorP)
        {
            if (string.IsNullOrEmpty(_view.FirstName))
            {
                errorP.SetError(tBox, "Pole nie może być puste");
                return false;
            }
            else
                errorP.SetError(tBox, null);
            return true;
        }

        private bool _view_ValidateLN(System.Windows.Forms.TextBox tBox, ErrorProvider errorP)
        {
            if (string.IsNullOrEmpty(_view.LastName))
            {
                errorP.SetError(tBox, "Pole nie może być puste");
                return false;
            }
            else
                errorP.SetError(tBox, null);
            return true;
        }

        private bool _view_ValidateP(System.Windows.Forms.ComboBox cBox, ErrorProvider errorP)
        {
            if (cBox.SelectedItem == null)
            {
                errorP.SetError(cBox, "Pole nie może być puste");
                return false;
            }
            else
                errorP.SetError(cBox, null);
            return true;
        }

        private void _view_Add(ListBox lBox)
        {
            if(_view.ValidateAll())
            {
                Employee employee = new Employee(_view.FirstName, _view.LastName, _view.BirthDate, _view.Salary, 
                    _view.Position, _view.Contract);
                lBox.Items.Add(employee);
            }
            else
                _view.ShowMessage("Należy podać wszystkie dane pracownika.", "Brak informacji!", MessageBoxIcon.Warning);
        }

        void _view_SaveList(ListBox lBox)
        {
            List<Employee> list = new List<Employee>();

            foreach (Employee employee in lBox.Items)
                list.Add(employee);

            if (File.Exists("employees.xml"))
                File.Delete("employees.xml");

            Stream stream = File.OpenWrite("employees.xml");
            XmlSerializer xmlSer = new XmlSerializer(typeof(List<Employee>));
            xmlSer.Serialize(stream, list);

            stream.Close();
        }

        void _view_LoadList(ListBox lBox)
        {
            if (File.Exists("employees.xml"))
            {
                Stream stream = File.OpenRead("employees.xml");
                XmlSerializer xmlSer = new XmlSerializer(typeof(List<Employee>));
                List<Employee> employees = (List<Employee>)xmlSer.Deserialize(stream);

                lBox.Items.Clear();
                foreach (Employee employee in employees)
                    lBox.Items.Add(employee);

                stream.Close();
            }
            else
                _view.ShowMessage("Upewnij się, że istnieje plik do wczytania.", "Nie znaleziono pliku!", MessageBoxIcon.Error);

        }

        void _view_Clear(Control form)
        {
            foreach (Control control in form.Controls)
            {
                if (control is System.Windows.Forms.TextBox)
                {
                    System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)control;
                    textBox.Text = "";
                }

                if (control is DateTimePicker)
                {
                    DateTimePicker dateTime = (DateTimePicker)control;
                    dateTime.Value = DateTime.Now;
                }

                if (control is NumericUpDown)
                {
                    NumericUpDown numeric = (NumericUpDown)control;
                    numeric.Value = 3000;
                }

                if (control is System.Windows.Forms.ComboBox)
                {
                    System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)control;
                    comboBox.SelectedItem = null;
                }

                if (control is RadioButton)
                {
                    RadioButton radioButton = (RadioButton)control;
                    radioButton.Checked = true;
                }
            }
        }

        void _view_Delete(ListBox lBox)
        {
            lBox.Items.Remove(lBox.SelectedItem);
        }

        void _view_Edit(ListBox lBox)
        {
            if (_view.ValidateAll())
            {
                if (lBox.SelectedItem != null)
                {
                    Employee newEmployee = new Employee(_view.FirstName, _view.LastName, _view.BirthDate, _view.Salary, _view.Position, _view.Contract);
                    int selectedId = lBox.Items.IndexOf(lBox.SelectedItem);
                    lBox.Items.RemoveAt(selectedId);
                    lBox.Items.Insert(selectedId, newEmployee);
                }
                else
                    _view.ShowMessage("Nie wybrano żadnego pracownika do edycji", "Brak informacji!", MessageBoxIcon.Warning);
            }
            else
                _view.ShowMessage("Należy podać wszystkie dane pracownika.", "Brak informacji!", MessageBoxIcon.Warning);

        }

        void _view_SelectItem(ListBox lBox, System.Windows.Forms.ComboBox cBox)
        {
            Employee employee = new Employee();
            employee = lBox.SelectedItem as Employee;

            if (employee != null)
            {
                _view.FirstName = employee.FirstName;
                _view.LastName = employee.LastName;
                _view.BirthDate = employee.BirthDate;
                _view.Salary = employee.Salary;
                _view.Position = employee.Position;
                _view.Contract = employee.Contract;
            }
        }
    }
}
