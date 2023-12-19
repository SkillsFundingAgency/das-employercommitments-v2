using SFA.DAS.EmployerCommitmentsV2.Contracts;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public class AuthorizationContext : IAuthorizationContext
{
    private readonly Dictionary<string, object> _data = new(StringComparer.InvariantCultureIgnoreCase);
        
    public T Get<T>(string key)
    {
        if (!_data.TryGetValue(key, out var value))
        {
            throw new KeyNotFoundException($"The key '{key}' was not present in the authorization context");
        }

        return (T)value;
    }

    public void Set<T>(string key, T value)
    {
        _data[key] = value;
    }

    public bool TryGet<T>(string key, out T value)
    {
        var exists = _data.TryGetValue(key, out var obj);

        value = exists ? (T)obj : default;

        return exists;
    }

    public override string ToString()
    {
        return _data.Count > 0 ? string.Join(", ", _data.Select(d => $"{d.Key}: {d.Value}")) : "None";
    }
}