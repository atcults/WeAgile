using System;
using Microsoft.Extensions.DependencyInjection;

namespace BuildMaster.Infrastructure
{
    public class ServiceCollectionProvider
    {
        private IServiceCollection _serviceCollection;
        private IServiceProvider _serviceProvider;
        private static readonly Lazy<ServiceCollectionProvider> Lazy = new Lazy<ServiceCollectionProvider>(() =>
        {
            var details = new ServiceCollectionProvider();
            return details;
        });

        public static ServiceCollectionProvider Instance => Lazy.Value;

        private ServiceCollectionProvider()
        {
            _serviceCollection = new ServiceCollection();
        }

        public IServiceCollection Collections
        {
            get
            {
                return _serviceCollection;
            }
        }

        public IServiceProvider Provider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = _serviceCollection.BuildServiceProvider();
                }

                return _serviceProvider;
            }
        }
    }
}