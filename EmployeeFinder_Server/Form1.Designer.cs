
namespace ToDoListServer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.IpBox = new System.Windows.Forms.TextBox();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StartBut = new System.Windows.Forms.Button();
            this.ConsoleBox = new System.Windows.Forms.ListBox();
            this.leftBut = new System.Windows.Forms.Button();
            this.rightBut = new System.Windows.Forms.Button();
            this.DataBox = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataBox)).BeginInit();
            this.SuspendLayout();
            // 
            // IpBox
            // 
            this.IpBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.IpBox.Location = new System.Drawing.Point(315, 23);
            this.IpBox.Name = "IpBox";
            this.IpBox.Size = new System.Drawing.Size(100, 23);
            this.IpBox.TabIndex = 1;
            this.IpBox.Text = "127.0.0.1";
            // 
            // PortBox
            // 
            this.PortBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.PortBox.Location = new System.Drawing.Point(468, 23);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(100, 23);
            this.PortBox.TabIndex = 2;
            this.PortBox.Text = "1024";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(421, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 19);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(281, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 19);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ip:";
            // 
            // StartBut
            // 
            this.StartBut.BackColor = System.Drawing.Color.White;
            this.StartBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartBut.Font = new System.Drawing.Font("Century Gothic", 8.25F);
            this.StartBut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this.StartBut.Location = new System.Drawing.Point(285, 52);
            this.StartBut.Name = "StartBut";
            this.StartBut.Size = new System.Drawing.Size(283, 23);
            this.StartBut.TabIndex = 5;
            this.StartBut.Text = "Start server";
            this.StartBut.UseVisualStyleBackColor = false;
            this.StartBut.Click += new System.EventHandler(this.StartBut_Click);
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.FormattingEnabled = true;
            this.ConsoleBox.Location = new System.Drawing.Point(421, 98);
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.Size = new System.Drawing.Size(367, 342);
            this.ConsoleBox.TabIndex = 6;
            // 
            // leftBut
            // 
            this.leftBut.BackColor = System.Drawing.Color.White;
            this.leftBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftBut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this.leftBut.Location = new System.Drawing.Point(12, 69);
            this.leftBut.Name = "leftBut";
            this.leftBut.Size = new System.Drawing.Size(75, 23);
            this.leftBut.TabIndex = 8;
            this.leftBut.Text = "<";
            this.leftBut.UseVisualStyleBackColor = false;
            this.leftBut.Click += new System.EventHandler(this.leftBut_Click);
            // 
            // rightBut
            // 
            this.rightBut.BackColor = System.Drawing.Color.White;
            this.rightBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rightBut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this.rightBut.Location = new System.Drawing.Point(93, 69);
            this.rightBut.Name = "rightBut";
            this.rightBut.Size = new System.Drawing.Size(75, 23);
            this.rightBut.TabIndex = 9;
            this.rightBut.Text = ">";
            this.rightBut.UseVisualStyleBackColor = false;
            this.rightBut.Click += new System.EventHandler(this.rightBut_Click);
            // 
            // DataBox
            // 
            this.DataBox.BackgroundColor = System.Drawing.Color.White;
            this.DataBox.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataBox.Location = new System.Drawing.Point(12, 98);
            this.DataBox.Name = "DataBox";
            this.DataBox.Size = new System.Drawing.Size(404, 342);
            this.DataBox.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DataBox);
            this.Controls.Add(this.rightBut);
            this.Controls.Add(this.leftBut);
            this.Controls.Add(this.ConsoleBox);
            this.Controls.Add(this.StartBut);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PortBox);
            this.Controls.Add(this.IpBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.DataBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox IpBox;
        private System.Windows.Forms.TextBox PortBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button StartBut;
        private System.Windows.Forms.ListBox ConsoleBox;
        private System.Windows.Forms.Button leftBut;
        private System.Windows.Forms.Button rightBut;
        private System.Windows.Forms.DataGridView DataBox;
    }
}

