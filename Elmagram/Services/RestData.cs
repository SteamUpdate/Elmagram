using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elmagram.Services
{
    /// <summary>
    /// Данные для REST
    /// </summary>
    public class RestData
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="url">URL Метода</param>
        /// <param name="requestData">Сериализованные данные</param>
        /// <param name="httpMethod">Метод запроса (POST/GET)</param>
        public RestData(string url, string requestData, string httpMethod)
        {
            Url = url;
            Data = requestData;
            HTTPMethod = httpMethod;
        }

        /// <summary>
        /// URL Метода
        /// </summary>
        /// <example>http://localhost:5555/API/REST/EntityChanges/Sync</example>>
        public string Url;

        /// <summary>
        /// Сериализованные данные
        /// </summary>
        public string Data;

        /// <summary>
        /// Метод запроса (POST/GET)
        /// </summary>
        /// <example>WebRequestMethods.Http.Post</example>
        public string HTTPMethod;
    }
}
