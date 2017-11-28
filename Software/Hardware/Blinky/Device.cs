using System;
using System.Linq;
using System.Threading;
using HidLibrary;

namespace BISS.Hardware.Blinky
{
	/// <summary>
	/// Provides methods for controlling the BISS.Blinky device. This is done by sending
	/// and receiving HID reports over USB.
	/// </summary>
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

		/// <summary>
		/// The default trigger options. Each option is enabled.
		/// </summary>
		public const TriggerOptions DefaultTriggerOptions = TriggerOptions.TouchSensor | TriggerOptions.Timeout | TriggerOptions.AuxOutput;

		/// <summary>
		/// Size of the USB HID report in bytes.
		/// </summary>
		/// <remarks>This value must be kept in sync with the report size in the Blinky firmware.</remarks>
		private const byte reportSize = 8;

		/// <summary>
		/// Maximum number of arguments to commands.
		/// </summary>
		private const int maxArgCount = 6;

		HidDevice hidDevice;
		Random random;
		bool disposed;
		byte lastSyncByte;
		readonly object lockObject;

		/// <summary>
		/// Gets a new synchronisation byte used for synchronising the communication with the Blinky device.
		/// </summary>
		private byte syncByte
		{
			get
			{
				byte result = lastSyncByte;

				// Ensure that the new sync byte differs from the old one.
				while (result == lastSyncByte)
				{
					result = (byte)random.Next(byte.MaxValue);
				}

				lastSyncByte = result;
				return result;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Blinky device is available.
		/// </summary>
		public static bool Available
		{
			get
			{
				return enumerate() != null;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is connected to the device.
		/// </summary>
		public bool Connected
		{
			get
			{
				return this.hidDevice != null && this.hidDevice.IsConnected;
			}
		}

		/// <summary>
		/// Gets or sets the timeout value in milliseconds until a command to the device fails.
		/// </summary>
		public UInt32 Timeout { get; set; } = 100;

		/// <summary>
		/// Occurs after the device was removed from the USB bus.
		/// </summary>
		public event EventHandler Removed;

		/// <summary>
		/// Initializes a new instance of the Device class.
		/// </summary>
		public Device()
		{
			this.random = new Random();
			this.lockObject = new object();
		}

		/// <summary>
		/// Return an instance of a <see cref="HidDevice"/> prepared for communicating with the Blinky device.
		/// </summary>
		/// <returns>Instance of <see cref="HidDevice"/> or NULL if the Blinky device is not available.</returns>
		private static HidDevice enumerate()
		{
			return HidDevices.Enumerate(UsbVendorId, UsbProductId).FirstOrDefault();
		}

		private void Device_Removed() => OnRemoved(EventArgs.Empty);

		/// <summary>
		/// Throws an exception if the current state if this instance is not suited for communicating with
		/// a Blinky device.
		/// </summary>
		/// <param name="checkConnected">TRUE if an exception should be thrown when this instance is not
		/// connected to a device. Defaults to TRUE.</param>
		/// <exception cref="ObjectDisposedException">Thrown when the object is already disposed.</exception>
		/// <exception cref="InvalidOperationException">Thrown when this instance is not connected to a device.</exception>
		private void isValidMethodCall(bool checkConnected = true)
		{
			if (this.disposed)
				throw new ObjectDisposedException(GetType().FullName);

			if (checkConnected && !Connected)
				throw new InvalidOperationException("Not connected to a device.");
		}

		/// <summary>
		/// Sends a command to the connected Blinky device.
		/// </summary>
		/// <param name="cmd">Command to send.</param>
		/// <param name="releaseLock">TRUE if the lock for thread-safety should be released after sending the command.</param>
		/// <param name="lockTaken">TRUE if the lock was aquired.</param>
		/// <param name="args">Arguments to the command.</param>
		/// <returns>TRUE if the command was sent successfully.</returns>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private bool sendReport(Command cmd, bool releaseLock, ref bool lockTaken, params byte[] args)
		{
			if (cmd == Command.None)
				throw new ArgumentException();
			if (args.Length > maxArgCount)
				throw new ArgumentOutOfRangeException("args");

			bool result = false;

			// Always reset lockTaken.
			lockTaken = false;

			try
			{
				Monitor.Enter(this.lockObject, ref lockTaken);

				// Build a HID report. The first byte is the command byte and the last byte is
				// the byte for syncing a (possible) answer to this report.
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

		/// <summary>
		/// Sends a command to the connected Blinky device.
		/// </summary>
		/// <param name="cmd">Command to send.</param>
		/// <param name="args">Arguments to the command.</param>
		/// <returns>TRUE if the command was sent successfully.</returns>
		private bool sendReport(Command cmd, params byte[] args)
		{
			bool lockTaken = false;
			return sendReport(cmd, true, ref lockTaken, args);
		}

		/// <summary>
		/// Sends a command to the connected Blinky device and receive an answer to that command.
		/// </summary>
		/// <param name="cmd">Command to send.</param>
		/// <param name="args">Arguments to the command.</param>
		/// <returns>Answer to the command or NULL if no answer was received.</returns>
		private byte[] sendAndReceiveReport(Command cmd, params byte[] args)
		{
			bool lockTaken = false;

			if (!sendReport(cmd, false, ref lockTaken, args))
				return null;

			HidReport receivedReport = new HidReport(reportSize);

			// Ensure that the answer from the device belongs to the previously sent command.
			// This is done by looping until the sync byte received from the device is the same
			// as the sent one.
			do
			{
				receivedReport = this.hidDevice.ReadReport((int)Timeout);
			} while (receivedReport.Data[7] != lastSyncByte);

			if (lockTaken)
				Monitor.Exit(this.lockObject);

			// Cut the answer from the received report. The first byte is the command to which
			// this answer belongs to and the last byte is the sync byte sent with the command.
			byte[] result = new byte[maxArgCount];
			Array.Copy(receivedReport.Data, 1, result, 0, maxArgCount);

			return result;
		}

		/// <summary>
		/// Raises the <see cref="Removed"/> event.
		/// </summary>
		/// <param name="e">An <see cref="EventArgs"/> that contains the event data.</param>
		protected virtual void OnRemoved(EventArgs e)
		{
			Removed?.Invoke(this, e);
		}

		#region Public methods
		/// <summary>
		/// Connect to the device.
		/// </summary>
		/// <returns>TRUE on success.</returns>
		/// <exception cref="InvalidOperationException">Thrown if this instance is already connected.</exception>
		/// <seealso cref="Connected"/>
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

		/// <summary>
		/// Disconnect from the device.
		/// </summary>
		public void Disconnect()
		{
			isValidMethodCall(false);

			if (this.hidDevice == null || !this.hidDevice.IsConnected)
				return;

			this.hidDevice.Removed -= Device_Removed;
			this.hidDevice.CloseDevice();
		}

		/// <summary>
		/// Enables the blink algorithm by sending the Trigger command.
		/// </summary>
		/// <param name="flags">Options to be used by the Trigger command.</param>
		/// <returns>TRUE on success.</returns>
		public bool Trigger(TriggerOptions flags = DefaultTriggerOptions)
		{
			isValidMethodCall();

			return sendReport(Command.Trigger, (byte)flags);
		}

		/// <summary>
		/// Enables the blink algorithm by sending the Trigger command.
		/// </summary>
		/// <param name="touchSensor">TRUE if the touch sensor for disabling the blink algorithm is enabled.</param>
		/// <param name="timeout">TRUE if the timeout of the blink algorithm is enabled.</param>
		/// <param name="auxOutput">TRUE if the auxiliary output signal is enabled.</param>
		/// <returns>TRUE on success.</returns>
		public bool Trigger(bool touchSensor, bool timeout, bool auxOutput)
		{
			TriggerOptions flags = TriggerOptions.None;

			if (touchSensor)
				flags |= TriggerOptions.TouchSensor;
			if (timeout)
				flags |= TriggerOptions.Timeout;
			if (auxOutput)
				flags |= TriggerOptions.AuxOutput;

			return Trigger(flags);
		}

		/// <summary>
		/// Sets the settings of the blink algorithm.
		/// </summary>
		/// <remarks>The settings are not saved to the EEPROM by calling this method.
		/// Call the <see cref="SaveSettings"/> method to save the settings.</remarks>
		/// <param name="settings">Instance of the <see cref="Settings"/> class containing the settings
		/// for the blink algorithm.</param>
		/// <returns>TRUE on success.</returns>
		/// <exception cref="ArgumentNullException">The parameter <paramref name="settings"/> was NULL.</exception>
		public bool SetSettings(Settings settings)
		{
			isValidMethodCall();

			if (settings == null)
				throw new ArgumentNullException("settings");

			byte[] args = new byte[5];
			args[0] = settings.Color.R;
			args[1] = settings.Color.G;
			args[2] = settings.Color.B;
			args[3] = settings.BlinkInterval;
			args[4] = settings.BlinkTimeout;

			return sendReport(Command.SetSettings, args);
		}

		/// <summary>
		/// Gets the settings of the blink algorithm.
		/// </summary>
		/// <returns>Instance of the <see cref="Settings"/> class containing the settings
		/// for the blink algorithm or NULL if the settings could not be determined.</returns>
		public Settings GetSettings()
		{
			isValidMethodCall();

			byte[] received = sendAndReceiveReport(Command.GetSettings);

			return new Settings(received[0], received[1], received[3], received[4], received[5]);
		}

		/// <summary>
		/// Saves the settings in the EEPROM of the device.
		/// </summary>
		/// <returns>TRUE on success.</returns>
		public bool SaveSettings()
		{
			isValidMethodCall();

			return sendReport(Command.SaveSettings);
		}

		/// <summary>
		/// Resets the settings to the defaults.
		/// </summary>
		/// <remarks>The settings are not saved to the EEPROM by calling this method.
		/// Call the <see cref="SaveSettings"/> method to save the settings.</remarks>
		/// <returns>TRUE on success.</returns>
		public bool ResetSettings()
		{
			isValidMethodCall();

			return sendReport(Command.ResetSettings);
		}

		/// <summary>
		/// Start the bootloader for uploading firmware updates. This also disconnects
		/// this instance from the device. The <see cref="Removed"/> event is raised.
		/// </summary>
		/// <returns>TRUE on success.</returns>
		public bool Bootloader()
		{
			isValidMethodCall();

			bool result = sendReport(Command.Bootloader);

			if (result)
			{
				Disconnect();
				OnRemoved(EventArgs.Empty);
			}

			return result;
		}

		/// <summary>
		/// Turns a previously enabled blink algorithm off again.
		/// </summary>
		/// <returns>TRUE on success.</returns>
		public bool TurnOff()
		{
			isValidMethodCall();

			return sendReport(Command.TurnOff);
		}
		#endregion

		#region IDisposable Support
		/// <summary>
		/// Disposes of the resources used by this instance.
		/// </summary>
		/// <param name="disposing">TRUE to release both managed and unmanaged resources.</param>
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
						this.hidDevice = null;
					}
				}

				disposed = true;
			}
		}

		/// <summary>
		/// Releases all resources used by this instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion
	}
}
