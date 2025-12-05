using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Desktop.Helpers;

namespace Desktop.Forms.Users
{
    public partial class UserListForm : Form
    {
        private readonly UserBLL _userBLL;
        private DataGridView dgvUsers;
        private TextBox txtSearch;

        public UserListForm()
        {
            InitializeComponent();
            _userBLL = new UserBLL();
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Maximized;
            LoadUsers();
        }

        private void InitializeComponent()
        {
            this.Text = "User Management";
            this.Size = new Size(1100, 700);

            // Title
            Label lblTitle = new Label
            {
                Text = "User Management",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Search Panel
            Panel searchPanel = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(1040, 50),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(10, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            txtSearch = new TextBox
            {
                Location = new Point(80, 12),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.TextChanged += (s, e) => FilterUsers();

            Button btnAdd = new Button
            {
                Text = "+ Add User",
                Location = new Point(900, 10),
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += BtnAdd_Click;

            searchPanel.Controls.Add(lblSearch);
            searchPanel.Controls.Add(txtSearch);
            searchPanel.Controls.Add(btnAdd);

            // DataGridView
            dgvUsers = new DataGridView
            {
                Location = new Point(20, 140),
                Size = new Size(1040, 500),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 9)
            };
            dgvUsers.CellDoubleClick += DgvUsers_CellDoubleClick;

            // Add Action Buttons Column
            DataGridViewButtonColumn btnEditCol = new DataGridViewButtonColumn
            {
                Name = "Edit",
                Text = "Edit",
                UseColumnTextForButtonValue = true,
                Width = 70
            };

            DataGridViewButtonColumn btnDeleteCol = new DataGridViewButtonColumn
            {
                Name = "Delete",
                Text = "Delete",
                UseColumnTextForButtonValue = true,
                Width = 70
            };

            dgvUsers.CellClick += DgvUsers_CellClick;

            // Close Button
            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(960, 650),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();

            // Add controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(searchPanel);
            this.Controls.Add(dgvUsers);
            this.Controls.Add(btnClose);
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userBLL.GetAll();

                dgvUsers.DataSource = null;
                dgvUsers.Columns.Clear();

                // Add columns manually for better control
                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "UserID",
                    HeaderText = "ID",
                    Width = 50
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Username",
                    HeaderText = "Username",
                    Width = 150
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "FullName",
                    HeaderText = "Full Name",
                    Width = 200
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    Width = 200
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Role",
                    HeaderText = "Role",
                    Width = 100
                });

                dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Status",
                    HeaderText = "Status",
                    Width = 100
                });

                dgvUsers.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                });

                dgvUsers.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                });

                dgvUsers.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterUsers()
        {
            try
            {
                var users = _userBLL.GetAll();
                string searchText = txtSearch.Text.ToLower();

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    users = users.Where(u =>
                        u.Username.ToLower().Contains(searchText) ||
                        u.FullName.ToLower().Contains(searchText) ||
                        u.Email.ToLower().Contains(searchText) ||
                        u.Role.ToLower().Contains(searchText)
                    ).ToList();
                }

                dgvUsers.DataSource = null;
                dgvUsers.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering users: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            UserFormDialog form = new UserFormDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadUsers();
            }
        }

        private void DgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var user = (User)dgvUsers.Rows[e.RowIndex].DataBoundItem;
                UserFormDialog form = new UserFormDialog(user);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var user = (User)dgvUsers.Rows[e.RowIndex].DataBoundItem;

            if (dgvUsers.Columns[e.ColumnIndex].Name == "Edit")
            {
                UserFormDialog form = new UserFormDialog(user);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
            else if (dgvUsers.Columns[e.ColumnIndex].Name == "Delete")
            {
                var result = MessageBox.Show(
                    $"Are you sure you want to delete user '{user.FullName}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var deleteResult = _userBLL.DeleteUser(user.UserID);
                    if (deleteResult.Success)
                    {
                        MessageBox.Show(deleteResult.Message, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadUsers();
                    }
                    else
                    {
                        MessageBox.Show(deleteResult.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
