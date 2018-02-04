using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TarkLogs;
using TarkLogs.Constants;
using TarkUtils;

namespace TarkNoIP
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        public LogApi Log { get; set; }

        public string GetCLientIpAddress(HttpRequestMessage request)
        {
            try
            {
                if (request.Properties.ContainsKey("MS_HttpContext"))
                {
                    return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostName;
                }
                else if (HttpContext.Current != null)
                {
                    return HttpContext.Current.Request.UserHostAddress;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetRequestString(HttpActionContext action)
        {
            string requestQueryString;
            string requestHeaders;
            string requestContent;

            try
            {
                requestQueryString = action.Request.RequestUri.Query;
            }
            catch (Exception)
            {
                requestQueryString = string.Empty;
            }

            try
            {
                requestHeaders = action.Request.Headers.ToString();
            }
            catch (Exception)
            {
                requestHeaders = string.Empty;
            }

            try
            {
                var stream = new StreamReader(action.Request.Content.ReadAsStreamAsync().Result);
                stream.BaseStream.Position = 0;
                requestContent = stream.ReadToEnd();
            }
            catch (Exception)
            {
                requestContent = string.Empty;
            }

            var sb = new StringBuilder();

            if (!String.IsNullOrWhiteSpace(requestQueryString))
            {
                sb.Append(requestQueryString);
                sb.Append(Environment.NewLine);
            }

            if (!String.IsNullOrWhiteSpace(requestHeaders))
            {
                sb.Append(requestHeaders);
                sb.Append(Environment.NewLine);
            }

            if (!String.IsNullOrWhiteSpace(requestContent))
            {
                sb.Append(requestContent);
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public string GetServiceEndPoint(HttpActionContext action)
        {
            try
            {
                return string.Format("{0}://{1}:{2}",
                        action.Request.RequestUri.Scheme,
                        action.Request.RequestUri.Host,
                        action.Request.RequestUri.Port.ToString()
                    );
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetOperation(HttpActionContext action)
        {
            try
            {
                return string.Format("{0}.{1}",
                        action.ControllerContext.ControllerDescriptor.ControllerName,
                        action.ActionDescriptor.ActionName
                    );
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public string GetResponseString(HttpActionExecutedContext action)
        {
            try
            {
                if (action.Response.Content == null)
                    return string.Empty;

                var sr = new StreamReader(action.Response.Content.ReadAsStreamAsync().Result);
                sr.BaseStream.Position = 0;
                return sr.ReadToEnd();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private int GetResponseStatusCode(HttpActionExecutedContext action)
        {
            try
            {
                return action.Response.StatusCode.GetHashCode();
            }
            catch (Exception)
            {
                return - 1;
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null || actionContext.Request == null)
                return;

            try
            {
                Log = new LogApi();
                Log.Watch.Start();

                Log.ServiceDescription = AppSettingsManager.Get<string>(AppSettingsConstants.KEY_SERVICE_DESCRIPTION);
                Log.FromHost = GetCLientIpAddress(actionContext.Request);
                Log.OperationDescription = GetOperation(actionContext);
                Log.Request = GetRequestString(actionContext);
                Log.ToEndPoint = GetServiceEndPoint(actionContext);
            }
            catch (Exception)
            {
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                Log.Response = GetResponseString(actionExecutedContext);
                Log.Status = GetResponseStatusCode(actionExecutedContext);
                Log.Persist();
            }
            catch (Exception)
            {
            }
        }
    }
}