using System;
using System.Drawing;
using System.Windows.Forms;
using AWEElectronics.BLL;
using Desktop.Helpers;

namespace Desktop.Forms
{
    public partial class LoginForm : Form
    {
        private readonly UserBLL _userBLL;

        public LoginForm()
        {
            InitializeComponent();
            _userBLL = new UserBLL();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
        }

        private void InitializeComponent()
        {
            this.Text = "AWE Electronics - Login";
            this.Size = new Size(450, 350);
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Title Label
            Label lblTitle = new Label
            {
                Text = "AWE Electronics",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(100, 20),
                Size = new Size(250, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(33, 150, 243)
            };

            Label lblSubtitle = new Label
            {
                Text = "Desktop Management System",
                Font = new Font("Segoe UI", 10),
                Location = new Point(100, 60),
                Size = new Size(250, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.Gray
            };

            // Username
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(50, 110),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10)
            };

            TextBox txtUsername = new TextBox
            {
                Name = "txtUsername",
                Location = new Point(50, 135),
                Size = new Size(340, 30),
                Font = new Font("Segoe UI", 11)
            };

            // Password
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(50, 175),
                Size = new Size(100, 25),
                Font = new Font("Segoe UI", 10)
            };

            TextBox txtPassword = new TextBox
            {
                Name = "txtPassword",
                Location = new Point(50, 200),
                Size = new Size(340, 30),
                Font = new Font("Segoe UI", 11),
                UseSystemPasswordChar = true
            };

            // Login Button
            Button btnLogin = new Button
            {
                Name = "btnLogin",
                Text = "Login",
                Location = new Point(50, 250),
                Size = new Size(160, 40),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += (s, e) => BtnLogin_Click(txtUsername, txtPassword);

            // Exit Button
            Button btnExit = new Button
            {
                Name = "btnExit",
                Text = "Exit",
                Location = new Point(230, 250),
                Size = new Size(160, 40),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();

            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblSubtitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnExit);

            // Set Enter key to trigger login
            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(TextBox txtUsername, TextBox txtPassword)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter username.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                var result = _userBLL.Login(username, password);

                if (result.Success)
                {
                    // Store user in session
                    SessionManager.CurrentUser = result.User;

                    MessageBox.Show($"Welcome, {result.User.FullName}!", "Login Successful",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open main form and hide login
                    this.Hide();
                    MainForm mainForm = new MainForm();
                    mainForm.FormClosed += (s, e) => this.Close();
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show(result.Message, "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
