using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PhotoStorm.Core.Portable.Common.Serializers;

namespace PhotoStorm.Core.Portable.Common.Base
{
	public abstract class BaseHttpRequest<TRequest, TResponse>
		where TRequest : BaseRequestParameters
		where TResponse : BaseHttpResponse
	{
	    private readonly HttpClient _httpClient;
		private readonly string _address;
		private string _responseJson;
		private readonly Serializer<TResponse> _serializer;
		private TResponse _result;
		protected BaseHttpRequest(string address, string method = "GET")
		{
            _httpClient = new HttpClient();
			_address = address;
			_serializer = new Serializer<TResponse>();
		}
		public async Task<TResponse> Execute(TRequest request)
		{
			try
			{
				var parameters = request.Parameters;
				var url = $"{_address}?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";
			    var responseStream = await _httpClient.GetStreamAsync(url);
				if (responseStream != null)
					using (var sr = new StreamReader(responseStream))
					{
						_responseJson = sr.ReadToEnd();
						_result = _serializer.DeserializeJson(_responseJson);
					}
			}
			catch (Exception)
			{
			}
			return _result;
		}
	}
}
