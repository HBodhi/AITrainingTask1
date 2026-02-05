using MomentusEventApi.Models;
using Ungerboeck.Api.Models.Search;

namespace MomentusEventApi.Services;

public class ServiceOrderService : IServiceOrderService
{
    private readonly IUngerboeckApiClientFactory _clientFactory;
    private readonly ILogger<ServiceOrderService> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _defaultOrganization;
    private readonly bool _useMockData;

    public ServiceOrderService(
        IUngerboeckApiClientFactory clientFactory, 
        ILogger<ServiceOrderService> logger, 
        IConfiguration configuration)
    {
        _clientFactory = clientFactory;
        _logger = logger;
        _configuration = configuration;
        _defaultOrganization = configuration["UngerboeckApi:DefaultOrganization"] ?? "10";
        
        // Check if we should use mock data (for testing without real API credentials)
        _useMockData = configuration.GetValue<bool>("UngerboeckApi:UseMockData", false);
        
        if (_useMockData)
        {
            _logger.LogWarning("ServiceOrderService is configured to use MOCK DATA. Set UngerboeckApi:UseMockData to false to use real API.");
        }
    }

    public async Task<IEnumerable<ServiceOrder>> GetAllOrdersAsync(int eventId)
    {
        try
        {
            _logger.LogInformation("Fetching all service orders for event {EventId} from Ungerboeck API", eventId);
            
            if (_useMockData)
            {
                _logger.LogDebug("Returning mock data");
                return GetMockOrders(eventId);
            }

            // Use the SDK to fetch orders for a specific event
            var client = _clientFactory.CreateClient();
            
            // Search for orders by event ID
            var searchFilter = $"Event eq {eventId}";
            var searchResponse = await Task.Run(() => 
                client.Endpoints.ServiceOrders.Search(_defaultOrganization, searchFilter));
            
            var resultCount = searchResponse?.Results != null ? searchResponse.Results.Count() : 0;
            _logger.LogInformation("Retrieved {Count} service orders from Ungerboeck API", resultCount);
            
            if (searchResponse == null || searchResponse.Results == null)
            {
                _logger.LogWarning("No service orders found or null response from Ungerboeck API");
                return new List<ServiceOrder>();
            }
            
            // Convert OrdersModel to ServiceOrder (our wrapper class)
            return searchResponse.Results.Select(MapToServiceOrder).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching service orders from Ungerboeck API");
            throw;
        }
    }

    public async Task<ServiceOrder?> GetOrderByNumberAsync(int orderNumber)
    {
        try
        {
            _logger.LogInformation("Fetching service order {OrderNumber} from Ungerboeck API for organization {Organization}", 
                orderNumber, _defaultOrganization);
            
            if (_useMockData)
            {
                _logger.LogDebug("Returning mock data");
                var allOrders = GetMockOrders(null);
                return await Task.FromResult(allOrders.FirstOrDefault(o => o.OrderNumber == orderNumber));
            }

            // Use the SDK to fetch a specific order
            var client = _clientFactory.CreateClient();
            
            var orderModel = await Task.Run(() => 
                client.Endpoints.ServiceOrders.Get(_defaultOrganization, orderNumber));
            
            if (orderModel == null)
            {
                _logger.LogWarning("Service order {OrderNumber} not found in organization {Organization}", 
                    orderNumber, _defaultOrganization);
                return null;
            }

            _logger.LogInformation("Retrieved service order {OrderNumber}: {OrderSearch}", 
                orderNumber, orderModel.OrderSearch);
            
            // Convert OrdersModel to ServiceOrder
            return MapToServiceOrder(orderModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching service order {OrderNumber} from Ungerboeck API", orderNumber);
            throw;
        }
    }

    public async Task<IEnumerable<ServiceOrder>> SearchOrdersAsync(string searchFilter)
    {
        try
        {
            _logger.LogInformation("Searching service orders in Ungerboeck API for organization {Organization} with filter: {Filter}", 
                _defaultOrganization, searchFilter);
            
            if (_useMockData)
            {
                _logger.LogDebug("Returning mock data");
                var allOrders = GetMockOrders(null);
                
                // Simple filtering on mock data
                if (string.IsNullOrEmpty(searchFilter))
                    return allOrders;
                
                return allOrders.Where(o => 
                    o.OrderSearch?.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) == true ||
                    o.Account?.Contains(searchFilter, StringComparison.OrdinalIgnoreCase) == true
                ).ToList();
            }

            // Use the SDK to search orders
            var client = _clientFactory.CreateClient();
            
            // Search with the provided filter
            var searchResponse = await Task.Run(() => 
                client.Endpoints.ServiceOrders.Search(_defaultOrganization, searchFilter ?? ""));
            
            var resultCount = searchResponse?.Results != null ? searchResponse.Results.Count() : 0;
            _logger.LogInformation("Search returned {Count} service orders from Ungerboeck API", resultCount);
            
            if (searchResponse == null || searchResponse.Results == null)
            {
                _logger.LogWarning("No service orders found or null response from Ungerboeck API");
                return new List<ServiceOrder>();
            }
            
            // Convert OrdersModel to ServiceOrder (our wrapper class)
            return searchResponse.Results.Select(MapToServiceOrder).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching service orders from Ungerboeck API");
            throw;
        }
    }

