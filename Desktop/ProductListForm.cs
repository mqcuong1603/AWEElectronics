using System;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;


namespace AWEElectronics.Desktop
{
    public partial class ProductListForm : Form
    {
        private User currentUser;
        private ProductBLL productBLL;

        public ProductListForm(User user)
        {
            InitializeComponent();
            currentUser = user;
            productBLL = new ProductBLL();

            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                var products = productBLL.GetAll();
                dgvProducts.DataSource = products.Select(p => new
                {
                    p.ProductID,
                    p.SKU,
                    p.Name,
                    Category = p.Category?.Name ?? "N/A",
                    Supplier = p.Supplier?.CompanyName ?? "N/A",
                    p.Price,
                    p.StockLevel,
                    Published = p.IsPublished ? "Yes" : "No"
                }).ToList();

                dgvProducts.Columns["ProductID"].Visible = false;
                lblTotal.Text = $"Total Products: {products.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                LoadProducts();
                return;
            }

            try
            {
                var products = productBLL.Search(keyword);
                dgvProducts.DataSource = products.Select(p => new
                {
                    p.ProductID,
                    p.SKU,
                    p.Name,
                    Category = p.Category?.Name ?? "N/A",
                    Supplier = p.Supplier?.CompanyName ?? "N/A",
                    p.Price,
                    p.StockLevel,
                    Published = p.IsPublished ? "Yes" : "No"
                }).ToList();

                lblTotal.Text = $"Found: {products.Count} products";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching: {ex.Message}", "Error");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ProductEditForm form = new ProductEditForm(currentUser, null);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Please select a product to edit!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
            Product product = productBLL.GetById(productId);

            ProductEditForm form = new ProductEditForm(currentUser, product);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Please select a product to delete!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Are you sure you want to delete this product?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["ProductID"].Value);
                    productBLL.Delete(productId);
                    MessageBox.Show("Product deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting product: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            LoadProducts();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEdit_Click(sender, e);
            }
        }
    }
}