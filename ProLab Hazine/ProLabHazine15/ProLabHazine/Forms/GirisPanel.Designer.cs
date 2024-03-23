namespace ProLabHazine
{
    partial class GirisPanel
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
            this.btnCreateNewMap = new System.Windows.Forms.Button();
            this.txtBoxMapXAxis = new System.Windows.Forms.TextBox();
            this.txtBoxMapYAxis = new System.Windows.Forms.TextBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateNewMap
            // 
            this.btnCreateNewMap.Location = new System.Drawing.Point(52, 111);
            this.btnCreateNewMap.Name = "btnCreateNewMap";
            this.btnCreateNewMap.Size = new System.Drawing.Size(165, 23);
            this.btnCreateNewMap.TabIndex = 2;
            this.btnCreateNewMap.Text = "Yeni Harita Oluştur";
            this.btnCreateNewMap.UseVisualStyleBackColor = true;
            this.btnCreateNewMap.Click += new System.EventHandler(this.btnCreateNewMap_Click);
            // 
            // txtBoxMapXAxis
            // 
            this.txtBoxMapXAxis.Location = new System.Drawing.Point(52, 21);
            this.txtBoxMapXAxis.Name = "txtBoxMapXAxis";
            this.txtBoxMapXAxis.Size = new System.Drawing.Size(165, 22);
            this.txtBoxMapXAxis.TabIndex = 0;
            // 
            // txtBoxMapYAxis
            // 
            this.txtBoxMapYAxis.Location = new System.Drawing.Point(52, 65);
            this.txtBoxMapYAxis.Name = "txtBoxMapYAxis";
            this.txtBoxMapYAxis.Size = new System.Drawing.Size(165, 22);
            this.txtBoxMapYAxis.TabIndex = 1;
            // 
            // btnStartGame
            // 
            this.btnStartGame.Location = new System.Drawing.Point(52, 153);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(165, 23);
            this.btnStartGame.TabIndex = 3;
            this.btnStartGame.Text = "Başlat";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // GirisPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 193);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.txtBoxMapYAxis);
            this.Controls.Add(this.txtBoxMapXAxis);
            this.Controls.Add(this.btnCreateNewMap);
            this.Name = "GirisPanel";
            this.Text = "GirisPanel";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateNewMap;
        private System.Windows.Forms.TextBox txtBoxMapXAxis;
        private System.Windows.Forms.TextBox txtBoxMapYAxis;
        private System.Windows.Forms.Button btnStartGame;
    }
}