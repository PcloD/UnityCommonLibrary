namespace UnityCommonLibrary
{
	public interface IUpdateable
	{
		bool enabled { get; set; }
		void ManagedUpdate();
	}
}
