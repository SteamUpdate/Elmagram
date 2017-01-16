using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
//using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Elmagram.Services
{
    public class ElmaManager
    {
        public static ElmaManager Instance { get { return new ElmaManager();}}

        public List<User> GetElmaUsers(string login, string password)
        {
            ElmaServer = "https://elma.elewise.com";
            var a = Authorization(login, password);
            if (a != null)
            {
                AuthToken = a.AuthToken;
                SessionToken = a.SessionToken;
                CurrentUserId = a.CurrentUserId;
            }
            return GetUsers();
        }

        private static string AuthToken { get; set; }
        private static string SessionToken { get; set; }
        private static string ElmaServer { get; set; }
        private static string CurrentUserId { get; set; }
        //токен приложения задается в админке ELMA
        //private static readonly string appToken = "35FCC7951C7B42046EC767975F0A138E5E85AC1D5B6CC4D0464BFF95FD267CA30AB9162CED0B93E18E19330AE1C03CCCC4D80CD3DC125CA9387927C3459F057C";
        private static readonly string appToken = "285C8352AA7C67BFF882E4F236DECF51098C141AFB33A2AA4F7B34B4B3CEEF5DA30C848591DA55D5226C5D8D2C36432B12A5EF86C3D2EDF7E7C5781EC9D4E14A";
        private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            DateParseHandling = DateParseHandling.DateTime,
            DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
        };
        private static Guid GuidFile { get; set; }

        /// <summary>
        /// Выполнить запрос на основании данных
        /// </summary>
        /// <param name="restData">Данные</param>
        /// <returns>Ответ сервера</returns>
        private static HttpWebResponse DoRestExecute(RestData restData, Dictionary<string, string> headers = null)
        {
            //Создаем экземпляр запроса по URL
            var HttpWReq = (HttpWebRequest)WebRequest.Create(restData.Url);

            //Указываем метод запроса
            HttpWReq.Method = restData.HTTPMethod;
            HttpWReq.Credentials = CredentialCache.DefaultCredentials;
            //Добавляем токен приложения
            HttpWReq.Headers["ApplicationToken"] = appToken;
            //Добавляем токен авторизации, если есть
            if (!string.IsNullOrWhiteSpace(AuthToken))
            {
                HttpWReq.Headers["AuthToken"] = AuthToken;
            }
            //Добавляем токен сессии, если есть
            if (!string.IsNullOrEmpty(SessionToken))
            {
                HttpWReq.Headers["SessionToken"] = SessionToken;
            }
            HttpWReq.Accept = "application/json; charset=utf-8";
            HttpWReq.Headers["WebData-Version"] = "2.0";
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    HttpWReq.Headers[header.Key] = header.Value;
                }
            }
            var encoding = new UTF8Encoding();

            //Если метод POST, тогда добавляем тип контента и вставляем данные
            if (restData.HTTPMethod == "POST")
            {
                var byteData = encoding.GetBytes(restData.Data);
                HttpWReq.ContentType = "application/json; charset=utf-8";

                HttpWReq.Headers["Content-Lenght"] = byteData.Length.ToString();
                using (Stream requestStream = HttpWReq.GetRequestStreamAsync().GetAwaiter().GetResult())
                {
                    requestStream.Write(byteData, 0, byteData.Length);
                }
            }
            return (HttpWebResponse)HttpWReq.GetResponseAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Выполнить запрос
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="url">URL</param>
        /// <param name="requestData">Сериализованные данные</param>
        /// <param name="httpMethod">Метод запроса (post, get)</param>
        /// <returns>Десериализованный ответ с сервера</returns>
        private static T RestExecute<T>(string url, object requestData, string httpMethod, Dictionary<string, string> addHeaders = null)
        {
            var serializedObject = JsonConvert.SerializeObject(requestData, new HttpPostedFileConverter());
            var g = new RestData(url, serializedObject, httpMethod);
            HttpWebResponse httpWResp = null;
            try
            {
                httpWResp = DoRestExecute(g, addHeaders);
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Response);
            }

            var result = "";
            if (httpWResp != null && httpWResp.StatusCode == HttpStatusCode.OK)
            {
                using (var sr = new StreamReader(httpWResp.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            else
            {
                if (httpWResp != null)
                    Console.WriteLine("ERROR STATUS CODE:{0}, MESSAGE: {1}", httpWResp.StatusCode,
                        httpWResp.StatusDescription);
                else
                {
                }
            }
            return JsonConvert.DeserializeObject<T>(result, JsonSettings);
        }

        /// <summary>
        /// Функция получения статуса сервера
        /// </summary>
        /// <returns>Статус сервера 1, 2, 3</returns>
        private string GetServerStatus(out string reason)
        {
            reason = string.Empty;
            //Отправляем запрос на сервер для получения статуса
            var httpWReq = (HttpWebRequest)WebRequest.Create(string.Format("{0}StartInfoHandler.ashx?type=StartInfo", ElmaServer));
            var response = httpWReq.GetResponseAsync().GetAwaiter().GetResult();
            string result = string.Empty;
            string stringResponse;
            using (var sr = new StreamReader(response.GetResponseStream()))
            {
                stringResponse = sr.ReadToEnd();
            }
            if (!string.IsNullOrWhiteSpace(stringResponse))
                result = stringResponse.Substring(0, 1);
            while (result == "0")
            {
                System.Threading.Thread.Sleep(1000);
                result = GetServerStatus(out reason);
            }
            if (result == "3")
            {
                var index = stringResponse.LastIndexOf(";");
                reason = stringResponse.Substring(index, stringResponse.Length - index);
            }

            return result;
        }

        /// <summary>
        /// ELMA Авторизация REST
        /// </summary>
        public Auth Authorization(string login, string password)
        {
            var urlAuth = string.Format("https://elma.elewise.com/API/REST/Authorization/LoginWith?username={1}", ElmaServer, login);
            return RestExecute<Auth>(urlAuth, password, "POST");
        }

        private static List<User> GetUsers()
        {
            var url = String.Format("https://elma.elewise.com/API/REST/Entity/QueryTree?type={0}&q={1}&select={2}", new Guid("18faf3ae-03c9-4e64-b02a-95dd63e54c4d"), "Id<>" + CurrentUserId, "FullName");
            return RestExecute<List<User>>(url, "", "GET");
        }
    }
}
