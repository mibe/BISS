﻿using System;
using System.Linq;
using System.Threading;
using HidLibrary;

namespace BISS.Hardware.Blinky
{
	public class Device : IDisposable
	{
		/// <summary>
		/// The vendor ID of the USB device.
		/// <remarks>The vendor ID of pid.codes / InterBiometrics is used.</remarks>
		/// </summary>
		const UInt16 UsbVendorId = 0x1209;

		/// <summary>
		/// The product ID of the USB device.
		/// </summary>
		// FIXME: Still the tesing PID.
		const UInt16 UsbProductId = 0x0001;

		public const TriggerFlags DefaultTriggerFlags = TriggerFlags.TouchSensor | TriggerFlags.Timeout | TriggerFlags.AuxOutput;

		private const byte reportSize = 8;
		private const int maxArgCount = 6;

		HidDevice hidDevice;
		Random random;
		bool disposed;
		byte lastSyncByte;
		readonly object lockObject;

		private byte syncByte
		{
			get
			{
				byte result = lastSyncByte;

				while (result == lastSyncByte)
				{
					result = (byte)random.Next(byte.MaxValue);
				}

				lastSyncByte = result;
				return result;
			}
		}

		public static bool Available
		{
			get
			{
				return enumerate() != null;
			}
		}

		public bool Connected
		{
			get
			{
				return this.hidDevice != null && this.hidDevice.IsConnected;
			}
		}

		public UInt32 Timeout
		{ get; set; }

		event EventHandler Removed;

		public Device()
		{
			this.random = new Random();
			this.lockObject = new object();
		}

		private void Device_Removed() => Removed?.Invoke(this, EventArgs.Empty);

		private static HidDevice enumerate()
		{
			return HidDevices.Enumerate(UsbVendorId, UsbProductId).FirstOrDefault();
		}

		private void isValidMethodCall(bool checkConnected = true)
		{
			if (this.disposed)
				throw new ObjectDisposedException(GetType().FullName);

			if (checkConnected && !Connected)
				throw new InvalidOperationException("Not connected to device.");
		}

		private bool sendReport(Command cmd, bool releaseLock, ref bool lockTaken, params byte[] args)
		{
			if (cmd == Command.None)
				throw new ArgumentException();
			if (args.Length > maxArgCount)
				throw new ArgumentOutOfRangeException("args");

			bool result = false;

			try
			{
				Monitor.Enter(this.lockObject, ref lockTaken);

				HidReport report = new HidReport(reportSize);
				report.Data = new byte[reportSize];
				report.Data[0] = (byte)cmd;
				for (int a = 0; a < args.Length; a++)
					report.Data[a + 1] = args[a];
				report.Data[7] = syncByte;

				result = this.hidDevice.WriteReport(report, (int)Timeout);
			}
			finally
			{
				if (releaseLock && lockTaken)
					Monitor.Exit(this.lockObject);
			}

			return result;
		}

		private bool sendReport(Command cmd, params byte[] args)
		{
			bool lockTaken = false;
			return sendReport(cmd, true, ref lockTaken, args);
		}

		private byte[] sendAndReceiveReport(Command cmd, params byte[] args)
		{
			bool lockTaken = false;

			if (!sendReport(cmd, false, ref lockTaken, args))
				return null;

			HidReport receivedReport = new HidReport(reportSize);

			do
			{
				receivedReport = this.hidDevice.ReadReport((int)Timeout);
			} while (receivedReport.Data[7] != lastSyncByte);

			if (lockTaken)
				Monitor.Exit(this.lockObject);

			byte[] result = new byte[maxArgCount];
			Array.Copy(receivedReport.Data, 1, result, 0, maxArgCount);

			return result;
		}

		#region Public methods
		public bool Connect()
		{
			isValidMethodCall(false);

			if (Connected)
				throw new InvalidOperationException("Already connected to device.");

			this.hidDevice = enumerate();

			if (this.hidDevice == null)
				return false;

			this.hidDevice.Removed += Device_Removed;
			this.hidDevice.OpenDevice();

			return true;
		}

		public void Disconnect()
		{
			isValidMethodCall(false);

			if (this.hidDevice == null || !this.hidDevice.IsConnected)
				return;

			this.hidDevice.Removed -= Device_Removed;
			this.hidDevice.CloseDevice();
		}

		public bool Trigger(TriggerFlags flags = DefaultTriggerFlags)
		{
			isValidMethodCall();

			return sendReport(Command.Trigger, (byte)flags);
		}

		public bool Trigger(bool touchSensor, bool timeout, bool auxOutput)
		{
			TriggerFlags flags = 0;

			if (touchSensor)
				flags |= TriggerFlags.TouchSensor;
			if (timeout)
				flags |= TriggerFlags.Timeout;
			if (auxOutput)
				flags |= TriggerFlags.AuxOutput;

			return Trigger(flags);
		}

		public bool SetSettings(Settings settings)
		{
			isValidMethodCall();

			byte[] args = new byte[5];
			args[0] = settings.Color.R;
			args[1] = settings.Color.G;
			args[2] = settings.Color.B;
			args[3] = settings.BlinkInterval;
			args[4] = settings.BlinkTimeout;

			return sendReport(Command.SetSettings, args);
		}

		public Settings GetSettings()
		{
			isValidMethodCall();

			byte[] received = sendAndReceiveReport(Command.GetSettings);

			return new Settings(received[0], received[1], received[3], received[4], received[5]);
		}

		public bool SaveSettings()
		{
			isValidMethodCall();

			return sendReport(Command.SaveSettings);
		}

		public bool ResetSettings()
		{
			isValidMethodCall();

			return sendReport(Command.ResetSettings);
		}

		public bool Bootloader()
		{
			isValidMethodCall();

			bool result = sendReport(Command.Bootloader);

			if (result)
			{
				Disconnect();
				Device_Removed();
			}

			return result;
		}

		public bool TurnOff()
		{
			isValidMethodCall();

			return sendReport(Command.TurnOff);
		}
		#endregion

		#region IDisposable Support
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					if (this.hidDevice != null)
					{
						Disconnect();
						this.hidDevice.Dispose();
					}
				}

				disposed = true;
			}
		}

		// Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
