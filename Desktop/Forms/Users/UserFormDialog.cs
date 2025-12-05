using System;
using System.Drawing;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Desktop.Forms.Users
{
    public partial class UserFormDialog : Form
    {
        private readonly UserBLL _userBLL;
        private readonly User _user;
        private readonly bool _isEditMode;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private ComboBox cmbRole;
        private ComboBox cmbStatus;

        public UserFormDialog(User user = null)
        {
            InitializeComponent();
            _userBLL = new UserBLL();
            _user = user;
            _isEditMode = user != null;

            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            if (_isEditMode)
            {
                LoadUserData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Edit User" : "Add New User";
            this.Size = new Size(500, 480);

            // Title
            Label lblTitle = new Label
            {
                Text = _isEditMode ? "Edit User" : "Add New User",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Username
            Label lblUsername = new Label
            {
                Text = "Username:",
                Location = new Point(30, 80),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtUsername = new TextBox
            {
                Location = new Point(160, 78),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                ReadOnly = _isEditMode, // Username cannot be changed in edit mode
                BackColor = _isEditMode ? Color.FromArgb(240, 240, 240) : Color.White
            };

            if (_isEditMode)
            {
                Label lblUsernameNote = new Label
                {
                    Text = "(Username cannot be changed)",
                    Location = new Point(160, 103),
                    Size = new Size(300, 20),
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.Gray
                };
                this.Controls.Add(lblUsernameNote);
            }

            // Password
            Label lblPassword = new Label
            {
                Text = "Password:",
                Location = new Point(30, 120),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtPassword = new TextBox
            {
                Location = new Point(160, 118),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };

            if (_isEditMode)
            {
                Label lblPasswordNote = new Label
                {
                    Text = "(Leave blank to keep current password)",
                    Location = new Point(160, 145),
                    Size = new Size(300, 20),
                    Font = new Font("Segoe UI", 8),
                    ForeColor = Color.Gray
                };
                this.Controls.Add(lblPasswordNote);
            }

            // Full Name
            Label lblFullName = new Label
            {
                Text = "Full Name:",
                Location = new Point(30, 170),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtFullName = new TextBox
            {
                Location = new Point(160, 168),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Email
            Label lblEmail = new Label
            {
                Text = "Email:",
                Location = new Point(30, 210),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtEmail = new TextBox
            {
                Location = new Point(160, 208),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Role
            Label lblRole = new Label
            {
                Text = "Role:",
                Location = new Point(30, 250),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            cmbRole = new ComboBox
            {
                Location = new Point(160, 248),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new object[] { "Admin", "Staff", "Accountant", "Agent" });
            cmbRole.SelectedIndex = 3; // Default to Agent

            // Status
            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(30, 290),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            cmbStatus = new ComboBox
            {
                Location = new Point(160, 288),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "Active", "Inactive", "Locked" });
            cmbStatus.SelectedIndex = 0; // Default to Active

            // Save Button
            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(260, 370),
                Size = new Size(100, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            // Cancel Button
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(370, 370),
                Size = new Size(100, 40),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblFullName);
            this.Controls.Add(txtFullName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblRole);
            this.Controls.Add(cmbRole);
            this.Controls.Add(lblStatus);
            this.Controls.Add(cmbStatus);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadUserData()
        {
            if (_user != null)
            {
                txtUsername.Text = _user.Username;
                txtFullName.Text = _user.FullName;
                txtEmail.Text = _user.Email;
                cmbRole.SelectedItem = _user.Role;
                cmbStatus.SelectedItem = _user.Status;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isEditMode)
                {
                    // Update user
                    _user.FullName = txtFullName.Text.Trim();
                    _user.Email = txtEmail.Text.Trim();
                    _user.Role = cmbRole.SelectedItem.ToString();
                    _user.Status = cmbStatus.SelectedItem.ToString();

                    var result = _userBLL.UpdateUser(_user);

                    if (result.Success)
                    {
                        // Update password if provided
                        if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                        {
                            // For simplicity, we'll need to add a method to update password without requiring current password
                            // In a production system, you'd want proper password update flow
                            MessageBox.Show("User updated successfully! Note: Password change requires separate implementation.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(result.Message, "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Create new user
                    var user = new User
                    {
                        Username = txtUsername.Text.Trim(),
                        FullName = txtFullName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Role = cmbRole.SelectedItem.ToString(),
                        Status = cmbStatus.SelectedItem.ToString()
                    };

                    string password = txtPassword.Text;

                    var result = _userBLL.CreateUser(user, password);

                    if (result.Success)
                    {
                        MessageBox.Show(result.Message, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
