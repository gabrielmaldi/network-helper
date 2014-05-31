namespace NetworkHelper.Utilities
{
    public enum WinApiRouteManagementResult
    {
        //Windows API error codes
        Success = 0,
        ErrorRouteNotFound = 1168,
        ErrorRouteAlreadyExists = 5010,

        //Custom error codes
        ErrorUnknown = int.MaxValue - 1000,
        ErrorCouldNotFindNetworkInterface = ErrorUnknown + 1,
        ErrorCouldNotFindNetworkInterfaceMetric = ErrorCouldNotFindNetworkInterface + 1
    }
}