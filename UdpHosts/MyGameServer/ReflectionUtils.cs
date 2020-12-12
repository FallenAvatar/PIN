﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Shared.Common.Extensions;

using MyGameServer.Extensions;

namespace MyGameServer {
	public static class ReflectionUtils {
		public static IEnumerable<System.Type> FindTypesByAttribute<T>() where T : Attribute {
			return (from t in Assembly.GetExecutingAssembly().GetTypes()
					where t.GetAttribute<T>() != null
					select t);
		}

		public static IEnumerable<MethodInfo> FindMethodsByAttribute<T>(object obj) where T : Attribute {
			return (from m in obj.GetType().GetMethods()
					where m.GetAttribute<T>() != null
					select m);
		}
	}
}
