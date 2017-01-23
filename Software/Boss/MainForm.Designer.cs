namespace BISS.Boss
{
	partial class MainForm
	{
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Windows Form-Designer generierter Code

		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung.
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnSend = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cbMessageType = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// btnSend
			// 
			this.btnSend.Location = new System.Drawing.Point(155, 9);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 43);
			this.btnSend.TabIndex = 0;
			this.btnSend.Text = "Send!";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(114, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Message type to send:";
			// 
			// cbMessageType
			// 
			this.cbMessageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMessageType.FormattingEnabled = true;
			this.cbMessageType.Items.AddRange(new object[] {
            "The items are generated dynamically."});
			this.cbMessageType.Location = new System.Drawing.Point(12, 31);
			this.cbMessageType.Name = "cbMessageType";
			this.cbMessageType.Size = new System.Drawing.Size(114, 21);
			this.cbMessageType.TabIndex = 2;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(242, 63);
			this.Controls.Add(this.cbMessageType);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnSend);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "MainForm";
			this.Text = "Boss";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbMessageType;
	}
}

