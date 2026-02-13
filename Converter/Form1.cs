using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Converter
{
    public partial class Form1 : Form
    {
        private string currentCategory = "Length";

        public Form1()
        {
            InitializeComponent();
            LoadUnits();
        }

        /// <summary>
        /// Load units when category changes
        /// </summary>
        private void Category_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (rb == radioButtonLength)
                    currentCategory = "Length";
                else if (rb == radioButtonMass)
                    currentCategory = "Mass";
                else if (rb == radioButtonTemperature)
                    currentCategory = "Temperature";

                LoadUnits();
                UpdateInfo();
            }
        }

        /// <summary>
        /// Load units into ComboBoxes
        /// </summary>
        private void LoadUnits()
        {
            string[] units = UnitConverter.GetUnits(currentCategory);
            comboBoxFrom.Items.Clear();
            comboBoxTo.Items.Clear();
            comboBoxFrom.Items.AddRange(units);
            comboBoxTo.Items.AddRange(units);

            if (units.Length > 0)
            {
                comboBoxFrom.SelectedIndex = 0;
                comboBoxTo.SelectedIndex = Math.Min(1, units.Length - 1);
            }
        }

        /// <summary>
        /// Update info string
        /// </summary>
        private void UpdateInfo()
        {
            if (comboBoxFrom.SelectedItem != null && comboBoxTo.SelectedItem != null)
            {
                string fromSymbol = UnitConverter.GetUnitSymbol(comboBoxFrom.SelectedItem.ToString());
                string toSymbol = UnitConverter.GetUnitSymbol(comboBoxTo.SelectedItem.ToString());
                labelInfo.Text = $"Conversion: {currentCategory} ({fromSymbol} → {toSymbol})";
            }
        }

        /// <summary>
        /// Source unit changed
        /// </summary>
        private void ComboBoxFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        /// <summary>
        /// Target unit changed
        /// </summary>
        private void ComboBoxTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateInfo();
        }

        /// <summary>
        /// Input validation (only digits and comma)
        /// </summary>
        private void TextBoxValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Allow only one comma
            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Convert button click
        /// </summary>
        private void ButtonConvert_Click(object sender, EventArgs e)
        {
            try
            {
                // Check input
                if (string.IsNullOrWhiteSpace(textBoxValue.Text))
                {
                    MessageBox.Show("Enter a value to convert!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Check unit selection
                if (comboBoxFrom.SelectedItem == null || comboBoxTo.SelectedItem == null)
                {
                    MessageBox.Show("Select units!", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Parse value (replace dot with comma)
                double value = double.Parse(textBoxValue.Text.Replace('.', ','));

                string fromUnit = comboBoxFrom.SelectedItem.ToString();
                string toUnit = comboBoxTo.SelectedItem.ToString();

                // Convert
                double result = UnitConverter.Convert(value, fromUnit, toUnit, currentCategory);

                // Show result
                string fromSymbol = UnitConverter.GetUnitSymbol(fromUnit);
                string toSymbol = UnitConverter.GetUnitSymbol(toUnit);

                labelResult.Text = $"{value:F2} {fromSymbol} = {result:F4} {toSymbol}";

                // Change color based on category
                if (currentCategory == "Length")
                    labelResult.ForeColor = Color.Green;
                else if (currentCategory == "Mass")
                    labelResult.ForeColor = Color.Blue;
                else if (currentCategory == "Temperature")
                    labelResult.ForeColor = Color.Red;
                else
                    labelResult.ForeColor = Color.Black;
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter a valid number (use comma)!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Conversion error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
