using Opc.Ua;
using Opc.Ua.Client;

Console.WriteLine("FactoryBridge: Connecting to Cloud Machine...");

// 1. Define Server and Target Node
string serverUrl = "opc.tcp://milo.digitalpetri.com:62541/milo";
// THIS is the specific address you found in UaExpert:
string targetNodeId = "ns=2;s=Demo.Dynamic.Int32";

// 2. Setup Configuration (The Driver)
var config = new ApplicationConfiguration()
{
    ApplicationName = "FactoryBridgeClient",
    ApplicationUri = Utils.Format(@"urn:{0}:FactoryBridgeClient", System.Net.Dns.GetHostName()),
    ApplicationType = ApplicationType.Client,
    SecurityConfiguration = new SecurityConfiguration
    {
        ApplicationCertificate = new CertificateIdentifier { StoreType = @"Directory", StorePath = @"%CommonApplicationData%\OPC Foundation\CertificateStores\MachineDefault", SubjectName = "FactoryBridgeClient" },
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

// 3. Connect
var endpoint = CoreClientUtils.SelectEndpoint(serverUrl, useSecurity: false);
using (var session = await Session.Create(config, new ConfiguredEndpoint(null, endpoint, EndpointConfiguration.Create(config)), false, "", 60000, null, null))
{
    Console.WriteLine($"CONNECTED to {serverUrl}");
    Console.WriteLine("Creating Subscription (Listening for updates)...");

    // 4. Create Subscription (Check every 1000ms)
    var subscription = new Subscription(session.DefaultSubscription) { PublishingInterval = 1000 };

    // 5. Add the Item to Watch
    var list = new List<MonitoredItem> {
        new MonitoredItem(subscription.DefaultItem)
        {
            DisplayName = "TurbineSpeed",
            StartNodeId = NodeId.Parse(targetNodeId) // Parsing your specific ID
        }
    };

    // 6. Define the "On Change" Event
    list.ForEach(i => i.Notification += (item, e) => {
        foreach (var value in item.DequeueValues())
        {
            // This prints every time the cloud server updates the value
            Console.WriteLine($"[LIVE DATA] {item.DisplayName}: {value.Value} | Time: {value.SourceTimestamp.ToLocalTime()}");
        }
    });

    subscription.AddItems(list);
    session.AddSubscription(subscription);
    subscription.Create();

    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("   LISTENING LIVE. Press any key to stop.");
    Console.WriteLine("------------------------------------------------");

    Console.ReadKey();
}