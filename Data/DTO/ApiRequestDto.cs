namespace ECommerceApp.RyanW84.Data.DTO;

/// <summary>
/// Standard request wrapper for API endpoints.
/// The payload is modeled as a nullable value to support validation and consistent error responses.
/// </summary>
/// <typeparam name="T">The request payload type.</typeparam>
public record ApiRequestDto<T>(T? Payload);
