using Microsoft.Extensions.DependencyInjection;

namespace Binkus.DependencyInjection;

public interface IProvideServices : IServiceProvider
{
    IServiceProvider Services { get; }
    
    object? IServiceProvider.GetService(Type serviceType) => Services.GetService(serviceType);
}

public static class ProvideServicesExtensions
{
    // required cause default interface implementation of IServiceProvider IServiceProvider.GetService(Type) gets hidden
    /// <summary><inheritdoc cref="IServiceProvider.GetService(Type)"/></summary>
    /// <param name="serviceProviderProvider">The <see cref="IProvideServices"/> to retrieve the
    /// <see cref="IServiceProvider"/> object from to retrieve the service object from.</param>
    /// <param name="serviceType"><inheritdoc cref="IServiceProvider.GetService(Type)"/></param>
    /// <returns><inheritdoc cref="IServiceProvider.GetService(Type)"/></returns>
    public static object? GetService(this IProvideServices serviceProviderProvider, Type serviceType)
        => serviceProviderProvider.Services.GetService(serviceType);
    
    //
    // not required (for simpler access of those basic functions with just one using Binkus.DependencyInjection):
    
    /// <summary><inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService{T}"/></summary>
    /// <typeparam name="TService">The type of service object to get.</typeparam>
    /// <param name="serviceProviderProvider">The <see cref="IProvideServices"/> to retrieve the
    /// <see cref="IServiceProvider"/> object from to retrieve the service object from.</param>
    /// <returns><inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetService{T}"/></returns>
    public static TService? GetService<TService>(this IProvideServices serviceProviderProvider)
        => serviceProviderProvider.Services.GetService<TService>();
    
    //
    
    /// <summary><inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService"/></summary>
    /// <param name="serviceProviderProvider">The <see cref="IProvideServices"/> to retrieve the
    /// <see cref="IServiceProvider"/> object from to retrieve the service object from.</param>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns><inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService"/></returns>
    /// <exception cref="System.InvalidOperationException">
    /// <inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService"/></exception>
    public static object GetRequiredService(this IProvideServices serviceProviderProvider, Type serviceType)
        => serviceProviderProvider.Services.GetRequiredService(serviceType);

    /// <summary><inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService{T}"/></summary>
    /// <typeparam name="TService">The type of service object to get.</typeparam>
    /// <param name="serviceProviderProvider">The <see cref="IProvideServices"/> to retrieve the
    /// <see cref="IServiceProvider"/> object from to retrieve the service object from.</param>
    /// <returns><inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService{T}"/></returns>
    /// <exception cref="System.InvalidOperationException">
    /// <inheritdoc cref="Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService{T}"/></exception>
    public static TService GetRequiredService<TService>(this IProvideServices serviceProviderProvider)
        where TService : notnull => serviceProviderProvider.Services.GetRequiredService<TService>();
}