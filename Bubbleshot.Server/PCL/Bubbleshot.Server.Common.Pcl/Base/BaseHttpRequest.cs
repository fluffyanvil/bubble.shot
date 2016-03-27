using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bubbleshot.Server.Common.Pcl.Serializers;

namespace Bubbleshot.Server.Common.Pcl.Base
{
	public abstract class BaseHttpRequest<TRequest, TResponse>
		where TRequest : BaseRequestParameters
		where TResponse : BaseHttpResponse
	{
		private HttpWebRequest _request;
		private readonly string _address;
		private readonly string _method;
		private string _responseJson;
		private readonly Serializer<TResponse> _serializer;
		private TResponse _result;
		protected BaseHttpRequest(string address, string method = "GET")
		{
			_address = address;
			_method = method;
			_serializer = new Serializer<TResponse>();
		}
		public async Task<TResponse> Execute(TRequest request)
		{
			try
			{
				var parameters = request.Parameters;
				var url = $"{_address}?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";

				_request = (HttpWebRequest) WebRequest.Create(url);
				_request.Method = _method;
				
				var response = await _request.GetResponseAsync();
				var responseStream = response.GetResponseStream();

				if (responseStream != null)
					using (var sr = new StreamReader(responseStream))
					{
						_responseJson = sr.ReadToEnd();
						_result = _serializer.DeserializeJson(_responseJson);
					}
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
			}
			return _result;
		}
	}
}
