﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DAQMS.Core.Utility;
using DAQMS.Core;
using DAQMS.Domain.Models;

namespace DAQMS.Web
{
    //[UserAuthorize]
    public class BaseController : Controller
    {
        #region Global Variable Declaration

        #endregion

        #region Constructor

        public BaseController()
        {
        }

        #endregion

        #region Action

        public User CurrentLoggedInUser
        {
            get
            {
                User user = SessionHelper.CurrentSession.Content.LoggedInUser;
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            #region Get Current Action, Controller, Area
            string currentActionName = string.Empty;
            string currentControllerName = string.Empty;
            string currentAreaName = string.Empty;

            object objCurrentControllerName;
            this.RouteData.Values.TryGetValue("controller", out objCurrentControllerName);

            object objCurrentActionName;
            this.RouteData.Values.TryGetValue("action", out objCurrentActionName);

            if (this.RouteData.DataTokens.ContainsKey("area"))
            {
                currentAreaName = this.RouteData.DataTokens["area"].ToString();
            }
            if (objCurrentActionName != null)
            {
                currentActionName = objCurrentActionName.ToString();
            }
            if (objCurrentControllerName != null)
            {
                currentControllerName = objCurrentControllerName.ToString();
            }
            #endregion

            base.OnActionExecuting(filterContext);
        }

        #region Check User Authenticated
        private bool CheckIfUserIsAuthenticated(ActionExecutingContext filterContext)
        {
            // If Result is null, we’re OK: the user is authenticated and authorized. 
            if (filterContext.Result == null)
            {
                return true;
            }

            // If here, you’re getting an HTTP 401 status code. In particular,
            // filterContext.Result is of HttpUnauthorizedResult type. Check Ajax here. 
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfUserAccessRight(string actionName, string controllerName, string areaName)
        {
            return true;
        }
        #endregion

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;

            // Attempt to read the culture cookie from Request
            HttpCookie cultureCookie = Request.Cookies["_culture"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureCookie = Request.Cookies["app.culture"];
                if (cultureCookie != null)
                {
                    cultureName = cultureCookie.Value;
                }
                else
                {
                    cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0
                        ? Request.UserLanguages[0]
                        : null; // obtain it from HTTP header AcceptLanguages   
                }
            }

            // Validate culture name
            cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        #endregion
    }
}