namespace _444
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.porogN = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.porogK = new System.Windows.Forms.TextBox();
            this.testRB = new System.Windows.Forms.RadioButton();
            this.serverRB = new System.Windows.Forms.RadioButton();
            this.kompRB = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(519, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(92, 26);
            this.button1.TabIndex = 4;
            this.button1.Text = "Собрать граф";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(351, 400);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = "";
            // 
            // button4
            // 
            this.button4.Enabled = false;
            this.button4.Location = new System.Drawing.Point(562, 221);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 105);
            this.button4.TabIndex = 10;
            this.button4.Text = "Запустить алгоритм";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(468, 221);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(55, 20);
            this.textBox3.TabIndex = 11;
            this.textBox3.Text = "32498";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(382, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Вершина";
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(519, 117);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(118, 57);
            this.button5.TabIndex = 13;
            this.button5.Text = "Переместить граф из файла в массив + Сериализация";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // porogN
            // 
            this.porogN.Location = new System.Drawing.Point(468, 279);
            this.porogN.Name = "porogN";
            this.porogN.Size = new System.Drawing.Size(55, 20);
            this.porogN.TabIndex = 14;
            this.porogN.Text = "0,1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(381, 282);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Порог для N";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(468, 250);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(55, 20);
            this.textBox5.TabIndex = 16;
            this.textBox5.Text = "3";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(381, 253);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Проходов";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(535, 342);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 70);
            this.button2.TabIndex = 18;
            this.button2.Text = "Преобразовать ПО в статьи";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(384, 357);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(94, 55);
            this.button3.TabIndex = 19;
            this.button3.Text = "Визуализация";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(382, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 21;
            this.label1.Text = "Порог для К";
            // 
            // porogK
            // 
            this.porogK.Location = new System.Drawing.Point(468, 306);
            this.porogK.Name = "porogK";
            this.porogK.Size = new System.Drawing.Size(55, 20);
            this.porogK.TabIndex = 20;
            this.porogK.Text = "0,1";
            // 
            // testRB
            // 
            this.testRB.AutoSize = true;
            this.testRB.Location = new System.Drawing.Point(6, 31);
            this.testRB.Name = "testRB";
            this.testRB.Size = new System.Drawing.Size(49, 17);
            this.testRB.TabIndex = 23;
            this.testRB.TabStop = true;
            this.testRB.Text = "Тест";
            this.testRB.UseVisualStyleBackColor = true;
            this.testRB.CheckedChanged += new System.EventHandler(this.testRB_CheckedChanged);
            // 
            // serverRB
            // 
            this.serverRB.AutoSize = true;
            this.serverRB.Location = new System.Drawing.Point(6, 54);
            this.serverRB.Name = "serverRB";
            this.serverRB.Size = new System.Drawing.Size(62, 17);
            this.serverRB.TabIndex = 24;
            this.serverRB.TabStop = true;
            this.serverRB.Text = "Сервер";
            this.serverRB.UseVisualStyleBackColor = true;
            this.serverRB.CheckedChanged += new System.EventHandler(this.serverRB_CheckedChanged);
            // 
            // kompRB
            // 
            this.kompRB.AutoSize = true;
            this.kompRB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.kompRB.Location = new System.Drawing.Point(6, 77);
            this.kompRB.Name = "kompRB";
            this.kompRB.Size = new System.Drawing.Size(61, 18);
            this.kompRB.TabIndex = 25;
            this.kompRB.Text = " Комп";
            this.kompRB.UseVisualStyleBackColor = true;
            this.kompRB.CheckedChanged += new System.EventHandler(this.kompRB_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.kompRB);
            this.groupBox1.Controls.Add(this.serverRB);
            this.groupBox1.Controls.Add(this.testRB);
            this.groupBox1.Location = new System.Drawing.Point(384, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(129, 110);
            this.groupBox1.TabIndex = 26;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Режим работы";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(550, 102);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "или";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 429);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(617, 23);
            this.progressBar1.TabIndex = 29;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(519, 178);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(111, 23);
            this.button6.TabIndex = 30;
            this.button6.Text = "Десериализация";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(363, 13);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 31;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(445, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(192, 20);
            this.textBox1.TabIndex = 32;
            this.textBox1.Text = "https://ru.wikipedia.org/wiki/Категория:Звёзды";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 464);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.porogK);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.porogN);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox porogN;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox porogK;
        private System.Windows.Forms.RadioButton testRB;
        private System.Windows.Forms.RadioButton serverRB;
        private System.Windows.Forms.RadioButton kompRB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox textBox1;
    }
}

