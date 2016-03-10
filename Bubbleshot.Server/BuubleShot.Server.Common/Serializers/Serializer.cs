using Newtonsoft.Json;

namespace BubbleShot.Server.Common.Serializers
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
