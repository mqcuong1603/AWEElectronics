using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Desktop.Helpers;

namespace Desktop.Forms.Orders
{
    public partial class OrderDetailsForm : Form
    {
        private readonly OrderBLL _orderBLL;
        private readonly int _orderId;
        private Order _order;

        private Label lblOrderCode;
        private Label lblCustomerName;
        private Label lblOrderDate;
        private Label lblStatus;
        private Label lblSubTotal;
        private Label lblTax;
        private Label lblGrandTotal;
        private DataGridView dgvOrderDetails;
        private ComboBox cmbNewStatus;

        public OrderDetailsForm(int orderId)
        {
            InitializeComponent();
            _orderBLL = new OrderBLL();
            _orderId = orderId;

            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.Sizable;

            LoadOrderDetails();
        }

        private void InitializeComponent()
        {
            this.Text = "Order Details";
            this.Size = new Size(900, 700);

            // Title
            Label lblTitle = new Label
            {
                Text = "Order Details",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Order Information Panel
            Panel infoPanel = new Panel
            {
                Location = new Point(20, 70),
                Size = new Size(840, 180),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Order Code
            Label lblOrderCodeTitle = new Label
            {
                Text = "Order Code:",
                Location = new Point(20, 15),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblOrderCode = new Label
            {
                Text = "-",
                Location = new Point(150, 15),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Customer Name
            Label lblCustomerTitle = new Label
            {
                Text = "Customer:",
                Location = new Point(20, 45),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblCustomerName = new Label
            {
                Text = "-",
                Location = new Point(150, 45),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Order Date
            Label lblOrderDateTitle = new Label
            {
                Text = "Order Date:",
                Location = new Point(20, 75),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblOrderDate = new Label
            {
                Text = "-",
                Location = new Point(150, 75),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Status
            Label lblStatusTitle = new Label
            {
                Text = "Status:",
                Location = new Point(20, 105),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblStatus = new Label
            {
                Text = "-",
                Location = new Point(150, 105),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            // Totals
            Label lblSubTotalTitle = new Label
            {
                Text = "Subtotal:",
                Location = new Point(500, 15),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblSubTotal = new Label
            {
                Text = "$0.00",
                Location = new Point(630, 15),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblTaxTitle = new Label
            {
                Text = "Tax:",
                Location = new Point(500, 45),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            lblTax = new Label
            {
                Text = "$0.00",
                Location = new Point(630, 45),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                TextAlign = ContentAlignment.MiddleRight
            };

            Label lblGrandTotalTitle = new Label
            {
                Text = "Grand Total:",
                Location = new Point(500, 75),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            lblGrandTotal = new Label
            {
                Text = "$0.00",
                Location = new Point(630, 75),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(76, 175, 80),
                TextAlign = ContentAlignment.MiddleRight
            };

            infoPanel.Controls.Add(lblOrderCodeTitle);
            infoPanel.Controls.Add(lblOrderCode);
            infoPanel.Controls.Add(lblCustomerTitle);
            infoPanel.Controls.Add(lblCustomerName);
            infoPanel.Controls.Add(lblOrderDateTitle);
            infoPanel.Controls.Add(lblOrderDate);
            infoPanel.Controls.Add(lblStatusTitle);
            infoPanel.Controls.Add(lblStatus);
            infoPanel.Controls.Add(lblSubTotalTitle);
            infoPanel.Controls.Add(lblSubTotal);
            infoPanel.Controls.Add(lblTaxTitle);
            infoPanel.Controls.Add(lblTax);
            infoPanel.Controls.Add(lblGrandTotalTitle);
            infoPanel.Controls.Add(lblGrandTotal);

            // Order Items Label
            Label lblItems = new Label
            {
                Text = "Order Items:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 270),
                AutoSize = true
            };

            // DataGridView for Order Details
            dgvOrderDetails = new DataGridView
            {
                Location = new Point(20, 305),
                Size = new Size(840, 200),
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

            // Status Update Panel (Admin & Staff only)
            Panel statusPanel = new Panel
            {
                Location = new Point(20, 520),
                Size = new Size(840, 70),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            Label lblUpdateStatus = new Label
            {
                Text = "Update Status:",
                Location = new Point(20, 22),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            cmbNewStatus = new ComboBox
            {
                Location = new Point(150, 20),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button btnUpdateStatus = new Button
            {
                Text = "Update Status",
                Location = new Point(320, 17),
                Size = new Size(130, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnUpdateStatus.FlatAppearance.BorderSize = 0;
            btnUpdateStatus.Click += BtnUpdateStatus_Click;

            Button btnCancelOrder = new Button
            {
                Text = "Cancel Order",
                Location = new Point(470, 17),
                Size = new Size(130, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancelOrder.FlatAppearance.BorderSize = 0;
            btnCancelOrder.Click += BtnCancelOrder_Click;

            statusPanel.Controls.Add(lblUpdateStatus);
            statusPanel.Controls.Add(cmbNewStatus);
            statusPanel.Controls.Add(btnUpdateStatus);
            statusPanel.Controls.Add(btnCancelOrder);

            // Close Button
            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(760, 610),
                Size = new Size(100, 40),
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
            this.Controls.Add(infoPanel);
            this.Controls.Add(lblItems);
            this.Controls.Add(dgvOrderDetails);
            this.Controls.Add(btnClose);

            // Only show status panel for Admin/Staff
            if (SessionManager.CanManageOrders)
            {
                this.Controls.Add(statusPanel);
            }
        }

        private void LoadOrderDetails()
        {
            try
            {
                _order = _orderBLL.GetById(_orderId);

                if (_order == null)
                {
                    MessageBox.Show("Order not found.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Update labels
                lblOrderCode.Text = _order.OrderCode;
                lblCustomerName.Text = $"{_order.CustomerName} ({_order.CustomerEmail})";
                lblOrderDate.Text = _order.OrderDate.ToString("dd/MM/yyyy HH:mm");
                lblStatus.Text = _order.Status;
                UpdateStatusColor();

                lblSubTotal.Text = $"${_order.SubTotal:N2}";
                lblTax.Text = $"${_order.Tax:N2}";
                lblGrandTotal.Text = $"${_order.GrandTotal:N2}";

                // Load order details
                dgvOrderDetails.Columns.Clear();

                dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductName",
                    HeaderText = "Product",
                    Width = 300
                });

                dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "UnitPrice",
                    HeaderText = "Unit Price",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
                });

                dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Quantity",
                    HeaderText = "Quantity",
                    Width = 100
                });

                dgvOrderDetails.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Subtotal",
                    HeaderText = "Subtotal",
                    Width = 120,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
                });

                dgvOrderDetails.DataSource = _order.OrderDetails;

                // Load status options
                LoadStatusOptions();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading order details: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatusColor()
        {
            switch (_order.Status)
            {
                case "Pending":
                    lblStatus.ForeColor = Color.FromArgb(245, 124, 0);
                    break;
                case "Processing":
                    lblStatus.ForeColor = Color.FromArgb(2, 136, 209);
                    break;
                case "Shipped":
                    lblStatus.ForeColor = Color.FromArgb(56, 142, 60);
                    break;
                case "Delivered":
                    lblStatus.ForeColor = Color.FromArgb(46, 125, 50);
                    break;
                case "Cancelled":
                    lblStatus.ForeColor = Color.FromArgb(198, 40, 40);
                    break;
            }
        }

        private void LoadStatusOptions()
        {
            cmbNewStatus.Items.Clear();

            switch (_order.Status)
            {
                case "Pending":
                    cmbNewStatus.Items.Add("Processing");
                    cmbNewStatus.Items.Add("Cancelled");
                    break;
                case "Processing":
                    cmbNewStatus.Items.Add("Shipped");
                    cmbNewStatus.Items.Add("Cancelled");
                    break;
                case "Shipped":
                    cmbNewStatus.Items.Add("Delivered");
                    break;
                default:
                    cmbNewStatus.Enabled = false;
                    break;
            }

            if (cmbNewStatus.Items.Count > 0)
                cmbNewStatus.SelectedIndex = 0;
        }

        private void BtnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (cmbNewStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a new status.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newStatus = cmbNewStatus.SelectedItem.ToString();

            var result = MessageBox.Show(
                $"Are you sure you want to update order status to '{newStatus}'?",
                "Confirm Status Update",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var updateResult = _orderBLL.UpdateOrderStatus(_orderId, newStatus, SessionManager.CurrentUser.UserID);

                    if (updateResult.Success)
                    {
                        MessageBox.Show(updateResult.Message, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderDetails(); // Reload to show updated status
                    }
                    else
                    {
                        MessageBox.Show(updateResult.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancelOrder_Click(object sender, EventArgs e)
        {
            if (_order.Status == "Shipped" || _order.Status == "Delivered")
            {
                MessageBox.Show("Cannot cancel shipped or delivered orders.", "Cannot Cancel",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_order.Status == "Cancelled")
            {
                MessageBox.Show("Order is already cancelled.", "Already Cancelled",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to CANCEL this order?\n\nOrder: {_order.OrderCode}\nCustomer: {_order.CustomerName}\nTotal: ${_order.GrandTotal:N2}",
                "Confirm Order Cancellation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var cancelResult = _orderBLL.CancelOrder(_orderId, SessionManager.CurrentUser.UserID);

                    if (cancelResult.Success)
                    {
                        MessageBox.Show(cancelResult.Message, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrderDetails(); // Reload to show cancelled status
                    }
                    else
                    {
                        MessageBox.Show(cancelResult.Message, "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
}
