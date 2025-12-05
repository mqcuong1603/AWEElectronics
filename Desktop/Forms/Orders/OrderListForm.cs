using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Desktop.Helpers;

namespace Desktop.Forms.Orders
{
    public partial class OrderListForm : Form
    {
        private readonly OrderBLL _orderBLL;
        private DataGridView dgvOrders;
        private ComboBox cmbStatus;
        private TextBox txtSearch;

        public OrderListForm()
        {
            InitializeComponent();
            _orderBLL = new OrderBLL();
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Maximized;
            LoadOrders();
        }

        private void InitializeComponent()
        {
            this.Text = "Order Management";
            this.Size = new Size(1200, 700);

            // Title
            Label lblTitle = new Label
            {
                Text = "Order Management",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Filter Panel
            Panel filterPanel = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(1140, 50),
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
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };
            txtSearch.TextChanged += (s, e) => FilterOrders();

            Label lblStatus = new Label
            {
                Text = "Status:",
                Location = new Point(350, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            cmbStatus = new ComboBox
            {
                Location = new Point(420, 12),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "All", "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += (s, e) => FilterOrders();

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(600, 10),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadOrders();

            filterPanel.Controls.Add(lblSearch);
            filterPanel.Controls.Add(txtSearch);
            filterPanel.Controls.Add(lblStatus);
            filterPanel.Controls.Add(cmbStatus);
            filterPanel.Controls.Add(btnRefresh);

            // DataGridView
            dgvOrders = new DataGridView
            {
                Location = new Point(20, 140),
                Size = new Size(1140, 480),
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
            dgvOrders.CellDoubleClick += DgvOrders_CellDoubleClick;
            dgvOrders.CellClick += DgvOrders_CellClick;
            dgvOrders.CellFormatting += DgvOrders_CellFormatting;

            // Close Button
            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(1060, 630),
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
            this.Controls.Add(filterPanel);
            this.Controls.Add(dgvOrders);
            this.Controls.Add(btnClose);
        }

        private void LoadOrders()
        {
            try
            {
                var orders = _orderBLL.GetAll();

                dgvOrders.DataSource = null;
                dgvOrders.Columns.Clear();

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "OrderID",
                    HeaderText = "ID",
                    Width = 50
                });

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "OrderCode",
                    HeaderText = "Order Code",
                    Width = 120
                });

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CustomerName",
                    HeaderText = "Customer",
                    Width = 180
                });

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "OrderDate",
                    HeaderText = "Order Date",
                    Width = 140,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
                });

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "GrandTotal",
                    HeaderText = "Total",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
                });

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Status",
                    HeaderText = "Status",
                    Width = 100
                });

                dgvOrders.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StaffName",
                    HeaderText = "Staff",
                    Width = 150
                });

                dgvOrders.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "ViewDetails",
                    Text = "View Details",
                    UseColumnTextForButtonValue = true,
                    Width = 100
                });

                dgvOrders.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterOrders()
        {
            try
            {
                var orders = _orderBLL.GetAll();
                string searchText = txtSearch.Text.ToLower();
                string status = cmbStatus.SelectedItem?.ToString();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    orders = orders.Where(o =>
                        o.OrderCode.ToLower().Contains(searchText) ||
                        (o.CustomerName != null && o.CustomerName.ToLower().Contains(searchText)) ||
                        (o.CustomerEmail != null && o.CustomerEmail.ToLower().Contains(searchText))
                    ).ToList();
                }

                // Filter by status
                if (status != "All")
                {
                    orders = orders.Where(o => o.Status == status).ToList();
                }

                dgvOrders.DataSource = null;
                dgvOrders.DataSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvOrders_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Color code status column
            if (dgvOrders.Columns[e.ColumnIndex].DataPropertyName == "Status" && e.Value != null)
            {
                string status = e.Value.ToString();
                switch (status)
                {
                    case "Pending":
                        e.CellStyle.BackColor = Color.FromArgb(255, 243, 224);
                        e.CellStyle.ForeColor = Color.FromArgb(245, 124, 0);
                        break;
                    case "Processing":
                        e.CellStyle.BackColor = Color.FromArgb(227, 242, 253);
                        e.CellStyle.ForeColor = Color.FromArgb(2, 136, 209);
                        break;
                    case "Shipped":
                        e.CellStyle.BackColor = Color.FromArgb(232, 245, 233);
                        e.CellStyle.ForeColor = Color.FromArgb(56, 142, 60);
                        break;
                    case "Delivered":
                        e.CellStyle.BackColor = Color.FromArgb(200, 230, 201);
                        e.CellStyle.ForeColor = Color.FromArgb(46, 125, 50);
                        break;
                    case "Cancelled":
                        e.CellStyle.BackColor = Color.FromArgb(255, 205, 210);
                        e.CellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                        break;
                }
            }
        }

        private void DgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex < dgvOrders.Columns.Count - 1) // Exclude button column
            {
                var order = (Order)dgvOrders.Rows[e.RowIndex].DataBoundItem;
                OpenOrderDetails(order.OrderID);
            }
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var order = (Order)dgvOrders.Rows[e.RowIndex].DataBoundItem;

            if (dgvOrders.Columns[e.ColumnIndex].Name == "ViewDetails")
            {
                OpenOrderDetails(order.OrderID);
            }
        }

        private void OpenOrderDetails(int orderId)
        {
            OrderDetailsForm form = new OrderDetailsForm(orderId);
            form.ShowDialog();
            LoadOrders(); // Refresh after closing details
        }
    }
}
