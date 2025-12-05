using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;

namespace Desktop.Forms.Products
{
    public partial class ProductFormDialog : Form
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private readonly SupplierBLL _supplierBLL;
        private readonly Product _product;
        private readonly bool _isEditMode;

        private TextBox txtSKU;
        private TextBox txtName;
        private TextBox txtSpecifications;
        private NumericUpDown numPrice;
        private NumericUpDown numStock;
        private ComboBox cmbCategory;
        private ComboBox cmbSupplier;
        private CheckBox chkPublished;

        public ProductFormDialog(Product product = null)
        {
            InitializeComponent();
            _productBLL = new ProductBLL();
            _categoryBLL = new CategoryBLL();
            _supplierBLL = new SupplierBLL();
            _product = product;
            _isEditMode = product != null;

            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            LoadCategories();
            LoadSuppliers();

            if (_isEditMode)
            {
                LoadProductData();
            }
        }

        private void InitializeComponent()
        {
            this.Text = _isEditMode ? "Edit Product" : "Add New Product";
            this.Size = new Size(550, 580);

            // Title
            Label lblTitle = new Label
            {
                Text = _isEditMode ? "Edit Product" : "Add New Product",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // SKU
            Label lblSKU = new Label
            {
                Text = "SKU:",
                Location = new Point(30, 80),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtSKU = new TextBox
            {
                Location = new Point(180, 78),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Name
            Label lblName = new Label
            {
                Text = "Product Name:",
                Location = new Point(30, 120),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtName = new TextBox
            {
                Location = new Point(180, 118),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10)
            };

            // Specifications
            Label lblSpecifications = new Label
            {
                Text = "Specifications:",
                Location = new Point(30, 160),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            txtSpecifications = new TextBox
            {
                Location = new Point(180, 158),
                Size = new Size(330, 80),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // Category
            Label lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(30, 250),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(180, 248),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Supplier
            Label lblSupplier = new Label
            {
                Text = "Supplier:",
                Location = new Point(30, 290),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            cmbSupplier = new ComboBox
            {
                Location = new Point(180, 288),
                Size = new Size(330, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Price
            Label lblPrice = new Label
            {
                Text = "Price ($):",
                Location = new Point(30, 330),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            numPrice = new NumericUpDown
            {
                Location = new Point(180, 328),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                DecimalPlaces = 2,
                Maximum = 999999,
                Minimum = 0
            };

            // Stock Level
            Label lblStock = new Label
            {
                Text = "Stock Level:",
                Location = new Point(30, 370),
                Size = new Size(120, 25),
                Font = new Font("Segoe UI", 10)
            };

            numStock = new NumericUpDown
            {
                Location = new Point(180, 368),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                Maximum = 999999,
                Minimum = 0
            };

            // Published
            chkPublished = new CheckBox
            {
                Text = "Published (visible to customers)",
                Location = new Point(180, 410),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10),
                Checked = true
            };

            // Save Button
            Button btnSave = new Button
            {
                Text = "Save",
                Location = new Point(300, 470),
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
                Location = new Point(410, 470),
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
            this.Controls.Add(lblSKU);
            this.Controls.Add(txtSKU);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblSpecifications);
            this.Controls.Add(txtSpecifications);
            this.Controls.Add(lblCategory);
            this.Controls.Add(cmbCategory);
            this.Controls.Add(lblSupplier);
            this.Controls.Add(cmbSupplier);
            this.Controls.Add(lblPrice);
            this.Controls.Add(numPrice);
            this.Controls.Add(lblStock);
            this.Controls.Add(numStock);
            this.Controls.Add(chkPublished);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.Items.Clear();
                var categories = _categoryBLL.GetAll();

                foreach (var category in categories)
                {
                    cmbCategory.Items.Add(category);
                }

                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "CategoryID";

                if (cmbCategory.Items.Count > 0)
                    cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSuppliers()
        {
            try
            {
                cmbSupplier.Items.Clear();
                cmbSupplier.Items.Add("-- No Supplier --");

                var suppliers = _supplierBLL.GetAll();
                foreach (var supplier in suppliers)
                {
                    cmbSupplier.Items.Add(supplier);
                }

                cmbSupplier.DisplayMember = "CompanyName";
                cmbSupplier.ValueMember = "SupplierID";
                cmbSupplier.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProductData()
        {
            if (_product != null)
            {
                txtSKU.Text = _product.SKU;
                txtName.Text = _product.Name;
                txtSpecifications.Text = _product.Specifications;
                numPrice.Value = _product.Price;
                numStock.Value = _product.StockLevel;
                chkPublished.Checked = _product.IsPublished;

                // Set category
                for (int i = 0; i < cmbCategory.Items.Count; i++)
                {
                    if (cmbCategory.Items[i] is Category cat && cat.CategoryID == _product.CategoryID)
                    {
                        cmbCategory.SelectedIndex = i;
                        break;
                    }
                }

                // Set supplier
                if (_product.SupplierID.HasValue)
                {
                    for (int i = 1; i < cmbSupplier.Items.Count; i++)
                    {
                        if (cmbSupplier.Items[i] is Supplier sup && sup.SupplierID == _product.SupplierID.Value)
                        {
                            cmbSupplier.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(txtSKU.Text))
                {
                    MessageBox.Show("Please enter SKU.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtSKU.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter product name.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (cmbCategory.SelectedItem == null)
                {
                    MessageBox.Show("Please select a category.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get supplier ID
                int? supplierId = null;
                if (cmbSupplier.SelectedIndex > 0 && cmbSupplier.SelectedItem is Supplier supplier)
                {
                    supplierId = supplier.SupplierID;
                }

                var category = (Category)cmbCategory.SelectedItem;

                if (_isEditMode)
                {
                    // Update product
                    _product.SKU = txtSKU.Text.Trim();
                    _product.Name = txtName.Text.Trim();
                    _product.Specifications = txtSpecifications.Text.Trim();
                    _product.CategoryID = category.CategoryID;
                    _product.SupplierID = supplierId;
                    _product.Price = numPrice.Value;
                    _product.StockLevel = (int)numStock.Value;
                    _product.IsPublished = chkPublished.Checked;

                    var result = _productBLL.UpdateProduct(_product);

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
                else
                {
                    // Create new product
                    var product = new Product
                    {
                        SKU = txtSKU.Text.Trim(),
                        Name = txtName.Text.Trim(),
                        Specifications = txtSpecifications.Text.Trim(),
                        CategoryID = category.CategoryID,
                        SupplierID = supplierId,
                        Price = numPrice.Value,
                        StockLevel = (int)numStock.Value,
                        IsPublished = chkPublished.Checked
                    };

                    var result = _productBLL.CreateProduct(product);

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
