using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bubbleshot.Server.Common.Pcl.Serializers;

namespace Bubbleshot.Server.Common.Pcl.Base
{
	public abstract class BaseHttpRequest<TRequest, TResult, TResponse>
		where TRequest : BaseRequestParameters
		where TResponse : BaseResponse
		where TResult : BaseHttpResult<TResponse>
	{
		private HttpWebRequest _request;
		private readonly string _address;
		private readonly string _method;
		private string _responseJson;
		private readonly Serializer<TResult> _serializer;
		private TResult _result;
		private TResponse _response;
		protected BaseHttpRequest(string address, string method = "GET")
		{
			_address = address;
			_method = method;
			_serializer = new Serializer<TResult>();
		}
		public async Task<TResponse> Execute(TRequest request)
		{
			try
			{
				var parameters = request.Parameters;
				var url = $"{_address}?{string.Join("&", parameters.Select(kvp => $"{kvp.Key}={kvp.Value}"))}";

				_request = (HttpWebRequest) WebRequest.Create(url);
				_request.Method = _method;
				
				WebResponse response = await _request.GetResponseAsync();
				var responseStream = response.GetResponseStream();

				if (responseStream != null)
					using (var sr = new StreamReader(responseStream))
					{
						_responseJson = sr.ReadToEnd();
						_result = _serializer.DeserializeJson(_responseJson);
						_response = _result.Response;
					}
			}
			catch (Exception ex)
			{
				Debug.Write(ex.Message);
			}
			return _response;
		}
	}
}
