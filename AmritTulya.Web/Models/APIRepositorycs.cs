using AmritTulya.EntityLayer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace AmritTulya.Web.Models
{

    public class APICommunicationResponseModel<T>
    {
        public HttpStatusCode statusCode { get; set; }
        public T data { get; set; }
        public String Message { get; set; }
        public Boolean Success { get; set; }
        public string NotificationType { get; set; }
        public string ReturnToUrl { get; set; }
    }
    public class APIRepository
    {

        public static string baseURL = string.Empty;
        private readonly IConfiguration configuration;
        public APIRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            baseURL = "http://localhost:64016/";
        }


        #region APICommunication - Common Method for API calling
        public APICommunicationResponseModel<string> APICommunication(string URL, HttpMethod invokeType, ByteArrayContent body, string token)
        {
            APICommunicationResponseModel<string> response = new APICommunicationResponseModel<string>();
            response.statusCode = HttpStatusCode.BadRequest;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    HttpResponseMessage oHttpResponseMessage = new HttpResponseMessage();

                    if (invokeType.Method == HttpMethod.Get.ToString())
                    {
                        var responseTask = client.GetAsync(URL);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Post.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                        var responseTask = client.PostAsync(URL, body);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Put.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                        var responseTask = client.PostAsync(URL, body);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    else if (invokeType.Method == HttpMethod.Delete.ToString())
                    {
                        if (body != null)
                            body.Headers.ContentType = new MediaTypeHeaderValue("application/json");



                        var responseTask = client.DeleteAsync(URL);
                        responseTask.Wait();

                        oHttpResponseMessage = responseTask.Result;
                    }
                    response.statusCode = oHttpResponseMessage.StatusCode;

                    if (oHttpResponseMessage.IsSuccessStatusCode)
                        response.data = oHttpResponseMessage.Content.ReadAsStringAsync().Result;
                    else
                        response.data = string.Empty;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return response;
        }

       #endregion

        #region Common Method for GetToken
        public AccessTokenModel GetToken(UsersEntity objuser, string apiurl)
        {
            AccessTokenModel tokenData = new AccessTokenModel();

            try
            {
                HttpResponseMessage oHttpResponseMessage = new HttpResponseMessage();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    var content = new StringContent(JsonConvert.SerializeObject(objuser), Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsync(apiurl, content);//"/api/token"
                    responseTask.Wait();

                    oHttpResponseMessage = responseTask.Result;
                }

                if (oHttpResponseMessage.IsSuccessStatusCode)
                {
                    string jsonResult = oHttpResponseMessage.Content.ReadAsStringAsync().Result;

                    tokenData = JsonConvert.DeserializeObject<AccessTokenModel>(jsonResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return tokenData;
        }
        #endregion


    }
}
