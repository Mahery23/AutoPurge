namespace AutoPurge
{
    partial class MainForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpBoxBoutons = new System.Windows.Forms.GroupBox();
            this.btnStartPurge = new System.Windows.Forms.Button();
            this.btnConfigurePurge = new System.Windows.Forms.Button();
            this.grpBoxBoutons.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Gainsboro;
            this.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(124, 46);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(541, 57);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Gestion de la Purge de Fichiers";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpBoxBoutons
            // 
            this.grpBoxBoutons.Controls.Add(this.btnConfigurePurge);
            this.grpBoxBoutons.Controls.Add(this.btnStartPurge);
            this.grpBoxBoutons.Location = new System.Drawing.Point(82, 154);
            this.grpBoxBoutons.Name = "grpBoxBoutons";
            this.grpBoxBoutons.Size = new System.Drawing.Size(627, 243);
            this.grpBoxBoutons.TabIndex = 1;
            this.grpBoxBoutons.TabStop = false;
            this.grpBoxBoutons.Text = "Gérez votre purge de fichiers : exécutez-la ou configurez-la.";
            // 
            // btnStartPurge
            // 
            this.btnStartPurge.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnStartPurge.FlatAppearance.BorderSize = 0;
            this.btnStartPurge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartPurge.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartPurge.ForeColor = System.Drawing.SystemColors.Window;
            this.btnStartPurge.Location = new System.Drawing.Point(51, 97);
            this.btnStartPurge.Name = "btnStartPurge";
            this.btnStartPurge.Size = new System.Drawing.Size(238, 54);
            this.btnStartPurge.TabIndex = 0;
            this.btnStartPurge.Text = "Lancer la purge";
            this.btnStartPurge.UseVisualStyleBackColor = false;
            this.btnStartPurge.Click += new System.EventHandler(this.btnStartPurge_Click);
            // 
            // btnConfigurePurge
            // 
            this.btnConfigurePurge.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnConfigurePurge.FlatAppearance.BorderSize = 0;
            this.btnConfigurePurge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfigurePurge.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfigurePurge.ForeColor = System.Drawing.SystemColors.Window;
            this.btnConfigurePurge.Location = new System.Drawing.Point(336, 97);
            this.btnConfigurePurge.Name = "btnConfigurePurge";
            this.btnConfigurePurge.Size = new System.Drawing.Size(238, 54);
            this.btnConfigurePurge.TabIndex = 1;
            this.btnConfigurePurge.Text = "Configurer la purge";
            this.btnConfigurePurge.UseVisualStyleBackColor = false;
            this.btnConfigurePurge.Click += new System.EventHandler(this.btnConfigurePurge_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grpBoxBoutons);
            this.Controls.Add(this.lblTitle);
            this.Name = "MainForm";
            this.Text = "AutoPurge";
            this.grpBoxBoutons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpBoxBoutons;
        private System.Windows.Forms.Button btnConfigurePurge;
        private System.Windows.Forms.Button btnStartPurge;
    }
}