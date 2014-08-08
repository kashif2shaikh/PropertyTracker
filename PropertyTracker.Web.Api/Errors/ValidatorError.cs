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

namespace PropertyTracker.Web.Api.Errors
{
    public class ValidatorError : IHttpActionResult
    {
        readonly string _errorTitle;
        readonly ValidationResult _result;
        readonly HttpRequestMessage _request;

        public ValidatorError(String errorTitle, ValidationResult result, HttpRequestMessage request)
        {
            _errorTitle = errorTitle;
            _result = result;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {            
            return Task.FromResult(Response);
        }

        public HttpResponseMessage Response
        {
            get
            {
                string errorMessage = _errorTitle + ":\n";
                foreach (var failure in _result.Errors)
                {
                    errorMessage += "'" + failure.PropertyName + "' property failed validation.  Error: " + failure.ErrorMessage + "\n";
                }

                var response = new HttpResponseMessage()
                {
                    Content = new StringContent(errorMessage),
                    StatusCode = HttpStatusCode.InternalServerError,
                    RequestMessage = _request
                };
                return response;                
            }
        }
    }
}