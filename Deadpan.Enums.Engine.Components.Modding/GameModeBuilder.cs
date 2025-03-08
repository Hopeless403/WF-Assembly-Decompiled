namespace Deadpan.Enums.Engine.Components.Modding
{
	public class GameModeBuilder : DataFileBuilder<GameMode, GameModeBuilder>
	{
		public GameModeBuilder(WildfrostMod mod)
			: base(mod)
		{
		}

		public GameModeBuilder()
		{
		}

		public GameModeBuilder WithSaveFileName(string saveFileName)
		{
			_data.saveFileName = saveFileName;
			return this;
		}

		public GameModeBuilder WithSeed(string seed)
		{
			_data.seed = seed;
			return this;
		}

		public GameModeBuilder WithClasses(params ClassData[] classes)
		{
			_data.classes = classes;
			return this;
		}

		public GameModeBuilder WithGenerator(CampaignGenerator generator)
		{
			_data.generator = generator;
			return this;
		}

		public GameModeBuilder WithPopulator(CampaignPopulator populator)
		{
			_data.populator = populator;
			return this;
		}

		public GameModeBuilder WithStartInNode(bool startInNode)
		{
			_data.startInNode = startInNode;
			return this;
		}

		public GameModeBuilder WithTakeStartingPet(bool takeStartingPet = true)
		{
			_data.takeStartingPet = takeStartingPet;
			return this;
		}

		public GameModeBuilder WithCountsAsWin(bool countsAsWin = true)
		{
			_data.countsAsWin = countsAsWin;
			return this;
		}

		public GameModeBuilder WithShowStats(bool showStats = true)
		{
			_data.showStats = showStats;
			return this;
		}

		public GameModeBuilder WithGainProgress(bool gainProgress = true)
		{
			_data.gainProgress = gainProgress;
			return this;
		}

		public GameModeBuilder WithDoSave(bool doSave = true)
		{
			_data.doSave = doSave;
			return this;
		}

		public GameModeBuilder WithCanRestart(bool canRestart = true)
		{
			_data.canRestart = canRestart;
			return this;
		}

		public GameModeBuilder WithCanGoBack(bool canGoBack = true)
		{
			_data.canGoBack = canGoBack;
			return this;
		}

		public GameModeBuilder WithSubmitScore(bool submitScore = false)
		{
			_data.submitScore = submitScore;
			return this;
		}

		public GameModeBuilder WithMainGameMode(bool mainGameMode = true)
		{
			_data.mainGameMode = mainGameMode;
			return this;
		}

		public GameModeBuilder WithDailyRun(bool dailyRun = false)
		{
			_data.dailyRun = dailyRun;
			return this;
		}

		public GameModeBuilder WithTutorialRun(bool tutorialRun = false)
		{
			_data.tutorialRun = tutorialRun;
			return this;
		}

		public GameModeBuilder WithLeaderboardType(Scores.Type leaderboardType)
		{
			_data.leaderboardType = leaderboardType;
			return this;
		}

		public GameModeBuilder WithStartScene(string startScene)
		{
			_data.startScene = startScene;
			return this;
		}

		public GameModeBuilder WithSceneAfterSelection(string sceneAfterSelection = "Campaign")
		{
			_data.sceneAfterSelection = sceneAfterSelection;
			return this;
		}

		public GameModeBuilder WithCampaignSystemNames(params string[] campaignSystemNames)
		{
			_data.campaignSystemNames = campaignSystemNames;
			return this;
		}

		public GameModeBuilder WithSystemsToDisable(params string[] systemsToDisable)
		{
			_data.systemsToDisable = systemsToDisable;
			return this;
		}
	}
}
