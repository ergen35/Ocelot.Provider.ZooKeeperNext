﻿namespace Ocelot.Provider.ZooKeeperNext
{
    using System.Threading.Tasks;
    using Logging;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using ServiceDiscovery;

    public static class ZookeeperProviderFactory
    {
        public static ServiceDiscoveryFinderDelegate Get = (provider, config, name) =>
        {
            var factory = provider.GetService<IOcelotLoggerFactory>();

            var ZookeeperFactory = provider.GetService<IZookeeperClientFactory>();

            var ZookeeperRegistryConfiguration = new ZookeeperRegistryConfiguration(config.Host, config.Port, name.ServiceName);

            var ZookeeperServiceDiscoveryProvider = new Zookeeper(ZookeeperRegistryConfiguration, factory, ZookeeperFactory);

            if (config.Type?.ToLower() == "pollzookeeper")
            {
                return new PollZookeeper(config.PollingInterval, factory, ZookeeperServiceDiscoveryProvider);
            }

            return ZookeeperServiceDiscoveryProvider;
        };
    }
}