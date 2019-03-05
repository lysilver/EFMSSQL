using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;

namespace EFMSSQL
{
	public class OperationAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			var actionName = actionContext.ActionDescriptor.ActionName;
			var controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
			int parametersCount = actionContext.ActionParameters.Count;
			if (parametersCount > 0)
			{
				var keys = actionContext.ActionParameters.Keys;
				var t = actionContext.ActionParameters[""];
			}

			base.OnActionExecuting(actionContext);
		}
	}
}