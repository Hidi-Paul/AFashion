using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using System;
using System.Text.RegularExpressions;

namespace OCS.MVC.Security
{
    public class MultiTennantCookieManager : ChunkingCookieManager, ICookieManager
    {
        private static Regex SessionRegex { get; set; } = InitSessionRegex();

        public MultiTennantCookieManager() : base()
        {

        }

        private string GetSessionID(IOwinContext context)
        {
            string path = context.Request.Uri.LocalPath;

            Match match = SessionRegex.Match(path);

            if (match.Success)
            {
                return match.Value;
            }

            return string.Empty;
        }


        public new void AppendResponseCookie(IOwinContext context, string key, string value, CookieOptions options)
        {
            var sessionId = "" + context.Request.Get<string>("userSessionGuid");
            
            key = key + sessionId;

            base.AppendResponseCookie(context, key, value, options);
        }

        public new void DeleteCookie(IOwinContext context, string key, CookieOptions options)
        {
            var sessionId = GetSessionID(context);

            if (sessionId != string.Empty)
            {
                key = key + sessionId;
            }

            base.DeleteCookie(context, key, options);
        }

        public new string GetRequestCookie(IOwinContext context, string key)
        {
            var sessionId = GetSessionID(context);

            if (sessionId != string.Empty)
            {
                key = key + sessionId;
            }

            var cookie = base.GetRequestCookie(context, key);
            return cookie;
        }


        #region Helpers

        private static Regex InitSessionRegex()
        {
            return new Regex(@"g-[a-zA-Z0-9]{8}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{12}", RegexOptions.Compiled);
        }

        #endregion Helpers
    }
    
}