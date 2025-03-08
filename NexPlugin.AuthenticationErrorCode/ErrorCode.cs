namespace NexPlugin.AuthenticationErrorCode
{
	public enum ErrorCode
	{
		NASAuthenticateError = 1,
		TokenParseError,
		HttpConnectionError,
		HttpDNSError,
		HttpGetProxySetting,
		TokenExpired,
		ValidationFailed,
		InvalidParam,
		PrincipalIdUnmatched,
		MoveCountUnmatch,
		UnderMaintenance,
		UnsupportedVersion,
		ServerVersionIsOld,
		Unknown,
		ClientVersionIsOld,
		AccountLibraryError,
		ServiceNoLongerAvailable,
		UnknownApplication,
		ApplicationVersionIsOld,
		OutOfService,
		NetworkServiceLicenseRequired,
		NetworkServiceLicenseSystemError,
		NetworkServiceLicenseError3,
		NetworkServiceLicenseError4
	}
}
