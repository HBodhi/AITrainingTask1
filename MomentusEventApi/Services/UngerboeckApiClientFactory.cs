using Ungerboeck.Api.Sdk;
using Ungerboeck.Api.Models.Authorization;

namespace MomentusEventApi.Services;

/// <summary>
/// Factory for creating authenticated UngerboeckApiClient instances
/// </summary>
public class UngerboeckApiClientFactory : IUngerboeckApiClientFactory
{
    private readonly Jwt _authSettings;
    private readonly ILogger<UngerboeckApiClientFactory> _logger;

    public UngerboeckApiClientFactory(IConfiguration configuration, ILogger<UngerboeckApiClientFactory> logger)
    {
        _logger = logger;
        
        // Read configuration
        var baseUrl = configuration["UngerboeckApi:BaseUrl"] 
            ?? throw new InvalidOperationException("UngerboeckApi:BaseUrl configuration is required");
        var apiUserId = configuration["UngerboeckApi:ApiUserId"] 
            ?? throw new InvalidOperationException("UngerboeckApi:ApiUserId configuration is required");
        var secret = configuration["UngerboeckApi:Secret"] 
            ?? throw new InvalidOperationException("UngerboeckApi:Secret configuration is required");
        var key = configuration["UngerboeckApi:Key"] 
            ?? throw new InvalidOperationException("UngerboeckApi:Key configuration is required");

        // Configure JWT authentication
        _authSettings = new Jwt
        {
            APIUserID = apiUserId,
            Secret = secret,
            Key = key,
            UngerboeckURI = baseUrl,
            AutoRefresh = new AutoRefresh()
        };

        _logger.LogInformation("UngerboeckApiClientFactory configured with BaseUrl: {BaseUrl}, ApiUserId: {ApiUserId}", 
            baseUrl, apiUserId);
    }

    public ApiClient CreateClient()
    {
        try
        {
            _logger.LogDebug("Creating Ungerboeck API client for {UngerboeckURI}", _authSettings.UngerboeckURI);
            
            // Create the API client with JWT authentication
            var client = new ApiClient(_authSettings);
            
            _logger.LogDebug("Ungerboeck API client created successfully");
            
            return client;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create Ungerboeck API client");
            throw;
        }
    }
}
