namespace PhotoStorm.Core.Portable.Adapters.Rules
{
	public class AdapterRule : IAdapterRule
	{
		public double Longitude { get; set; }
		public double Latitude { get; set; }
		public int Radius { get; set; }
	}
}
