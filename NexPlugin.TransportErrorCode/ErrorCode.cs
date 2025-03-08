namespace NexPlugin.TransportErrorCode
{
	public enum ErrorCode
	{
		Unknown = 1,
		ConnectionFailure = 2,
		InvalidUrl = 3,
		InvalidKey = 4,
		InvalidURLType = 5,
		DuplicateEndpoint = 6,
		IOError = 7,
		Timeout = 8,
		ConnectionReset = 9,
		IncorrectRemoteAuthentication = 10,
		ServerRequestError = 11,
		DecompressionFailure = 12,
		ReliableSendBufferFullFatal = 13,
		UPnPCannotInit = 14,
		UPnPCannotAddMapping = 15,
		NatPMPCannotInit = 16,
		NatPMPCannotAddMapping = 17,
		UnsupportedNAT = 19,
		DnsError = 20,
		ProxyError = 21,
		DataRemaining = 22,
		NoBuffer = 23,
		NotFound = 24,
		TemporaryServerError = 25,
		PermanentServerError = 26,
		ServiceUnavailable = 27,
		ReliableSendBufferFull = 28,
		InvalidStation = 29,
		InvalidSubStreamID = 30,
		PacketBufferFull = 31,
		NatTraversalError = 32,
		NatCheckError = 33
	}
}
