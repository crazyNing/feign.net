﻿using Feign.Cache;
using Feign.Discovery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feign
{
    static class ServiceDiscoveryExtensions
    {
        public static async Task<IList<IServiceInstance>> GetServiceInstancesWithCacheAsync(this IServiceDiscovery serviceDiscovery, string serviceId, ICacheProvider cacheProvider, string serviceInstancesKeyPrefix = "ServiceDiscovery-ServiceInstances-")
        {
            // if distributed cache was provided, just make the call back to the provider
            if (cacheProvider != null)
            {
                // check the cache for existing service instances
                var services = await cacheProvider.GetAsync<List<SerializableIServiceInstance>>(serviceInstancesKeyPrefix + serviceId);
                if (services != null && services.Count > 0)
                {
                    return services.ToList<IServiceInstance>();
                }
            }

            // cache not found or instances not found, call out to the provider
            var instances = serviceDiscovery.GetServiceInstances(serviceId) ?? new List<IServiceInstance>();
            if (cacheProvider != null)
            {
                List<SerializableIServiceInstance> cacheValue = instances.Select(i => new SerializableIServiceInstance(i)).ToList();
                await cacheProvider.SetAsync(serviceInstancesKeyPrefix + serviceId, cacheValue, TimeSpan.FromMinutes(10));
            }

            return instances;
        }

        public static IList<IServiceInstance> GetServiceInstancesWithCache(this IServiceDiscovery serviceDiscovery, string serviceId, ICacheProvider cacheProvider, string serviceInstancesKeyPrefix = "ServiceDiscovery-ServiceInstances-")
        {
            // if distributed cache was provided, just make the call back to the provider
            if (cacheProvider != null)
            {
                // check the cache for existing service instances
                var services = cacheProvider.Get<List<SerializableIServiceInstance>>(serviceInstancesKeyPrefix + serviceId);
                if (services != null && services.Count > 0)
                {
                    return services.ToList<IServiceInstance>();
                }
            }

            // cache not found or instances not found, call out to the provider
            var instances = serviceDiscovery.GetServiceInstances(serviceId) ?? new List<IServiceInstance>();
            if (cacheProvider != null)
            {
                List<SerializableIServiceInstance> cacheValue = instances.Select(i => new SerializableIServiceInstance(i)).ToList();
                cacheProvider.Set(serviceInstancesKeyPrefix + serviceId, cacheValue, TimeSpan.FromMinutes(10));
            }

            return instances;
        }

    }
}
