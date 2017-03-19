using System;
using System.Net.NetworkInformation;

namespace BISS.Networking
{
	/// <summary>
	/// Transmits a packet over a network interface.
	/// </summary>
	public class InterfaceSender : Sender
	{
		/// <summary>
		/// Transmit the specified packet using the network interface specified.
		/// </summary>
		/// <param name="packet">Packet to be transmitted.</param>
		/// <param name="interface">The network interface over which the packet should be transmitted.</param>
		/// <returns>Number of packets successfully sent.</returns>
		public uint Send(Packet packet, NetworkInterface @interface)
		{
			if (@interface == null)
				throw new ArgumentNullException("interface");
			if (@interface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
				throw new ArgumentException("The specified interface is the loopback interface.", "interface");
			if (@interface.OperationalStatus != OperationalStatus.Up
				&& @interface.OperationalStatus != OperationalStatus.Unknown)
				throw new ArgumentException(String.Format("The specified interface ({0}) is down.", @interface), "interface");

			IPInterfaceProperties props = @interface.GetIPProperties();

			// Makes no sense if the interface has no IP addresses...
			if (props.UnicastAddresses.Count == 0)
				throw new ArgumentException(String.Format("The speficied interface ({0}) has no unicast address.",
					@interface), "interface");

			uint sent = 0;

			foreach (UnicastIPAddressInformation addr in props.UnicastAddresses)
			{
				// Check if the IP address is suitable.
				if (IsUsableIPAddress(addr.Address))
				{
					if (base.Send(packet, addr.Address))
						sent++;
				}
			}

			return sent;
		}

		/// <summary>
		/// Transmit the specified packet over all available network interfaces on this system.
		/// </summary>
		/// <param name="packet">Packet to be transmitted.</param>
		/// <returns>Number of packets successfully sent.</returns>
		public uint Send(Packet packet)
		{
			NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
			uint sent = 0;

			foreach (NetworkInterface @interface in nis)
			{
				// Ignore unsupported interfaces
				if (@interface.NetworkInterfaceType == NetworkInterfaceType.Loopback)
					continue;
				if (@interface.OperationalStatus != OperationalStatus.Up)
					continue;

				sent += Send(packet, @interface);
			}

			return sent;
		}
	}
}
