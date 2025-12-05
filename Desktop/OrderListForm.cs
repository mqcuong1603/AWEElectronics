using System;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;


namespace AWEElectronics.Desktop
{
    public partial class OrderListForm : Form
    {
        private User currentUser;
        private OrderBLL orderBLL;

        public OrderListForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            orderBLL = new OrderBLL();

            LoadOrders();
            LoadStatusFilter();
        }

        private void LoadOrders()
        {
            try
            {
                var orders = orderBLL.GetAll();
                dgvOrders.DataSource = orders.Select(o => new
                {
                    o.OrderID,
                    o.OrderCode,
                    Customer = o.Customer?.FullName ?? "N/A",
                    o.OrderDate,
                    o.SubTotal,
                    o.Tax,
                    o.GrandTotal,
                    o.Status,
                    Staff = o.StaffChecked?.FullName ?? "Not Assigned"
                }).ToList();

                dgvOrders.Columns["OrderID"].Visible = false;
                lblTotal.Text = $"Total Orders: {orders.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading orders: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadStatusFilter()
        {
            cboStatus.Items.Add("All");
            cboStatus.Items.Add("Pending");
            cboStatus.Items.Add("Processing");
            cboStatus.Items.Add("Shipped");
            cboStatus.Items.Add("Delivered");
            cboStatus.Items.Add("Cancelled");
            cboStatus.SelectedIndex = 0;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                var orders = orderBLL.GetAll();

                string selectedStatus = cboStatus.SelectedItem.ToString();
                if (selectedStatus != "All")
                {
                    orders = orders.Where(o => o.Status == selectedStatus).ToList();
                }

                dgvOrders.DataSource = orders.Select(o => new
                {
                    o.OrderID,
                    o.OrderCode,
                    Customer = o.Customer?.FullName ?? "N/A",
                    o.OrderDate,
                    o.SubTotal,
                    o.Tax,
                    o.GrandTotal,
                    o.Status,
                    Staff = o.StaffChecked?.FullName ?? "Not Assigned"
                }).ToList();

                dgvOrders.Columns["OrderID"].Visible = false;
                lblTotal.Text = $"Filtered Orders: {orders.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering orders: {ex.Message}", "Error");
            }
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)
            {
                MessageBox.Show("Please select an order to update!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["OrderID"].Value);
            string currentStatus = dgvOrders.CurrentRow.Cells["Status"].Value.ToString();

            using (var statusForm = new UpdateOrderStatusForm(currentStatus))
            {
                if (statusForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        orderBLL.UpdateOrderStatus(orderId, statusForm.SelectedStatus);
                        MessageBox.Show("Order status updated successfully!", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadOrders();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error updating status: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvOrders.CurrentRow == null)
            {
                MessageBox.Show("Please select an order to view details!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int orderId = Convert.ToInt32(dgvOrders.CurrentRow.Cells["OrderID"].Value);

            try
            {
                Order order = orderBLL.GetById(orderId);
                var details = orderBLL.GetOrderDetails(orderId);

                string message = $"Order: {order.OrderCode}\n";
                message += $"Customer: {order.Customer?.FullName}\n";
                message += $"Date: {order.OrderDate}\n";
                message += $"Status: {order.Status}\n";
                message += $"Total: ${order.GrandTotal:N2}\n\n";
                message += "Items:\n";

                foreach (var item in details)
                {
                    message += $"- {item.Product?.Name} (Qty: {item.Quantity}) - ${item.Total:N2}\n";
                }

                MessageBox.Show(message, "Order Details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading details: {ex.Message}", "Error");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cboStatus.SelectedIndex = 0;
            LoadOrders();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvOrders_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnViewDetails_Click(sender, e);
            }
        }
    }
}