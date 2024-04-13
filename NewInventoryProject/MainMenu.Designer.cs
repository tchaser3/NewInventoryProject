namespace NewInventoryProject
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Label1 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPerformValuation = new System.Windows.Forms.Button();
            this.btnCheckPrice = new System.Windows.Forms.Button();
            this.btnAbout = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnPartsWithoutPartNumbers = new System.Windows.Forms.Button();
            this.btnTableMissingPartNumbers = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(12, 109);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(540, 83);
            this.Label1.TabIndex = 42;
            this.Label1.Text = "Inventory Valuation Program Main Menu";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(383, 303);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(169, 72);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPerformValuation
            // 
            this.btnPerformValuation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPerformValuation.Location = new System.Drawing.Point(12, 212);
            this.btnPerformValuation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPerformValuation.Name = "btnPerformValuation";
            this.btnPerformValuation.Size = new System.Drawing.Size(169, 72);
            this.btnPerformValuation.TabIndex = 0;
            this.btnPerformValuation.Text = "Perform Valuation";
            this.btnPerformValuation.UseVisualStyleBackColor = true;
            this.btnPerformValuation.Click += new System.EventHandler(this.btnPerformValuation_Click);
            // 
            // btnCheckPrice
            // 
            this.btnCheckPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckPrice.Location = new System.Drawing.Point(383, 212);
            this.btnCheckPrice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCheckPrice.Name = "btnCheckPrice";
            this.btnCheckPrice.Size = new System.Drawing.Size(169, 72);
            this.btnCheckPrice.TabIndex = 2;
            this.btnCheckPrice.Text = "Check Price";
            this.btnCheckPrice.UseVisualStyleBackColor = true;
            this.btnCheckPrice.Click += new System.EventHandler(this.btnCheckPrice_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Location = new System.Drawing.Point(199, 303);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(169, 72);
            this.btnAbout.TabIndex = 4;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NewInventoryProject.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(197, 13);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(171, 90);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 43;
            this.pictureBox1.TabStop = false;
            // 
            // btnPartsWithoutPartNumbers
            // 
            this.btnPartsWithoutPartNumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPartsWithoutPartNumbers.Location = new System.Drawing.Point(197, 212);
            this.btnPartsWithoutPartNumbers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnPartsWithoutPartNumbers.Name = "btnPartsWithoutPartNumbers";
            this.btnPartsWithoutPartNumbers.Size = new System.Drawing.Size(169, 72);
            this.btnPartsWithoutPartNumbers.TabIndex = 1;
            this.btnPartsWithoutPartNumbers.Text = "Parts Without Part Numbers";
            this.btnPartsWithoutPartNumbers.UseVisualStyleBackColor = true;
            this.btnPartsWithoutPartNumbers.Click += new System.EventHandler(this.btnPartsWithoutPartNumbers_Click);
            // 
            // btnTableMissingPartNumbers
            // 
            this.btnTableMissingPartNumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTableMissingPartNumbers.Location = new System.Drawing.Point(12, 303);
            this.btnTableMissingPartNumbers.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTableMissingPartNumbers.Name = "btnTableMissingPartNumbers";
            this.btnTableMissingPartNumbers.Size = new System.Drawing.Size(169, 72);
            this.btnTableMissingPartNumbers.TabIndex = 3;
            this.btnTableMissingPartNumbers.Text = "Table Missing Part Numbers";
            this.btnTableMissingPartNumbers.UseVisualStyleBackColor = true;
            this.btnTableMissingPartNumbers.Click += new System.EventHandler(this.btnTableMissingPartNumbers_Click);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 386);
            this.ControlBox = false;
            this.Controls.Add(this.btnPartsWithoutPartNumbers);
            this.Controls.Add(this.btnTableMissingPartNumbers);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.btnCheckPrice);
            this.Controls.Add(this.btnPerformValuation);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Label1);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Menu";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnPerformValuation;
        internal System.Windows.Forms.Button btnCheckPrice;
        internal System.Windows.Forms.Button btnAbout;
        internal System.Windows.Forms.Button btnPartsWithoutPartNumbers;
        internal System.Windows.Forms.Button btnTableMissingPartNumbers;
    }
}