using System;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace AWEElectronics.Desktop
{
    public partial class ProductEditForm : Form
    {
        private User currentUser;
        private Product currentProduct;
        private ProductBLL productBLL;
        private CategoryBLL categoryBLL;
        private SupplierBLL supplierBLL;
        private bool isEditMode;

        public ProductEditForm(User user, Product product)
        {
            InitializeComponent();
            currentUser = user;
            currentProduct = product;
            productBLL = new ProductBLL();
            categoryBLL = new CategoryBLL();
            supplierBLL = new SupplierBLL();

            isEditMode = (product != null);

            LoadCategories();
            LoadSuppliers();

            if (isEditMode)
            {
                LoadProductData();
                this.Text = "Edit Product - AWE Electronics";
            }
            else
            {
                this.Text = "Add New Product - AWE Electronics";
            }
        }

        private void LoadCategories()
        {
            try
            {
                var categories = categoryBLL.GetAll();
                cboCategory.DataSource = categories;
                cboCategory.DisplayMember = "Name";
                cboCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error");
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                var suppliers = supplierBLL.GetAll();
                cboSupplier.DataSource = suppliers;
                cboSupplier.DisplayMember = "CompanyName";
                cboSupplier.ValueMember = "SupplierID";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error");
            }
        }

        private void LoadProductData()
        {
            txtSKU.Text = currentProduct.SKU;
            txtName.Text = currentProduct.Name;
            txtPrice.Text = currentProduct.Price.ToString("0.00");
            txtStock.Text = currentProduct.StockLevel.ToString();
            txtSpecifications.Text = currentProduct.Specifications;
            chkPublished.Checked = currentProduct.IsPublished;

            cboCategory.SelectedValue = currentProduct.CategoryID;
            if (currentProduct.SupplierID.HasValue)
            {
                cboSupplier.SelectedValue = currentProduct.SupplierID.Value;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                Product product = new Product
                {
                    SKU = txtSKU.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Price = decimal.Parse(txtPrice.Text),
                    StockLevel = int.Parse(txtStock.Text),
                    CategoryID = (int)cboCategory.SelectedValue,
                    SupplierID = cboSupplier.SelectedValue != null ? (int?)cboSupplier.SelectedValue : null,
                    Specifications = txtSpecifications.Text.Trim(),
                    IsPublished = chkPublished.Checked
                };

                if (isEditMode)
                {
                    product.ProductID = currentProduct.ProductID;
                    var result = productBLL.UpdateProduct(product);  // CHANGED: Call UpdateProduct
                    if (result.Success)
                    {
                        MessageBox.Show("Product updated successfully!", "Success",
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
                else
                {
                    var result = productBLL.CreateProduct(product);  // CHANGED: Call CreateProduct
                    if (result.Success)
                    {
                        MessageBox.Show("Product created successfully!", "Success",
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
                MessageBox.Show($"Error saving product: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtSKU.Text))
            {
                MessageBox.Show("Please enter SKU!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSKU.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter product name!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price < 0)
            {
                MessageBox.Show("Please enter a valid price!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Please enter a valid stock level!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStock.Focus();
                return false;
            }

            if (cboCategory.SelectedValue == null)
            {
                MessageBox.Show("Please select a category!", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategory.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}