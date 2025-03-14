namespace NexPlugin.RendezVousErrorCode
{
	public enum ErrorCode
	{
		ConnectionFailure = 1,
		NotAuthenticated = 2,
		InvalidUsername = 100,
		InvalidPassword = 101,
		UsernameAlreadyExists = 102,
		AccountDisabled = 103,
		AccountExpired = 104,
		ConcurrentLoginDenied = 105,
		EncryptionFailure = 106,
		InvalidPID = 107,
		MaxConnectionsReached = 108,
		InvalidGID = 109,
		InvalidControlScriptID = 110,
		InvalidOperationInLiveEnvironment = 111,
		DuplicateEntry = 112,
		ControlScriptFailure = 113,
		ClassNotFound = 114,
		SessionVoid = 115,
		DDLMismatch = 117,
		InvalidConfiguration = 118,
		SessionFull = 200,
		InvalidGatheringPassword = 201,
		WithoutParticipationPeriod = 202,
		PersistentGatheringCreationMax = 203,
		PersistentGatheringParticipationMax = 204,
		DeniedByParticipants = 205,
		ParticipantInBlockList = 206,
		GameServerMaintenance = 207,
		OutOfRatingRange = 209,
		ConnectionDisconnected = 210,
		InvalidOperation = 211,
		NotParticipatedGathering = 212,
		MatchmakeSessionUserPasswordUnmatch = 213,
		MatchmakeSessionSystemPasswordUnmatch = 214,
		UserIsOffline = 215,
		AlreadyParticipatedGathering = 216,
		PermissionDenied = 217,
		NotFriend = 218,
		SessionClosed = 219,
		DatabaseTemporarilyUnavailable = 220,
		InvalidUniqueId = 221,
		MatchmakingWithdrawn = 222,
		LimitExceeded = 223,
		AccountTemporarilyDisabled = 224,
		PartiallyServiceClosed = 225,
		ConnectionDisconnectedForConcurrentLogin = 226
	}
}
