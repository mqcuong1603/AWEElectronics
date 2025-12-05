using System;
using System.Windows.Forms;
using AWEElectronics.DTO;

namespace AWEElectronics.Desktop
{
    public partial class MainForm : Form
    {
        private User currentUser;

        public MainForm(User user)
        {
            InitializeComponent();
            currentUser = user;

            lblWelcome.Text = $"Welcome, {user.FullName} ({user.Role})";

            ConfigureMenuByRole();
        }

        private void ConfigureMenuByRole()
        {
            // All roles can view products and orders
            // Only Admin and Staff can manage inventory
            if (currentUser.Role == "Agent")
            {
                inventoryToolStripMenuItem.Visible = false;
            }
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProductListForm form = new ProductListForm(currentUser);
            form.ShowDialog();
        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderListForm form = new OrderListForm(currentUser);
            form.ShowDialog();
        }

        private void goodsReceivedNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goods Received Note (GRN) - To be implemented", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void goodsDeliveryNoteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Goods Delivery Note (GDN) - To be implemented", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void reportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportForm form = new ReportForm(currentUser);
            form.ShowDialog();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}