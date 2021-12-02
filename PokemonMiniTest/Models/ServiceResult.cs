using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace PokemonMiniTest.Models
{
    public class ServiceResult<T> where T : class
    {
        public ServiceResult()
        {
            HttpStatusCode = HttpStatusCode.OK;
        }

        public ServiceResult(T data, HttpStatusCode statusCode = HttpStatusCode.OK) {
            Data = data;
            HttpStatusCode = statusCode;
        }

        public ServiceResult(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            ErrorMessage = errorMessage;
            HttpStatusCode = statusCode;
        }

        public HttpStatusCode HttpStatusCode  { get; set;}
        public string ErrorMessage { get; set;}

        public bool IsSuccessful
        {
            get
            {
                if (string.IsNullOrEmpty(ErrorMessage))
                {
                    return true;
                }

                return false;
            }
        }

        public T Data { get; set;}
    }
}
