
namespace EmployeeFinder_Server
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
            this.ConsoleBox = new System.Windows.Forms.ListBox();
            this.leftBut = new System.Windows.Forms.Button();
            this.rightBut = new System.Windows.Forms.Button();
            this.DataBox = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.DataBox)).BeginInit();
            this.SuspendLayout();
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.FormattingEnabled = true;
            this.ConsoleBox.Location = new System.Drawing.Point(421, 40);
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.Size = new System.Drawing.Size(367, 342);
            this.ConsoleBox.TabIndex = 6;
            // 
            // leftBut
            // 
            this.leftBut.BackColor = System.Drawing.Color.White;
            this.leftBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftBut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(90)))), ((int)(((byte)(140)))));
            this.leftBut.Location = new System.Drawing.Point(12, 415);
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
            this.rightBut.Location = new System.Drawing.Point(93, 415);
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
            this.DataBox.Location = new System.Drawing.Point(12, 40);
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
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.DataBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox ConsoleBox;
        private System.Windows.Forms.Button leftBut;
        private System.Windows.Forms.Button rightBut;
        private System.Windows.Forms.DataGridView DataBox;
    }
}

