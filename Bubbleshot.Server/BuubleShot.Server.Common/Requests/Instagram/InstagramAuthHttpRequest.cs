using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BubbleShot.Server.Common.Base;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;

namespace BubbleShot.Server.Common.Requests.Instagram
{
	public class InstagramAuthHttpRequest : BaseAuthHttpRequest
	{
		private readonly string _clientId;
		private string _accessToken;
		public InstagramAuthHttpRequest(string address, string clientId) : base(address)
		{
			_clientId = clientId;
		}

		public override string Auth()
		{
			try
			{
				//https://api.instagram.com/oauth/authorize/
				//241559688.1677ed0.9d287accaaab4830885735d53ccc6018 token!!!
				

				OAuthManager.RegisterClient(new InstagramClient("b84c226bd30044a281b9444fd84e28e7", "72677245aa144fe6bd53ae92e7e8fedd"));

				string url = OAuthWeb.GetAuthorizationUrl("instagram");
				var result = OAuthWeb.VerifyAuthorization("http://localhost");


			}
			catch (Exception ex)
			{
				
			}
			return string.Empty;
		}
	}
}
