namespace Question_MAnager2
{
    partial class GetItem
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
            this.listOfGuids = new System.Windows.Forms.ListView();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listOfGuids
            // 
            this.listOfGuids.HideSelection = false;
            this.listOfGuids.Location = new System.Drawing.Point(12, 12);
            this.listOfGuids.Name = "listOfGuids";
            this.listOfGuids.Size = new System.Drawing.Size(264, 189);
            this.listOfGuids.TabIndex = 0;
            this.listOfGuids.UseCompatibleStateImageBehavior = false;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 228);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // GetQuestion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 263);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.listOfGuids);
            this.Name = "GetQuestion";
            this.Text = "GetQuestion";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listOfGuids;
        private System.Windows.Forms.Button btnOk;
    }
}