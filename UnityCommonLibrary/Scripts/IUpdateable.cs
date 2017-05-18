namespace UnityCommonLibrary
{
	public interface IUpdateable
	{
		bool Enabled { get; set; }
		void ManagedUpdate();
	}
}