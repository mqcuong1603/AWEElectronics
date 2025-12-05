namespace AWEElectronics.Desktop
{
    partial class UpdateOrderStatusForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblCurrentStatus;
        private System.Windows.Forms.Label lblNewStatus;
        private System.Windows.Forms.ComboBox cboNewStatus;
        private System.Windows.Forms.Button btnUpdate;
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
            this.lblCurrentStatus = new System.Windows.Forms.Label();
            this.lblNewStatus = new System.Windows.Forms.Label();
            this.cboNewStatus = new System.Windows.Forms.ComboBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCurrentStatus
            // 
            this.lblCurrentStatus.AutoSize = true;
            this.lblCurrentStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCurrentStatus.Location = new System.Drawing.Point(30, 30);
            this.lblCurrentStatus.Name = "lblCurrentStatus";
            this.lblCurrentStatus.Size = new System.Drawing.Size(0, 19);
            this.lblCurrentStatus.TabIndex = 0;
            // 
            // lblNewStatus
            // 
            this.lblNewStatus.AutoSize = true;
            this.lblNewStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblNewStatus.Location = new System.Drawing.Point(30, 70);
            this.lblNewStatus.Name = "lblNewStatus";
            this.lblNewStatus.Size = new System.Drawing.Size(81, 19);
            this.lblNewStatus.TabIndex = 1;
            this.lblNewStatus.Text = "New Status:";
            // 
            // cboNewStatus
            // 
            this.cboNewStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNewStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboNewStatus.FormattingEnabled = true;
            this.cboNewStatus.Location = new System.Drawing.Point(130, 67);
            this.cboNewStatus.Name = "cboNewStatus";
            this.cboNewStatus.Size = new System.Drawing.Size(200, 25);
            this.cboNewStatus.TabIndex = 2;
            // 
            // btnUpdate
            // 
            this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnUpdate.ForeColor = System.Drawing.Color.White;
            this.btnUpdate.Location = new System.Drawing.Point(90, 120);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(100, 35);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(200, 120);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UpdateOrderStatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 181);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.cboNewStatus);
            this.Controls.Add(this.lblNewStatus);
            this.Controls.Add(this.lblCurrentStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdateOrderStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Order Status";
            this.Load += new System.EventHandler(this.UpdateOrderStatusForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}