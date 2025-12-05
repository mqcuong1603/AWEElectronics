using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using AWEElectronics.BLL;
using Desktop.Helpers;
using Desktop.Forms.Users;
using Desktop.Forms.Products;
using Desktop.Forms.Orders;

namespace Desktop.Forms
{
    public partial class MainForm : Form
    {
        private readonly UserBLL _userBLL;
        private readonly ProductBLL _productBLL;
        private readonly OrderBLL _orderBLL;

        private Panel statsPanel;
        private Panel menuPanel;

        public MainForm()
        {
            InitializeComponent();
            _userBLL = new UserBLL();
            _productBLL = new ProductBLL();
            _orderBLL = new OrderBLL();

            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Maximized;
            LoadDashboard();
        }

        private void InitializeComponent()
        {
            this.Text = $"AWE Electronics - Dashboard [{SessionManager.CurrentUser?.Role}]";
            this.Size = new Size(1200, 700);
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Header Panel
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(33, 150, 243)
            };

            Label lblTitle = new Label
            {
                Text = "AWE Electronics Management System",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label lblUser = new Label
            {
                Text = $"Welcome, {SessionManager.CurrentUser?.FullName} ({SessionManager.CurrentUser?.Role})",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                Location = new Point(20, 48),
                AutoSize = true
            };

            Button btnLogout = new Button
            {
                Text = "Logout",
                Location = new Point(1050, 20),
                Size = new Size(120, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(244, 67, 54),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;

            headerPanel.Controls.Add(lblTitle);
            headerPanel.Controls.Add(lblUser);
            headerPanel.Controls.Add(btnLogout);

            // Menu Panel (Left Sidebar)
            menuPanel = new Panel
            {
                Dock = DockStyle.Left,
                Width = 250,
                BackColor = Color.FromArgb(52, 73, 94)
            };

            // Stats Panel (Main Content)
            statsPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Add controls
            this.Controls.Add(statsPanel);
            this.Controls.Add(menuPanel);
            this.Controls.Add(headerPanel);

            CreateMenu();
        }

        private void CreateMenu()
        {
            int yPos = 20;

            // Dashboard Button
            AddMenuButton("Dashboard", yPos, (s, e) => LoadDashboard());
            yPos += 50;

            // Users Menu (Admin only)
            if (SessionManager.CanManageUsers)
            {
                AddMenuButton("Manage Users", yPos, (s, e) => OpenUserManagement());
                yPos += 50;
            }

            // Products Menu (Admin & Staff)
            if (SessionManager.CanManageProducts)
            {
                AddMenuButton("Manage Products", yPos, (s, e) => OpenProductManagement());
                yPos += 50;
            }

            // Orders Menu (Admin, Staff, Accountant)
            if (SessionManager.CanViewOrders)
            {
                AddMenuButton("Manage Orders", yPos, (s, e) => OpenOrderManagement());
                yPos += 50;
            }
        }

        private void AddMenuButton(string text, int yPos, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(10, yPos),
                Size = new Size(230, 40),
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btn.Click += clickHandler;
            menuPanel.Controls.Add(btn);
        }

        private void LoadDashboard()
        {
            statsPanel.Controls.Clear();

            Label lblDashboard = new Label
            {
                Text = $"{SessionManager.CurrentUser?.Role} Dashboard",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(800, 40)
            };
            statsPanel.Controls.Add(lblDashboard);

            try
            {
                // Get statistics based on role
                int yPos = 70;

                if (SessionManager.IsAdmin)
                {
                    LoadAdminStats(ref yPos);
                }
                else if (SessionManager.IsStaff)
                {
                    LoadStaffStats(ref yPos);
                }
                else if (SessionManager.IsAccountant)
                {
                    LoadAccountantStats(ref yPos);
                }
                else
                {
                    Label lblWelcome = new Label
                    {
                        Text = "Welcome to AWE Electronics Management System",
                        Font = new Font("Segoe UI", 14),
                        Location = new Point(10, yPos),
                        AutoSize = true
                    };
                    statsPanel.Controls.Add(lblWelcome);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dashboard: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAdminStats(ref int yPos)
        {
            var users = _userBLL.GetAll();
            var products = _productBLL.GetAll();
            var orders = _orderBLL.GetAll();

            // Users Stats
            AddStatCard("Total Users", users.Count.ToString(),
                $"{users.Count(u => u.Status == "Active")} Active",
                Color.FromArgb(33, 150, 243), 10, yPos);

            // Products Stats
            AddStatCard("Total Products", products.Count.ToString(),
                $"{products.Count(p => p.StockLevel <= 10)} Low Stock",
                Color.FromArgb(76, 175, 80), 260, yPos);

            // Orders Stats
            AddStatCard("Total Orders", orders.Count.ToString(),
                $"{orders.Count(o => o.Status == "Pending")} Pending",
                Color.FromArgb(255, 152, 0), 510, yPos);

            yPos += 150;

            // Recent alerts
            var lowStockProducts = products.Where(p => p.StockLevel <= 10).Take(5).ToList();
            if (lowStockProducts.Any())
            {
                Label lblAlert = new Label
                {
                    Text = "⚠ Low Stock Alert",
                    Font = new Font("Segoe UI", 14, FontStyle.Bold),
                    ForeColor = Color.FromArgb(255, 152, 0),
                    Location = new Point(10, yPos),
                    AutoSize = true
                };
                statsPanel.Controls.Add(lblAlert);
                yPos += 40;

                foreach (var product in lowStockProducts)
                {
                    Label lblProduct = new Label
                    {
                        Text = $"• {product.Name} (SKU: {product.SKU}) - Stock: {product.StockLevel}",
                        Font = new Font("Segoe UI", 10),
                        Location = new Point(20, yPos),
                        AutoSize = true
                    };
                    statsPanel.Controls.Add(lblProduct);
                    yPos += 25;
                }
            }
        }

        private void LoadStaffStats(ref int yPos)
        {
            var products = _productBLL.GetAll();
            var orders = _orderBLL.GetAll();

            AddStatCard("Total Products", products.Count.ToString(),
                $"{products.Count(p => p.StockLevel <= 10)} Low Stock",
                Color.FromArgb(76, 175, 80), 10, yPos);

            AddStatCard("Total Orders", orders.Count.ToString(),
                $"{orders.Count(o => o.Status == "Pending")} Pending",
                Color.FromArgb(255, 152, 0), 260, yPos);

            AddStatCard("Processing Orders", orders.Count(o => o.Status == "Processing").ToString(),
                "Orders in progress",
                Color.FromArgb(33, 150, 243), 510, yPos);
        }

        private void LoadAccountantStats(ref int yPos)
        {
            var orders = _orderBLL.GetAll();
            var completedOrders = orders.Where(o => o.Status == "Completed").ToList();
            var totalRevenue = completedOrders.Sum(o => o.GrandTotal);

            AddStatCard("Total Orders", orders.Count.ToString(),
                $"{orders.Count(o => o.Status == "Completed")} Completed",
                Color.FromArgb(76, 175, 80), 10, yPos);

            AddStatCard("Total Revenue", $"${totalRevenue:N2}",
                "From completed orders",
                Color.FromArgb(33, 150, 243), 260, yPos);

            AddStatCard("Pending Orders", orders.Count(o => o.Status == "Pending").ToString(),
                "Awaiting processing",
                Color.FromArgb(255, 152, 0), 510, yPos);
        }

        private void AddStatCard(string title, string value, string subtitle, Color color, int x, int y)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(230, 120),
                BackColor = color
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                Location = new Point(15, 15),
                AutoSize = true
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 40),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                Location = new Point(15, 85),
                AutoSize = true
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(lblSubtitle);
            statsPanel.Controls.Add(card);
        }

        private void OpenUserManagement()
        {
            UserListForm form = new UserListForm();
            form.ShowDialog();
            LoadDashboard(); // Refresh dashboard
        }

        private void OpenProductManagement()
        {
            ProductListForm form = new ProductListForm();
            form.ShowDialog();
            LoadDashboard(); // Refresh dashboard
        }

        private void OpenOrderManagement()
        {
            OrderListForm form = new OrderListForm();
            form.ShowDialog();
            LoadDashboard(); // Refresh dashboard
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SessionManager.ClearSession();

                // Hide main form
                this.Hide();

                // Show login form again
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();

                // Close the main form (which will exit the app since it's the main form)
                this.Close();
            }
        }
    }
}
