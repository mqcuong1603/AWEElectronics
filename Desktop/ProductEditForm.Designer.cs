namespace AWEElectronics.Desktop
{
    partial class ProductEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblSKU;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblSupplier;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.Label lblSpecifications;
        private System.Windows.Forms.TextBox txtSKU;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.TextBox txtSpecifications;
        private System.Windows.Forms.ComboBox cboCategory;
        private System.Windows.Forms.ComboBox cboSupplier;
        private System.Windows.Forms.CheckBox chkPublished;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblSKU = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblSupplier = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblStock = new System.Windows.Forms.Label();
            this.lblSpecifications = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.txtSpecifications = new System.Windows.Forms.TextBox();
            this.cboCategory = new System.Windows.Forms.ComboBox();
            this.cboSupplier = new System.Windows.Forms.ComboBox();
            this.chkPublished = new System.Windows.Forms.CheckBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblSKU
            // 
            this.lblSKU.AutoSize = true;
            this.lblSKU.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSKU.Location = new System.Drawing.Point(30, 30);
            this.lblSKU.Name = "lblSKU";
            this.lblSKU.Size = new System.Drawing.Size(37, 19);
            this.lblSKU.TabIndex = 0;
            this.lblSKU.Text = "SKU:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.Location = new System.Drawing.Point(30, 70);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(48, 19);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblCategory.Location = new System.Drawing.Point(30, 110);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(68, 19);
            this.lblCategory.TabIndex = 2;
            this.lblCategory.Text = "Category:";
            // 
            // lblSupplier
            // 
            this.lblSupplier.AutoSize = true;
            this.lblSupplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSupplier.Location = new System.Drawing.Point(30, 150);
            this.lblSupplier.Name = "lblSupplier";
            this.lblSupplier.Size = new System.Drawing.Size(61, 19);
            this.lblSupplier.TabIndex = 3;
            this.lblSupplier.Text = "Supplier:";
            // 
            // lblPrice
            // 
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrice.Location = new System.Drawing.Point(30, 190);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Size = new System.Drawing.Size(41, 19);
            this.lblPrice.TabIndex = 4;
            this.lblPrice.Text = "Price:";
            // 
            // lblStock
            // 
            this.lblStock.AutoSize = true;
            this.lblStock.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStock.Location = new System.Drawing.Point(30, 230);
            this.lblStock.Name = "lblStock";
            this.lblStock.Size = new System.Drawing.Size(80, 19);
            this.lblStock.TabIndex = 5;
            this.lblStock.Text = "Stock Level:";
            // 
            // lblSpecifications
            // 
            this.lblSpecifications.AutoSize = true;
            this.lblSpecifications.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblSpecifications.Location = new System.Drawing.Point(30, 270);
            this.lblSpecifications.Name = "lblSpecifications";
            this.lblSpecifications.Size = new System.Drawing.Size(93, 19);
            this.lblSpecifications.TabIndex = 6;
            this.lblSpecifications.Text = "Specifications:";
            // 
            // txtSKU
            // 
            this.txtSKU.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSKU.Location = new System.Drawing.Point(160, 27);
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(300, 25);
            this.txtSKU.TabIndex = 7;
            // 
            // txtName
            // 
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtName.Location = new System.Drawing.Point(160, 67);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(300, 25);
            this.txtName.TabIndex = 8;
            // 
            // txtPrice
            // 
            this.txtPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPrice.Location = new System.Drawing.Point(160, 187);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(150, 25);
            this.txtPrice.TabIndex = 11;
            // 
            // txtStock
            // 
            this.txtStock.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtStock.Location = new System.Drawing.Point(160, 227);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(150, 25);
            this.txtStock.TabIndex = 12;
            // 
            // txtSpecifications
            // 
            this.txtSpecifications.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSpecifications.Location = new System.Drawing.Point(160, 267);
            this.txtSpecifications.Multiline = true;
            this.txtSpecifications.Name = "txtSpecifications";
            this.txtSpecifications.Size = new System.Drawing.Size(300, 80);
            this.txtSpecifications.TabIndex = 13;
            // 
            // cboCategory
            // 
            this.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCategory.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboCategory.FormattingEnabled = true;
            this.cboCategory.Location = new System.Drawing.Point(160, 107);
            this.cboCategory.Name = "cboCategory";
            this.cboCategory.Size = new System.Drawing.Size(300, 25);
            this.cboCategory.TabIndex = 9;
            // 
            // cboSupplier
            // 
            this.cboSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSupplier.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboSupplier.FormattingEnabled = true;
            this.cboSupplier.Location = new System.Drawing.Point(160, 147);
            this.cboSupplier.Name = "cboSupplier";
            this.cboSupplier.Size = new System.Drawing.Size(300, 25);
            this.cboSupplier.TabIndex = 10;
            // 
            // chkPublished
            // 
            this.chkPublished.AutoSize = true;
            this.chkPublished.Checked = true;
            this.chkPublished.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPublished.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkPublished.Location = new System.Drawing.Point(160, 365);
            this.chkPublished.Name = "chkPublished";
            this.chkPublished.Size = new System.Drawing.Size(87, 23);
            this.chkPublished.TabIndex = 14;
            this.chkPublished.Text = "Published";
            this.chkPublished.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(160, 410);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 40);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(300, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(120, 40);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ProductEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 481);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkPublished);
            this.Controls.Add(this.cboSupplier);
            this.Controls.Add(this.cboCategory);
            this.Controls.Add(this.txtSpecifications);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtSKU);
            this.Controls.Add(this.lblSpecifications);
            this.Controls.Add(this.lblStock);
            this.Controls.Add(this.lblPrice);
            this.Controls.Add(this.lblSupplier);
            this.Controls.Add(this.lblCategory);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.lblSKU);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ProductEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Product Edit";
            this.Load += new System.EventHandler(this.ProductEditForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}