namespace AutoColorScheme
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            darkSchemeTimeChooser = new ComboBox();
            lightSchemeTimeChooser = new ComboBox();
            label2 = new Label();
            bottomLabel = new Label();
            schemeChangeTimer = new System.Windows.Forms.Timer(components);
            panel1 = new Panel();
            autoStartCheckBox = new CheckBox();
            tray = new NotifyIcon(components);
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.Location = new Point(185, 8);
            label1.Name = "label1";
            label1.Size = new Size(114, 15);
            label1.TabIndex = 0;
            label1.Text = "🌑 Kiedy ciemno?";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // darkSchemeTimeChooser
            // 
            darkSchemeTimeChooser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            darkSchemeTimeChooser.DropDownHeight = 180;
            darkSchemeTimeChooser.DropDownStyle = ComboBoxStyle.DropDownList;
            darkSchemeTimeChooser.FormattingEnabled = true;
            darkSchemeTimeChooser.IntegralHeight = false;
            darkSchemeTimeChooser.Location = new Point(185, 26);
            darkSchemeTimeChooser.Name = "darkSchemeTimeChooser";
            darkSchemeTimeChooser.Size = new Size(114, 23);
            darkSchemeTimeChooser.TabIndex = 1;
            // 
            // lightSchemeTimeChooser
            // 
            lightSchemeTimeChooser.DropDownHeight = 180;
            lightSchemeTimeChooser.DropDownStyle = ComboBoxStyle.DropDownList;
            lightSchemeTimeChooser.FormattingEnabled = true;
            lightSchemeTimeChooser.IntegralHeight = false;
            lightSchemeTimeChooser.Location = new Point(12, 26);
            lightSchemeTimeChooser.Name = "lightSchemeTimeChooser";
            lightSchemeTimeChooser.Size = new Size(114, 23);
            lightSchemeTimeChooser.TabIndex = 3;
            // 
            // label2
            // 
            label2.Location = new Point(12, 8);
            label2.Name = "label2";
            label2.Size = new Size(114, 15);
            label2.TabIndex = 2;
            label2.Text = "☀️ Kiedy jasno?";
            // 
            // bottomLabel
            // 
            bottomLabel.Dock = DockStyle.Bottom;
            bottomLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 238);
            bottomLabel.Location = new Point(0, 87);
            bottomLabel.Name = "bottomLabel";
            bottomLabel.Size = new Size(311, 23);
            bottomLabel.TabIndex = 4;
            bottomLabel.Text = "Przełączenie trybu może spóźnić się do 10 minut.";
            bottomLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // schemeChangeTimer
            // 
            schemeChangeTimer.Enabled = true;
            schemeChangeTimer.Interval = 600000;
            schemeChangeTimer.Tick += schemeChangeTimer_Tick;
            // 
            // panel1
            // 
            panel1.Controls.Add(autoStartCheckBox);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(darkSchemeTimeChooser);
            panel1.Controls.Add(lightSchemeTimeChooser);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(311, 87);
            panel1.TabIndex = 5;
            // 
            // autoStartCheckBox
            // 
            autoStartCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            autoStartCheckBox.AutoSize = true;
            autoStartCheckBox.Location = new Point(12, 65);
            autoStartCheckBox.Name = "autoStartCheckBox";
            autoStartCheckBox.Size = new Size(75, 19);
            autoStartCheckBox.TabIndex = 4;
            autoStartCheckBox.Text = "Autostart";
            autoStartCheckBox.UseVisualStyleBackColor = true;
            autoStartCheckBox.CheckedChanged += autoStartCheckBox_CheckedChanged;
            // 
            // tray
            // 
            tray.Icon = (Icon)resources.GetObject("tray.Icon");
            tray.Text = "Auto Color Scheme";
            tray.Visible = true;
            tray.Click += tray_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(311, 110);
            Controls.Add(panel1);
            Controls.Add(bottomLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Opacity = 0D;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Color Scheme";
            Activated += Form1_Activated;
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
        private ComboBox darkSchemeTimeChooser;
        private ComboBox lightSchemeTimeChooser;
        private Label label2;
        private Label bottomLabel;
        private System.Windows.Forms.Timer schemeChangeTimer;
        private Panel panel1;
        private NotifyIcon tray;
        private CheckBox autoStartCheckBox;
    }
}
