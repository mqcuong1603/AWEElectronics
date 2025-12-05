using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AWEElectronics.BLL;
using AWEElectronics.DTO;
using Desktop.Helpers;

namespace Desktop.Forms.Products
{
    public partial class ProductListForm : Form
    {
        private readonly ProductBLL _productBLL;
        private readonly CategoryBLL _categoryBLL;
        private DataGridView dgvProducts;
        private TextBox txtSearch;
        private ComboBox cmbCategory;

        public ProductListForm()
        {
            _productBLL = new ProductBLL();
            _categoryBLL = new CategoryBLL();
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.WindowState = FormWindowState.Maximized;
            LoadCategories();
            LoadProducts();
        }

        private void InitializeComponent()
        {
            this.Text = "Product Management";
            this.Size = new Size(1200, 700);

            // Title
            Label lblTitle = new Label
            {
                Text = "Product Management",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            // Search Panel
            Panel searchPanel = new Panel
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
            txtSearch.TextChanged += (s, e) => FilterProducts();

            Label lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(350, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            cmbCategory = new ComboBox
            {
                Location = new Point(430, 12),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbCategory.SelectedIndexChanged += (s, e) => FilterProducts();

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(650, 10),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadProducts();

            Button btnAdd = new Button
            {
                Text = "+ Add Product",
                Location = new Point(1000, 10),
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
            searchPanel.Controls.Add(lblCategory);
            searchPanel.Controls.Add(cmbCategory);
            searchPanel.Controls.Add(btnRefresh);
            searchPanel.Controls.Add(btnAdd);

            // DataGridView
            dgvProducts = new DataGridView
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
            dgvProducts.CellDoubleClick += DgvProducts_CellDoubleClick;
            dgvProducts.CellClick += DgvProducts_CellClick;
            dgvProducts.CellFormatting += DgvProducts_CellFormatting;

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
            this.Controls.Add(searchPanel);
            this.Controls.Add(dgvProducts);
            this.Controls.Add(btnClose);
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add("All Categories");

                var categories = _categoryBLL.GetAll();
                foreach (var category in categories.Where(c => c.ParentCategoryID == null))
                {
                    cmbCategory.Items.Add(category);
                }

                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                var products = _productBLL.GetAll();

                dgvProducts.DataSource = null;
                dgvProducts.Columns.Clear();

                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ProductID",
                    HeaderText = "ID",
                    Width = 50
                });

                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "SKU",
                    HeaderText = "SKU",
                    Width = 100
                });

                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Name",
                    HeaderText = "Product Name",
                    Width = 250
                });

                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "CategoryName",
                    HeaderText = "Category",
                    Width = 150
                });

                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Price",
                    HeaderText = "Price",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
                });

                dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "StockLevel",
                    HeaderText = "Stock",
                    Width = 80
                });

                dgvProducts.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    DataPropertyName = "IsPublished",
                    HeaderText = "Published",
                    Width = 80
                });

                dgvProducts.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                });

                dgvProducts.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    Width = 70
                });

                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FilterProducts()
        {
            try
            {
                var products = _productBLL.GetAll();
                string searchText = txtSearch.Text.ToLower();

                // Filter by search text
                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    products = products.Where(p =>
                        p.Name.ToLower().Contains(searchText) ||
                        p.SKU.ToLower().Contains(searchText) ||
                        (p.CategoryName != null && p.CategoryName.ToLower().Contains(searchText))
                    ).ToList();
                }

                // Filter by category
                if (cmbCategory.SelectedIndex > 0 && cmbCategory.SelectedItem is Category category)
                {
                    products = products.Where(p => p.CategoryID == category.CategoryID).ToList();
                }

                dgvProducts.DataSource = null;
                dgvProducts.DataSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProducts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Highlight low stock products
            if (dgvProducts.Columns[e.ColumnIndex].DataPropertyName == "StockLevel")
            {
                if (e.Value != null && int.TryParse(e.Value.ToString(), out int stock))
                {
                    if (stock == 0)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(255, 205, 210);
                        e.CellStyle.ForeColor = Color.FromArgb(198, 40, 40);
                    }
                    else if (stock <= 10)
                    {
                        e.CellStyle.BackColor = Color.FromArgb(255, 243, 224);
                        e.CellStyle.ForeColor = Color.FromArgb(245, 124, 0);
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            ProductFormDialog form = new ProductFormDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void DgvProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex < dgvProducts.Columns.Count - 2) // Exclude button columns
            {
                var product = (Product)dgvProducts.Rows[e.RowIndex].DataBoundItem;
                ProductFormDialog form = new ProductFormDialog(product);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var product = (Product)dgvProducts.Rows[e.RowIndex].DataBoundItem;

            if (dgvProducts.Columns[e.ColumnIndex].Name == "Edit")
            {
                ProductFormDialog form = new ProductFormDialog(product);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
            else if (dgvProducts.Columns[e.ColumnIndex].Name == "Delete")
            {
                // Only Admin can delete
                if (!SessionManager.IsAdmin)
                {
                    MessageBox.Show("Only administrators can delete products.", "Access Denied",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Are you sure you want to delete product '{product.Name}'?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var deleteResult = _productBLL.DeleteProduct(product.ProductID);
                    if (deleteResult.Success)
                    {
                        MessageBox.Show(deleteResult.Message, "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
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
