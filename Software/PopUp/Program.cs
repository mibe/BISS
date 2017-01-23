using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using BISS.Networking;

namespace BISS.PopUp
{
	static class Program
	{
		static bool close;
		static NotifyIcon notifyIcon;
		static FilteredReceiver receiver;

		/// <summary>
		/// Der Haupteinstiegspunkt für die Anwendung.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			receiver = new FilteredReceiver();
			receiver.PacketReceived += receiver_PacketReceived;
			
			ContextMenu menu = new ContextMenu();
			MenuItem itemExit = new MenuItem("E&xit", exitMenu_Click);
			menu.MenuItems.Add(itemExit);

			notifyIcon = new NotifyIcon(new Container());
			notifyIcon.ContextMenu = menu;
			notifyIcon.Text = "BISS PopUp";
			notifyIcon.Icon = System.Drawing.SystemIcons.Information;
			notifyIcon.Visible = true;
			notifyIcon.BalloonTipTitle = "BISS";
			notifyIcon.BalloonTipIcon = ToolTipIcon.Info;

			// Everything is initialised now. Start receiver.
			receiver.StartReceiving();

			// Main program loop; the rest is event based.
			while (!close)
			{
				Application.DoEvents();
				System.Threading.Thread.Sleep(10);
			}
		}

		static void receiver_PacketReceived(object sender, PacketReceivedEventArgs e)
		{
			MessageType message = e.ReceivedPacket.MessageType;

			if (message == MessageType.BakeryIsThere)
				notifyIcon.BalloonTipText = "Bakery is there!";
			else if (message == MessageType.DeliveryIsThere)
				notifyIcon.BalloonTipText = "Delivery is there!";

			notifyIcon.ShowBalloonTip(300000);
		}

		static void exitMenu_Click(object sender, EventArgs e)
		{
			// Try to remove the icon from the tray.
			notifyIcon.Icon = null;
			notifyIcon.Visible = false;
			notifyIcon.Dispose();
			notifyIcon = null;

			close = true;
		}
	}
}
