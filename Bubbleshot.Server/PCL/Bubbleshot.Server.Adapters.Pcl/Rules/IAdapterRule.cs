namespace Bubbleshot.Server.Adapters.Pcl.Rules
{
	public interface IAdapterRule
	{
		double Longitude { get; set; } 
		double Latitude { get; set; }
		int Radius { get; set; }
	}
}