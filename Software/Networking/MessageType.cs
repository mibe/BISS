namespace BISS.Networking
{
	/// <summary>
	/// Message types used in the BISS protocol.
	/// </summary>
	public enum MessageType : uint
	{
		None = 0,
		BakeryIsThere = 1,
		DeliveryIsThere = 2
	}
}
