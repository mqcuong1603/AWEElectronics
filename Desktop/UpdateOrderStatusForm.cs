using System;
using System.Windows.Forms;

namespace AWEElectronics.Desktop
{
    public partial class UpdateOrderStatusForm : Form
    {
        public string SelectedStatus { get; private set; }

        public UpdateOrderStatusForm(string currentStatus)
        {
            InitializeComponent();
            LoadStatuses(currentStatus);
        }

        private void LoadStatuses(string currentStatus)
        {
            cboNewStatus.Items.Add("Pending");
            cboNewStatus.Items.Add("Processing");
            cboNewStatus.Items.Add("Shipped");
            cboNewStatus.Items.Add("Delivered");
            cboNewStatus.Items.Add("Cancelled");

            cboNewStatus.SelectedItem = currentStatus;
            lblCurrentStatus.Text = $"Current Status: {currentStatus}";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (cboNewStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a new status!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SelectedStatus = cboNewStatus.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void UpdateOrderStatusForm_Load(object sender, EventArgs e)
        {

        }
    }
}