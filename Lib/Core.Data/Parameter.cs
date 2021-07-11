using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data {
	public class Parameter : IParameter {
		public string Name { get; init; }
		public object? Value { get; init; }

		public Parameter( string name, object? val ) {
			Name = name;
			Value = val;
		}
	}
	public class Parameter<T> : IParameter<T> {
		public T? Value { get; init; }
		public string Name { get; init; }
		object IParameter.Value { get { return Value; } }

		public Parameter( string name, T val ) {
			Name = name;
			Value = val;
		}
	}
}
