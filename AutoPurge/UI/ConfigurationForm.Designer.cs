namespace AutoPurge
{
    partial class ConfigurationForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grpBoxEmail = new System.Windows.Forms.GroupBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxBcc = new System.Windows.Forms.TextBox();
            this.textBoxCc = new System.Windows.Forms.TextBox();
            this.textBoxTo = new System.Windows.Forms.TextBox();
            this.textBoxFrom = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblBccEmail = new System.Windows.Forms.Label();
            this.lblCcEmail = new System.Windows.Forms.Label();
            this.lblToEmail = new System.Windows.Forms.Label();
            this.lblFromEmail = new System.Windows.Forms.Label();
            this.grpBoxFichiers = new System.Windows.Forms.GroupBox();
            this.btnParcourir = new System.Windows.Forms.Button();
            this.dataGridViewFichiers = new System.Windows.Forms.DataGridView();
            this.Chemin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JoursEnArrière = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormatDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Extensions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Exceptions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpBoxEmail.SuspendLayout();
            this.grpBoxFichiers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFichiers)).BeginInit();
            this.SuspendLayout();
            // 
            // grpBoxEmail
            // 
            this.grpBoxEmail.Controls.Add(this.textBoxPassword);
            this.grpBoxEmail.Controls.Add(this.textBoxBcc);
            this.grpBoxEmail.Controls.Add(this.textBoxCc);
            this.grpBoxEmail.Controls.Add(this.textBoxTo);
            this.grpBoxEmail.Controls.Add(this.textBoxFrom);
            this.grpBoxEmail.Controls.Add(this.lblPassword);
            this.grpBoxEmail.Controls.Add(this.lblBccEmail);
            this.grpBoxEmail.Controls.Add(this.lblCcEmail);
            this.grpBoxEmail.Controls.Add(this.lblToEmail);
            this.grpBoxEmail.Controls.Add(this.lblFromEmail);
            this.grpBoxEmail.ForeColor = System.Drawing.SystemColors.WindowText;
            this.grpBoxEmail.Location = new System.Drawing.Point(26, 23);
            this.grpBoxEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxEmail.Name = "grpBoxEmail";
            this.grpBoxEmail.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxEmail.Size = new System.Drawing.Size(1472, 275);
            this.grpBoxEmail.TabIndex = 0;
            this.grpBoxEmail.TabStop = false;
            this.grpBoxEmail.Text = "Configuration des E-mail";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(917, 50);
            this.textBoxPassword.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(500, 27);
            this.textBoxPassword.TabIndex = 9;
            // 
            // textBoxBcc
            // 
            this.textBoxBcc.Location = new System.Drawing.Point(106, 208);
            this.textBoxBcc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxBcc.Name = "textBoxBcc";
            this.textBoxBcc.Size = new System.Drawing.Size(626, 27);
            this.textBoxBcc.TabIndex = 8;
            // 
            // textBoxCc
            // 
            this.textBoxCc.Location = new System.Drawing.Point(106, 155);
            this.textBoxCc.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxCc.Name = "textBoxCc";
            this.textBoxCc.Size = new System.Drawing.Size(626, 27);
            this.textBoxCc.TabIndex = 7;
            // 
            // textBoxTo
            // 
            this.textBoxTo.Location = new System.Drawing.Point(106, 101);
            this.textBoxTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxTo.Name = "textBoxTo";
            this.textBoxTo.Size = new System.Drawing.Size(626, 27);
            this.textBoxTo.TabIndex = 6;
            // 
            // textBoxFrom
            // 
            this.textBoxFrom.Location = new System.Drawing.Point(106, 50);
            this.textBoxFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.textBoxFrom.Name = "textBoxFrom";
            this.textBoxFrom.Size = new System.Drawing.Size(626, 27);
            this.textBoxFrom.TabIndex = 5;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(830, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(81, 20);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "Password :";
            // 
            // lblBccEmail
            // 
            this.lblBccEmail.AutoSize = true;
            this.lblBccEmail.Location = new System.Drawing.Point(50, 211);
            this.lblBccEmail.Name = "lblBccEmail";
            this.lblBccEmail.Size = new System.Drawing.Size(40, 20);
            this.lblBccEmail.TabIndex = 3;
            this.lblBccEmail.Text = "Bcc :";
            // 
            // lblCcEmail
            // 
            this.lblCcEmail.AutoSize = true;
            this.lblCcEmail.Location = new System.Drawing.Point(50, 159);
            this.lblCcEmail.Name = "lblCcEmail";
            this.lblCcEmail.Size = new System.Drawing.Size(33, 20);
            this.lblCcEmail.TabIndex = 2;
            this.lblCcEmail.Text = "Cc :";
            // 
            // lblToEmail
            // 
            this.lblToEmail.AutoSize = true;
            this.lblToEmail.Location = new System.Drawing.Point(50, 109);
            this.lblToEmail.Name = "lblToEmail";
            this.lblToEmail.Size = new System.Drawing.Size(33, 20);
            this.lblToEmail.TabIndex = 1;
            this.lblToEmail.Text = "To :";
            // 
            // lblFromEmail
            // 
            this.lblFromEmail.AutoSize = true;
            this.lblFromEmail.Location = new System.Drawing.Point(50, 58);
            this.lblFromEmail.Name = "lblFromEmail";
            this.lblFromEmail.Size = new System.Drawing.Size(53, 20);
            this.lblFromEmail.TabIndex = 0;
            this.lblFromEmail.Text = "From :";
            // 
            // grpBoxFichiers
            // 
            this.grpBoxFichiers.Controls.Add(this.btnParcourir);
            this.grpBoxFichiers.Controls.Add(this.dataGridViewFichiers);
            this.grpBoxFichiers.ForeColor = System.Drawing.SystemColors.WindowText;
            this.grpBoxFichiers.Location = new System.Drawing.Point(26, 317);
            this.grpBoxFichiers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxFichiers.Name = "grpBoxFichiers";
            this.grpBoxFichiers.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBoxFichiers.Size = new System.Drawing.Size(1472, 378);
            this.grpBoxFichiers.TabIndex = 1;
            this.grpBoxFichiers.TabStop = false;
            this.grpBoxFichiers.Text = "Configuration des fichiers à supprimer ";
            // 
            // btnParcourir
            // 
            this.btnParcourir.BackColor = System.Drawing.SystemColors.Window;
            this.btnParcourir.Location = new System.Drawing.Point(1354, 37);
            this.btnParcourir.Name = "btnParcourir";
            this.btnParcourir.Size = new System.Drawing.Size(81, 26);
            this.btnParcourir.TabIndex = 1;
            this.btnParcourir.Text = "Parcourir";
            this.btnParcourir.UseVisualStyleBackColor = false;
            this.btnParcourir.Click += new System.EventHandler(this.btnParcourir_Click);
            // 
            // dataGridViewFichiers
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewFichiers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewFichiers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFichiers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Chemin,
            this.JoursEnArrière,
            this.FormatDate,
            this.Extensions,
            this.Nom,
            this.Exceptions});
            this.dataGridViewFichiers.Location = new System.Drawing.Point(26, 37);
            this.dataGridViewFichiers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridViewFichiers.Name = "dataGridViewFichiers";
            this.dataGridViewFichiers.RowHeadersWidth = 51;
            this.dataGridViewFichiers.RowTemplate.Height = 24;
            this.dataGridViewFichiers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFichiers.Size = new System.Drawing.Size(1292, 311);
            this.dataGridViewFichiers.TabIndex = 0;
            // 
            // Chemin
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.Chemin.DefaultCellStyle = dataGridViewCellStyle2;
            this.Chemin.HeaderText = "Chemin";
            this.Chemin.MinimumWidth = 6;
            this.Chemin.Name = "Chemin";
            this.Chemin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Chemin.Width = 537;
            // 
            // JoursEnArrière
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.JoursEnArrière.DefaultCellStyle = dataGridViewCellStyle3;
            this.JoursEnArrière.HeaderText = "JoursEnArrière";
            this.JoursEnArrière.MinimumWidth = 6;
            this.JoursEnArrière.Name = "JoursEnArrière";
            this.JoursEnArrière.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.JoursEnArrière.Width = 107;
            // 
            // FormatDate
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.FormatDate.DefaultCellStyle = dataGridViewCellStyle4;
            this.FormatDate.HeaderText = "FormatDate";
            this.FormatDate.MinimumWidth = 6;
            this.FormatDate.Name = "FormatDate";
            this.FormatDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FormatDate.Width = 118;
            // 
            // Extensions
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.Extensions.DefaultCellStyle = dataGridViewCellStyle5;
            this.Extensions.HeaderText = "Extensions";
            this.Extensions.MinimumWidth = 6;
            this.Extensions.Name = "Extensions";
            this.Extensions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Extensions.Width = 107;
            // 
            // Nom
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.Nom.DefaultCellStyle = dataGridViewCellStyle6;
            this.Nom.HeaderText = "Nom";
            this.Nom.MinimumWidth = 6;
            this.Nom.Name = "Nom";
            this.Nom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Nom.Width = 185;
            // 
            // Exceptions
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            this.Exceptions.DefaultCellStyle = dataGridViewCellStyle7;
            this.Exceptions.HeaderText = "Exceptions";
            this.Exceptions.MinimumWidth = 6;
            this.Exceptions.Name = "Exceptions";
            this.Exceptions.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Exceptions.Width = 185;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.SystemColors.Window;
            this.btnSave.Location = new System.Drawing.Point(635, 719);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(254, 60);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Sauvegarder";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1524, 806);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpBoxFichiers);
            this.Controls.Add(this.grpBoxEmail);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Window;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ConfigurationForm";
            this.Text = "ConfigurationForm";
            this.grpBoxEmail.ResumeLayout(false);
            this.grpBoxEmail.PerformLayout();
            this.grpBoxFichiers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFichiers)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpBoxEmail;
        private System.Windows.Forms.Label lblCcEmail;
        private System.Windows.Forms.Label lblToEmail;
        private System.Windows.Forms.Label lblFromEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblBccEmail;
        private System.Windows.Forms.GroupBox grpBoxFichiers;
        private System.Windows.Forms.DataGridView dataGridViewFichiers;
        private System.Windows.Forms.TextBox textBoxBcc;
        private System.Windows.Forms.TextBox textBoxCc;
        private System.Windows.Forms.TextBox textBoxTo;
        private System.Windows.Forms.TextBox textBoxFrom;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnParcourir;
        private System.Windows.Forms.DataGridViewTextBoxColumn Chemin;
        private System.Windows.Forms.DataGridViewTextBoxColumn JoursEnArrière;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormatDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Extensions;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nom;
        private System.Windows.Forms.DataGridViewTextBoxColumn Exceptions;
    }
}