    /// <summary>
    /// Maps a ServiceOrdersModel from the SDK to our ServiceOrder wrapper class
    /// </summary>
    private static ServiceOrder MapToServiceOrder(Ungerboeck.Api.Models.Subjects.ServiceOrdersModel orderModel)
    {
        // Since ServiceOrder inherits from ServiceOrdersModel, we can create a new instance
        // and copy all properties from the SDK model
        var serviceOrder = new ServiceOrder
        {
            OrganizationCode = orderModel.OrganizationCode,
            OrderNumber = orderModel.OrderNumber,
            Event = orderModel.Event,
            Function = orderModel.Function,
            Account = orderModel.Account,
            OrderStatus = orderModel.OrderStatus,
            OrderDate = orderModel.OrderDate,
            BillToAccount = orderModel.BillToAccount,
            Contact = orderModel.Contact,
            EnteredDateTime = orderModel.EnteredDateTime,
            Category = orderModel.Category,
            OrderSearch = orderModel.OrderSearch
        };
        
        return serviceOrder;
    }

    private IEnumerable<ServiceOrder> GetMockOrders(int? eventId)
    {
        // Mock data simulating Momentus service order data
        var allOrders = new List<ServiceOrder>
        {
            new ServiceOrder
            {
                OrganizationCode = "10",
                OrderNumber = 1001,
                Event = 1,
                Function = 101,
                Account = "TECHCONF",
                OrderSearch = "AV Equipment Setup",
                OrderStatus = "A",
                OrderDate = new DateTime(2026, 6, 1),
                BillToAccount = "TECHCONF",
                Contact = "TECHCONT",
                Category = 1,
                EnteredDateTime = new DateTime(2026, 6, 1, 10, 0, 0)
            },
            new ServiceOrder
            {
                OrganizationCode = "10",
                OrderNumber = 1002,
                Event = 1,
                Function = 101,
                Account = "TECHCONF",
                OrderSearch = "Catering - Lunch Service",
                OrderStatus = "A",
                OrderDate = new DateTime(2026, 6, 1),
                BillToAccount = "TECHCONF",
                Contact = "TECHCONT",
                Category = 2,
                EnteredDateTime = new DateTime(2026, 6, 1, 10, 30, 0)
            },
            new ServiceOrder
            {
                OrganizationCode = "10",
                OrderNumber = 1003,
                Event = 2,
                Function = 201,
                Account = "MUSICFEST",
                OrderSearch = "Stage Setup and Lighting",
                OrderStatus = "A",
                OrderDate = new DateTime(2026, 7, 1),
                BillToAccount = "MUSICFEST",
                Contact = "MUSICCONT",
                Category = 3,
                EnteredDateTime = new DateTime(2026, 7, 1, 9, 0, 0)
            }
        };

        if (eventId.HasValue)
        {
            return allOrders.Where(o => o.Event == eventId.Value).ToList();
        }

        return allOrders;
    }
}
