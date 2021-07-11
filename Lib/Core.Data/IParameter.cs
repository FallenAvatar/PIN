
namespace Core.Data {
	public interface IParameter {
		string Name { get; }
		object? Value { get; }
	}

	public interface IParameter<T> : IParameter {
		new T? Value { get; }
	}
}
