using Newtonsoft.Json;

namespace Bubbleshot.Server.Common.Pcl.Serializers
{
	public class Serializer<TResult>
	{
		public TResult DeserializeJson(string json)
		{
			var result = JsonConvert.DeserializeObject<TResult>(json);
			return result;
		} 
	}
}
