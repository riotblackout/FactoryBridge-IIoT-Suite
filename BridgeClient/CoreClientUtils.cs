using Opc.Ua;
using Opc.Ua.Client;

public static class CoreClientUtils
{
    public static EndpointDescription SelectEndpoint(string discoveryUrl, bool useSecurity)
    {
        var configuration = EndpointConfiguration.Create();
        configuration.OperationTimeout = 10000;
        
        using (var client = DiscoveryClient.Create(new Uri(discoveryUrl), configuration))
        {
            var endpoints = client.GetEndpoints(null);
            foreach (var endpoint in endpoints)
            {
                if (endpoint.EndpointUrl.StartsWith(discoveryUrl))
                {
                    if (useSecurity && endpoint.SecurityMode == MessageSecurityMode.SignAndEncrypt)
                        return endpoint;
                    if (!useSecurity && endpoint.SecurityMode == MessageSecurityMode.None)
                        return endpoint;
                }
            }
            return endpoints[0]; // Fallback
        }
    }
}