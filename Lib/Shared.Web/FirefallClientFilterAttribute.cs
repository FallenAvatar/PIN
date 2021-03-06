﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Shared.Web {
	public class FirefallClientFilterAttribute : ActionFilterAttribute {
		public override void OnActionExecuted( ActionExecutedContext ctx ) {
			if( ctx.Result is ObjectResult objectResult ) {
				objectResult.Formatters.Add( new NewtonsoftJsonOutputFormatter(
					new JsonSerializerSettings {
						ContractResolver = new DefaultContractResolver {
							NamingStrategy = new SnakeCaseNamingStrategy()
						}
					},
					ctx.HttpContext.RequestServices.GetRequiredService<ArrayPool<char>>(),
					ctx.HttpContext.RequestServices.GetRequiredService<IOptions<MvcOptions>>().Value ) );
			}
		}
	}
}
