using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
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

			this.btnSend.Tag = this.btnSend.Text;
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
			InterfaceSender s = new InterfaceSender();
			Packet packet = PacketBuilder.Instance.Build(message);
			s.Send(packet);

			this.btnSend.Text = "Done!";

			Task.Delay(5000).ContinueWith(x => this.btnSend.Text = this.btnSend.Tag as String);
		}
	}
}
