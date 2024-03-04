namespace SFA.DAS.EmployerCommitmentsV2.Contracts;

public interface IOuterApiClient
{
    Task<TResponse> Get<TResponse>(string url);
    Task<TResponse> Post<TResponse>(string url, object data);
    Task<TResponse> Put<TResponse>(string url, object data);
}