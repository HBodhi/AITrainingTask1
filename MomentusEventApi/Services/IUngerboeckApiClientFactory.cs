using Ungerboeck.Api.Sdk;

namespace MomentusEventApi.Services;

/// <summary>
/// Interface for UngerboeckApiClient wrapper
/// </summary>
public interface IUngerboeckApiClientFactory
{
    /// <summary>
    /// Creates and returns an authenticated ApiClient instance
    /// </summary>
    ApiClient CreateClient();
}
