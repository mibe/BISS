using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using BISS.Networking;

namespace BISS.Boss
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			generateMessageTypes();
		}

		private void generateMessageTypes()
		{
			this.cbMessageType.Items.Clear();
			Type type = typeof(MessageType);

			foreach (var name in Enum.GetNames(type))
				if (name != "None")
					this.cbMessageType.Items.Add(name);
		}

		private void btnSend_Click(object sender, EventArgs e)
		{
			string item = (String)this.cbMessageType.SelectedItem;

			if (item == null)
				return;

			MessageType message = (MessageType)Enum.Parse(typeof(MessageType), item);
			Sender s = new Sender();
			Packet packet = PacketBuilder.Instance.Build(message);
			s.Send(packet);

			MessageBox.Show("Message transmitted", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
	}
}
