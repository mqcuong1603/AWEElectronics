using System;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.Desktop
{
    public partial class ReportForm : Form
    {
        private User currentUser;
        private ReportBLL reportBLL;

        public ReportForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            reportBLL = new ReportBLL();

            dtpFrom.Value = DateTime.Now.AddMonths(-1);
            dtpTo.Value = DateTime.Now;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime fromDate = dtpFrom.Value.Date;
            DateTime toDate = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

            if (fromDate > toDate)
            {
                MessageBox.Show("'From Date' cannot be later than 'To Date'!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var salesReport = reportBLL.GetDailySalesReport(fromDate, toDate);

                dgvReport.DataSource = salesReport.Select(s => new
                {
                    Date = s.Date.ToString("yyyy-MM-dd"),
                    s.TotalOrders,
                    s.TotalRevenue,
                    AverageOrder = s.TotalOrders > 0 ? s.TotalRevenue / s.TotalOrders : 0
                }).ToList();

                decimal totalRevenue = salesReport.Sum(s => s.TotalRevenue);
                int totalOrders = salesReport.Sum(s => s.TotalOrders);

                lblTotalRevenue.Text = $"Total Revenue: ${totalRevenue:N2}";
                lblTotalOrders.Text = $"Total Orders: {totalOrders}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTopProducts_Click(object sender, EventArgs e)
        {
            try
            {
                var topProducts = reportBLL.GetTopSellingProducts(10);

                dgvReport.DataSource = topProducts.Select(p => new
                {
                    p.ProductName,
                    p.QuantitySold,
                    p.TotalRevenue
                }).ToList();

                lblTotalRevenue.Text = $"Total from Top 10: ${topProducts.Sum(p => p.TotalRevenue):N2}";
                lblTotalOrders.Text = $"Units Sold: {topProducts.Sum(p => p.QuantitySold)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading top products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Export functionality - To be implemented\n\nWould export to CSV/Excel",
                "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}