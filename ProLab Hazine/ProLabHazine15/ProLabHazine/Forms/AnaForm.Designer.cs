namespace ProLabHazine
{
    partial class AnaForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pctrBoxMap = new System.Windows.Forms.PictureBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblMinimunStepValue = new System.Windows.Forms.Label();
            this.lblLastTresureStepValue = new System.Windows.Forms.Label();
            this.lblCollectedLastTresure = new System.Windows.Forms.Label();
            this.lblCollectedTresuresNumber = new System.Windows.Forms.Label();
            this.lblTotalStepValue = new System.Windows.Forms.Label();
            this.lstBoxGameObjects = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctrBoxMap)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1750, 950);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pctrBoxMap);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1742, 921);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pctrBoxMap
            // 
            this.pctrBoxMap.Location = new System.Drawing.Point(6, 6);
            this.pctrBoxMap.Name = "pctrBoxMap";
            this.pctrBoxMap.Size = new System.Drawing.Size(1241, 468);
            this.pctrBoxMap.TabIndex = 0;
            this.pctrBoxMap.TabStop = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblMinimunStepValue);
            this.tabPage2.Controls.Add(this.lblLastTresureStepValue);
            this.tabPage2.Controls.Add(this.lblCollectedLastTresure);
            this.tabPage2.Controls.Add(this.lblCollectedTresuresNumber);
            this.tabPage2.Controls.Add(this.lblTotalStepValue);
            this.tabPage2.Controls.Add(this.lstBoxGameObjects);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1742, 921);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblMinimunStepValue
            // 
            this.lblMinimunStepValue.AutoSize = true;
            this.lblMinimunStepValue.Location = new System.Drawing.Point(1026, 137);
            this.lblMinimunStepValue.Name = "lblMinimunStepValue";
            this.lblMinimunStepValue.Size = new System.Drawing.Size(133, 16);
            this.lblMinimunStepValue.TabIndex = 5;
            this.lblMinimunStepValue.Text = "lblMinimunStepValue";
            // 
            // lblLastTresureStepValue
            // 
            this.lblLastTresureStepValue.AutoSize = true;
            this.lblLastTresureStepValue.Location = new System.Drawing.Point(1025, 109);
            this.lblLastTresureStepValue.Name = "lblLastTresureStepValue";
            this.lblLastTresureStepValue.Size = new System.Drawing.Size(156, 16);
            this.lblLastTresureStepValue.TabIndex = 4;
            this.lblLastTresureStepValue.Text = "lblLastTresureStepValue";
            // 
            // lblCollectedLastTresure
            // 
            this.lblCollectedLastTresure.AutoSize = true;
            this.lblCollectedLastTresure.Location = new System.Drawing.Point(1026, 80);
            this.lblCollectedLastTresure.Name = "lblCollectedLastTresure";
            this.lblCollectedLastTresure.Size = new System.Drawing.Size(150, 16);
            this.lblCollectedLastTresure.TabIndex = 3;
            this.lblCollectedLastTresure.Text = "lblCollectedLastTresure";
            // 
            // lblCollectedTresuresNumber
            // 
            this.lblCollectedTresuresNumber.AutoSize = true;
            this.lblCollectedTresuresNumber.Location = new System.Drawing.Point(1026, 53);
            this.lblCollectedTresuresNumber.Name = "lblCollectedTresuresNumber";
            this.lblCollectedTresuresNumber.Size = new System.Drawing.Size(132, 16);
            this.lblCollectedTresuresNumber.TabIndex = 2;
            this.lblCollectedTresuresNumber.Text = "lblCollectedTresures";
            // 
            // lblTotalStepValue
            // 
            this.lblTotalStepValue.AutoSize = true;
            this.lblTotalStepValue.Location = new System.Drawing.Point(1026, 27);
            this.lblTotalStepValue.Name = "lblTotalStepValue";
            this.lblTotalStepValue.Size = new System.Drawing.Size(115, 16);
            this.lblTotalStepValue.TabIndex = 1;
            this.lblTotalStepValue.Text = "lblTotalStepValue";
            // 
            // lstBoxGameObjects
            // 
            this.lstBoxGameObjects.FormattingEnabled = true;
            this.lstBoxGameObjects.ItemHeight = 16;
            this.lstBoxGameObjects.Location = new System.Drawing.Point(67, 27);
            this.lstBoxGameObjects.Name = "lstBoxGameObjects";
            this.lstBoxGameObjects.Size = new System.Drawing.Size(952, 276);
            this.lstBoxGameObjects.TabIndex = 0;
            // 
            // AnaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1782, 953);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AnaForm";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pctrBoxMap)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.PictureBox pctrBoxMap;
        private System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.ListBox lstBoxGameObjects;
        public System.Windows.Forms.Label lblTotalStepValue;
        public System.Windows.Forms.Label lblCollectedTresuresNumber;
        public System.Windows.Forms.Label lblCollectedLastTresure;
        public System.Windows.Forms.Label lblLastTresureStepValue;
        public System.Windows.Forms.Label lblMinimunStepValue;
    }
}

