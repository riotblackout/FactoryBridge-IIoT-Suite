using Opc.Ua;
using Opc.Ua.Client;

namespace FactoryBridgeDashboard.Services
{
    public class OpcUaService
    {
        private Session _session;
        private Subscription _subscription;
        // This is the "Event" the website will listen to
        public event Action<string> OnValueUpdated;

        public async Task StartMonitoring()
        {
            var serverUrl = "opc.tcp://milo.digitalpetri.com:62541/milo";
            var targetNodeId = "ns=2;s=Demo.Dynamic.Int32";

            var config = new ApplicationConfiguration()
            {
                ApplicationName = "FactoryBridgeWeb",
                ApplicationUri = Utils.Format(@"urn:{0}:FactoryBridgeWeb", System.Net.Dns.GetHostName()),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = "FactoryBridgeWeb" },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Certificate Authorities" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\UA Applications" },
                    RejectedCertificateStore = new CertificateStoreIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\RejectedCertificates" },
                    AutoAcceptUntrustedCertificates = true,
                    RejectSHA1SignedCertificates = false,
                    MinimumCertificateKeySize = 1024
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 }
            };

            await config.Validate(ApplicationType.Client);
            var endpoint = CoreClientUtils.SelectEndpoint(serverUrl, useSecurity: false);

            _session = await Session.Create(config, new ConfiguredEndpoint(null, endpoint, EndpointConfiguration.Create(config)), false, "", 60000, null, null);

            _subscription = new Subscription(_session.DefaultSubscription) { PublishingInterval = 1000 };

            var item = new MonitoredItem(_subscription.DefaultItem)
            {
                DisplayName = "TurbineSpeed",
                StartNodeId = NodeId.Parse(targetNodeId)
            };

            item.Notification += (monitoredItem, args) =>
            {
                foreach (var value in monitoredItem.DequeueValues())
                {
                    // Notify the website that data changed!
                    OnValueUpdated?.Invoke(value.Value.ToString());
                }
            };

            _subscription.AddItem(item);
            _session.AddSubscription(_subscription);
            _subscription.Create();
        }
    }
}