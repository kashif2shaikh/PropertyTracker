using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using FluentValidation.Results;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

namespace PropertyTracker.Web.Api.Errors
{
    public class ValidatorError : IHttpActionResult
    {
        readonly string _errorTitle;
        readonly ValidationResult _result;
        readonly HttpRequestMessage _request;
        readonly HttpStatusCode _statusCode;

        public ValidatorError(String errorTitle, ValidationResult result, HttpRequestMessage request)
        {
            _errorTitle = errorTitle;
            _result = result;
            _request = request;
            _statusCode = HttpStatusCode.InternalServerError;
        }

        public ValidatorError(String errorTitle, HttpStatusCode statusCode, ValidationResult result, HttpRequestMessage request)
        {
            _errorTitle = errorTitle;
            _result = result;
            _request = request;
            _statusCode = statusCode;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {            
            return Task.FromResult(Response);
        }

        public HttpResponseMessage Response
        {
            get
            {
                string errorMessage = _errorTitle + ": ";
                foreach (var failure in _result.Errors)
                {
                    errorMessage += "'" + failure.PropertyName + "' property failed validation.  Error: " + failure.ErrorMessage + ";";
                    errorMessage = "{\n" + "Message: \"" + errorMessage + "\"\n}";
                }

                var response = new HttpResponseMessage()
                {                    
                    Content = new StringContent(errorMessage, Encoding.UTF8, "application/json"),
                    StatusCode = _statusCode,
                    RequestMessage = _request
                };
                return response;                
            }
        }
    }
}