
namespace courswork
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.AnT = new Tao.Platform.Windows.SimpleOpenGlControl();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RenderTimer = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AnT
            // 
            this.AnT.AccumBits = ((byte)(0));
            this.AnT.AutoCheckErrors = false;
            this.AnT.AutoFinish = false;
            this.AnT.AutoMakeCurrent = true;
            this.AnT.AutoSwapBuffers = true;
            this.AnT.BackColor = System.Drawing.Color.Black;
            this.AnT.ColorBits = ((byte)(32));
            this.AnT.DepthBits = ((byte)(16));
            this.AnT.Location = new System.Drawing.Point(13, 13);
            this.AnT.Margin = new System.Windows.Forms.Padding(4);
            this.AnT.Name = "AnT";
            this.AnT.Size = new System.Drawing.Size(810, 603);
            this.AnT.StencilBits = ((byte)(0));
            this.AnT.TabIndex = 1;
            this.AnT.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AnT_KeyDown);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Главная позиция",
            "Полёт на дирижабле",
            "Ракурс полёта"});
            this.comboBox1.Location = new System.Drawing.Point(831, 331);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(218, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(831, 312);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Позиция камеры";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(832, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Управление";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(832, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 26);
            this.label3.TabIndex = 4;
            this.label3.Text = "  W\r\nA S D - управление камерой";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(832, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Q, E - поворот камеры";
            // 
            // RenderTimer
            // 
            this.RenderTimer.Interval = 30;
            this.RenderTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(835, 151);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "B, N - поворот камеры вверх/вниз";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(832, 185);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 39);
            this.label10.TabIndex = 11;
            this.label10.Text = "Дирижабль:\r\nK - назад/лево\r\nL - вперёд/право";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.DarkRed;
            this.button1.Location = new System.Drawing.Point(830, 446);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(267, 170);
            this.button1.TabIndex = 12;
            this.button1.Text = "Полёт самолёта";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(827, 238);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(255, 39);
            this.label7.TabIndex = 13;
            this.label7.Text = "Самолёт в ангаре (Только когда полоса пустая):\r\nF1 - Вперёд \r\nF2 - Назад. Eсли уж" +
    "е в воздухе, то нельзя.";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1109, 628);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.AnT);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Аэропорт им. Райана Гослинга";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl AnT;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer RenderTimer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label7;
    }
}

