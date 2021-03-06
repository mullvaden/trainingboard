﻿using System;
using RestSharp;
using System.Collections.Generic;
using System.Text;

namespace Chello.Core
{
	public class TrelloApiBase
	{
		protected string AuthKey { get; private set; }
		protected string AuthToken { get; private set; }

		protected TrelloApiBase(string authKey, string authToken)
		{
			if (string.IsNullOrEmpty(authKey))
				throw new ArgumentException("Authentication key required. Auth keys can be generated from https://trello.com/1/appKey/generate", "authKey");

			this.AuthKey = authKey;
			this.AuthToken = authToken;
		}

		protected T GetRequest<T>(string path, params string[] args) where T : new()
		{
			RestClient client = new RestClient(Config.ApiBaseUrl);
            RestRequest request = new RestRequest(BuildUrl(path, args)) { DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" };


		    return client.Execute<T>(request).Data;
		}

		protected string GetRequest(string path, params string[] args)
		{
			RestClient client = new RestClient(Config.ApiBaseUrl);
            RestRequest request = new RestRequest(BuildUrl(path, args)) { DateFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" };

		    return client.Execute(request).Content;
		}

		protected TOutput PutRequest<TOutput, TInput>(TInput obj, string path, params string[] args) where TOutput : new()
		{
			return Request<TOutput, TInput>(Method.PUT, obj, path, args);
		}

		protected TOutput PutRequestWithJsonNet<TOutput, TInput>(TInput obj, string path, params string[] args) where TOutput : new()
		{
			return RequestWithLegacySerializer<TOutput, TInput>(Method.PUT, obj, path, args);
		}

		protected TOutput PostRequest<TOutput, TInput>(TInput obj, string path, params string[] args) where TOutput : new()
		{
			return Request<TOutput, TInput>(Method.POST, obj, path, args);
		}

		protected void DeleteRequest(string path, params string[] args)
		{
			Request<List<object>, object>(Method.DELETE, null, path, args);
		}

		private TOutput Request<TOutput, TInput>(Method method, TInput obj, string path, params string[] args) where TOutput : new()
		{
			RestClient client = new RestClient(Config.ApiBaseUrl);
			RestRequest request = new RestRequest(BuildUrl(path, args), method);

			request.RequestFormat = DataFormat.Json;
			request.AddBody(obj);

			return client.Execute<TOutput>(request).Data;
		}

		/// <summary>
		/// Uses the old JSON.NET serializer so JSON.NET Json[] attributes are supported.
		/// </summary>
		private TOutput RequestWithLegacySerializer<TOutput, TInput>(Method method, TInput obj, string path, params string[] args) where TOutput : new()
		{
			RestClient client = new RestClient(Config.ApiBaseUrl);
			RestRequest request = new RestRequest(BuildUrl(path, args), method);
			request.JsonSerializer = new LegacyJsonSerializer();

			request.RequestFormat = DataFormat.Json;
			request.AddBody(obj);

			return client.Execute<TOutput>(request).Data;
		}

		protected string BuildUrl(string path, params string[] args)
		{
			// Assume for now that no querystring is added
			path = string.Format(path, args);
			path = string.Format("{0}{1}key={2}", path, path.Contains("?") ? "&" : "?", this.AuthKey);

			if (!string.IsNullOrEmpty(this.AuthToken))
				path = string.Format("{0}&token={1}", path, this.AuthToken);

			return path;
		}

        protected static string BuildQueryString(object args)
        {
            string queryString = String.Empty;
            if (args != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var prop in args.GetType().GetProperties())
                {
                    sb.AppendFormat("{0}={1}&", prop.Name, prop.GetValue(args, null));
                }
                if (sb.Length > 0) sb.Remove(sb.Length - 1, 1);
                queryString = sb.ToString();
            }
            return queryString;
        }
	}
}
