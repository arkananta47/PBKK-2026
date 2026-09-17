using System;
using System.Drawing;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        double firstNumber = 0;
        double secondNumber = 0;
        string operation = "";

        TextBox txtDisplay;
        TableLayoutPanel table;
        ListBox historyList;
        Panel historyPanel;

        CalculatorService calculator = new CalculatorService();

        public Form1()
        {
            InitializeComponent();
            CreateCalculator();
        }

        private void CreateCalculator()
        {
            Text = "Calculator+";
            Size = new Size(450, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(25, 25, 25);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label lblTitle = new Label();
            lblTitle.Text = "CALCULATOR+";
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Height = 55;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;

            txtDisplay = new TextBox();
            txtDisplay.Text = "0";
            txtDisplay.Dock = DockStyle.Top;
            txtDisplay.Height = 80;
            txtDisplay.TextAlign = HorizontalAlignment.Right;
            txtDisplay.Font = new Font("Segoe UI", 30, FontStyle.Bold);
            txtDisplay.ForeColor = Color.White;
            txtDisplay.BackColor = Color.FromArgb(40, 40, 40);
            txtDisplay.BorderStyle = BorderStyle.None;
            txtDisplay.ReadOnly = true;

            table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.Padding = new Padding(10);
            table.ColumnCount = 5;
            table.RowCount = 7;
            table.BackColor = Color.FromArgb(25, 25, 25);

            for (int i = 0; i < 5; i++)
            {
                table.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 20)
                );
            }

            for (int i = 0; i < 7; i++)
            {
                table.RowStyles.Add(
                    new RowStyle(SizeType.Percent, 14.28f)
                );
            }

            AddButton("sin", ScientificButton_Click, 0, 0);
            AddButton("cos", ScientificButton_Click, 1, 0);
            AddButton("tan", ScientificButton_Click, 2, 0);
            AddButton("√", ScientificButton_Click, 3, 0);
            AddButton("x²", ScientificButton_Click, 4, 0);

            AddButton("7", NumberButton_Click, 0, 1);
            AddButton("8", NumberButton_Click, 1, 1);
            AddButton("9", NumberButton_Click, 2, 1);
            AddButton("÷", OperatorButton_Click, 3, 1);
            AddButton("⌫", BackspaceButton_Click, 4, 1);

            AddButton("4", NumberButton_Click, 0, 2);
            AddButton("5", NumberButton_Click, 1, 2);
            AddButton("6", NumberButton_Click, 2, 2);
            AddButton("×", OperatorButton_Click, 3, 2);
            AddButton("%", PercentageButton_Click, 4, 2);

            AddButton("1", NumberButton_Click, 0, 3);
            AddButton("2", NumberButton_Click, 1, 3);
            AddButton("3", NumberButton_Click, 2, 3);
            AddButton("−", OperatorButton_Click, 3, 3);
            AddButton("±", SignButton_Click, 4, 3);

            AddButton("0", NumberButton_Click, 0, 4);
            AddButton(".", btnDecimal_Click, 1, 4);
            AddButton("C", btnClear_Click, 2, 4);
            AddButton("+", OperatorButton_Click, 3, 4);
            AddButton("=", btnEquals_Click, 4, 4);

            AddButton("HISTORY", HistoryButton_Click, 0, 5);
            Button historyButton = (Button)table.GetControlFromPosition(0, 5);
            table.SetColumnSpan(historyButton, 5);

            historyPanel = new Panel();
            historyPanel.Dock = DockStyle.Fill;
            historyPanel.BackColor = Color.FromArgb(35, 35, 35);
            historyPanel.Visible = false;

            Label historyTitle = new Label();
            historyTitle.Text = "CALCULATION HISTORY";
            historyTitle.Dock = DockStyle.Top;
            historyTitle.Height = 55;
            historyTitle.TextAlign = ContentAlignment.MiddleCenter;
            historyTitle.Font =
                new Font("Segoe UI", 15, FontStyle.Bold);
            historyTitle.ForeColor = Color.White;

            historyList = new ListBox();
            historyList.Dock = DockStyle.Fill;
            historyList.BackColor = Color.FromArgb(35, 35, 35);
            historyList.ForeColor = Color.White;
            historyList.Font =
                new Font("Segoe UI", 11);
            historyList.BorderStyle = BorderStyle.None;

            Button clearHistoryButton = new Button();
            clearHistoryButton.Text = "CLEAR HISTORY";
            clearHistoryButton.Dock = DockStyle.Bottom;
            clearHistoryButton.Height = 50;
            clearHistoryButton.Margin = new Padding(10);
            clearHistoryButton.Font =
                new Font("Segoe UI", 11, FontStyle.Bold);
            clearHistoryButton.FlatStyle = FlatStyle.Flat;
            clearHistoryButton.FlatAppearance.BorderSize = 0;
            clearHistoryButton.BackColor =
                Color.FromArgb(180, 50, 50);
            clearHistoryButton.ForeColor = Color.White;
            clearHistoryButton.Click +=
                ClearHistoryButton_Click;

            Button backButton = new Button();
            backButton.Text = "← BACK TO CALCULATOR";
            backButton.Dock = DockStyle.Bottom;
            backButton.Height = 50;
            backButton.Margin = new Padding(10);
            backButton.Font =
                new Font("Segoe UI", 11, FontStyle.Bold);
            backButton.FlatStyle = FlatStyle.Flat;
            backButton.FlatAppearance.BorderSize = 0;
            backButton.BackColor =
                Color.FromArgb(60, 100, 150);
            backButton.ForeColor = Color.White;
            backButton.Click +=
                BackToCalculatorButton_Click;

            historyPanel.Controls.Add(historyList);
            historyPanel.Controls.Add(historyTitle);
            historyPanel.Controls.Add(backButton);
            historyPanel.Controls.Add(clearHistoryButton);

            Controls.Add(historyPanel);
            Controls.Add(table);
            Controls.Add(txtDisplay);
            Controls.Add(lblTitle);
        }

        private void AddButton(
            string text,
            EventHandler handler,
            int column,
            int row)
        {
            Button button = new Button();

            button.Text = text;
            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(4);
            button.Font = new Font(
                "Segoe UI",
                13,
                FontStyle.Bold
            );

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.ForeColor = Color.White;
            button.BackColor = Color.FromArgb(55, 55, 55);
            button.Cursor = Cursors.Hand;

            if (text == "+" ||
                text == "−" ||
                text == "×" ||
                text == "÷")
            {
                button.BackColor =
                    Color.FromArgb(75, 90, 110);
            }

            if (text == "=")
            {
                button.BackColor =
                    Color.FromArgb(0, 120, 215);

                button.Font =
                    new Font(
                        "Segoe UI",
                        17,
                        FontStyle.Bold
                    );
            }

            if (text == "C" ||
                text == "Clear History")
            {
                button.BackColor =
                    Color.FromArgb(180, 50, 50);
            }

            if (text == "HISTORY")
            {
                button.BackColor =
                    Color.FromArgb(60, 100, 150);
            }

            if (text == "sin" ||
                text == "cos" ||
                text == "tan" ||
                text == "√" ||
                text == "x²")
            {
                button.BackColor =
                    Color.FromArgb(70, 70, 90);
            }

            button.Click += handler;

            table.Controls.Add(button, column, row);
        }

        private void NumberButton_Click(
            object sender,
            EventArgs e)
        {
            Button button = (Button)sender;

            if (txtDisplay.Text == "0")
                txtDisplay.Text = button.Text;
            else
                txtDisplay.Text += button.Text;
        }

        private void OperatorButton_Click(
            object sender,
            EventArgs e)
        {
            Button button = (Button)sender;

            firstNumber =
                double.Parse(txtDisplay.Text);

            operation = button.Text;
            txtDisplay.Clear();
        }

        private void btnEquals_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                secondNumber =
                    double.Parse(txtDisplay.Text);

                double result =
                    calculator.Calculate(
                        firstNumber,
                        secondNumber,
                        operation
                    );

                string calculation =
                    $"{firstNumber} {operation} {secondNumber} = {result}";

                txtDisplay.Text =
                    result.ToString();

                historyList.Items.Add(calculation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnClear_Click(
            object sender,
            EventArgs e)
        {
            firstNumber = 0;
            secondNumber = 0;
            operation = "";
            txtDisplay.Text = "0";
        }

        private void btnDecimal_Click(
            object sender,
            EventArgs e)
        {
            if (!txtDisplay.Text.Contains("."))
                txtDisplay.Text += ".";
        }

        private void SignButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                double number =
                    double.Parse(txtDisplay.Text);

                txtDisplay.Text =
                    calculator.ToggleSign(number).ToString();
            }
            catch
            {
                txtDisplay.Text = "0";
            }
        }

        private void PercentageButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                double number =
                    double.Parse(txtDisplay.Text);

                txtDisplay.Text =
                    calculator.Percentage(number).ToString();
            }
            catch
            {
                txtDisplay.Text = "0";
            }
        }

        private void BackspaceButton_Click(
            object sender,
            EventArgs e)
        {
            if (txtDisplay.Text.Length > 1)
            {
                txtDisplay.Text =
                    txtDisplay.Text.Substring(
                        0,
                        txtDisplay.Text.Length - 1
                    );
            }
            else
            {
                txtDisplay.Text = "0";
            }
        }

        private void ScientificButton_Click(
            object sender,
            EventArgs e)
        {
            try
            {
                Button button =
                    (Button)sender;

                double number =
                    double.Parse(txtDisplay.Text);

                double result = 0;

                switch (button.Text)
                {
                    case "sin":
                        result =
                            calculator.Sin(number);
                        break;

                    case "cos":
                        result =
                            calculator.Cos(number);
                        break;

                    case "tan":
                        result =
                            calculator.Tan(number);
                        break;

                    case "√":
                        result =
                            calculator.SquareRoot(number);
                        break;

                    case "x²":
                        result =
                            calculator.Square(number);
                        break;
                }

                string calculation =
                    $"{button.Text}({number}) = {result}";

                txtDisplay.Text =
                    result.ToString();

                historyList.Items.Add(
                    calculation
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void HistoryButton_Click(
            object sender,
            EventArgs e)
        {
            historyPanel.Visible =
                !historyPanel.Visible;

            if (historyPanel.Visible)
            {
                table.Visible = false;
                txtDisplay.Visible = false;
            }
            else
            {
                table.Visible = true;
                txtDisplay.Visible = true;
            }
        }

        private void BackToCalculatorButton_Click(
            object sender,
            EventArgs e)
        {
            historyPanel.Visible = false;
            table.Visible = true;
            txtDisplay.Visible = true;
        }

        private void ClearHistoryButton_Click(
            object sender,
            EventArgs e)
        {
            historyList.Items.Clear();
        }
    }
}