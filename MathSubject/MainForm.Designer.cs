namespace MathSubject
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.subBox = new System.Windows.Forms.RichTextBox();
            this.btnPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btn_copy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // subBox
            // 
            this.subBox.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.subBox.Location = new System.Drawing.Point(12, 149);
            this.subBox.Name = "subBox";
            this.subBox.Size = new System.Drawing.Size(760, 400);
            this.subBox.TabIndex = 2;
            this.subBox.Text = "";
            // 
            // btnPanel
            // 
            this.btnPanel.AutoScroll = true;
            this.btnPanel.Location = new System.Drawing.Point(12, 12);
            this.btnPanel.Name = "btnPanel";
            this.btnPanel.Size = new System.Drawing.Size(760, 102);
            this.btnPanel.TabIndex = 3;
            // 
            // btn_copy
            // 
            this.btn_copy.Location = new System.Drawing.Point(647, 120);
            this.btn_copy.Name = "btn_copy";
            this.btn_copy.Size = new System.Drawing.Size(125, 23);
            this.btn_copy.TabIndex = 4;
            this.btn_copy.Text = "复制到粘贴板";
            this.btn_copy.UseVisualStyleBackColor = true;
            this.btn_copy.Click += new System.EventHandler(this.btn_copy_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.btn_copy);
            this.Controls.Add(this.btnPanel);
            this.Controls.Add(this.subBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "出数学题";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox subBox;
        private System.Windows.Forms.FlowLayoutPanel btnPanel;
        private System.Windows.Forms.Button btn_copy;
    }
}