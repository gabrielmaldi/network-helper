namespace NetworkHelper.Utilities
{
    public enum CmdRouteManagementResultCode
    {
        Success = 0,
        ErrorUnknown = 1,
        ErrorNoActiveNetworkConnection = 2,
        ErrorRouteAlreadyExists = 3,
        ErrorRouteNotFound = 4
    }
}