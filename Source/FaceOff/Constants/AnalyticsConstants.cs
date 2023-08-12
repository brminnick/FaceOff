﻿using System;
using Xamarin.Forms;

namespace FaceOff
{
	public static class AnalyticsConstants
	{
		public const string GameStarted = "Game Started";
		public const string PhotoButton1Tapped = "Photo Button 1 Tapped";
		public const string PhotoButton2Tapped = "Photo Button 2 Tapped";
		public const string PhotoTaken = "Photo Taken";
		public const string Player1NameEmpty = "Player 1 Name Empty";
		public const string Player2NameEmpty = "Player 2 Name Empty";
		public const string ResetButtonTapped = "Reset Button Tapped";
		public const string ResultsButton1Tapped = "Results Button 1 Tapped";
		public const string ResultsButton2Tapped = "Results Button 2 Tapped";
		public const string StartGameButtonTapped = "Start Game Button Tapped";
		public const string StartGameButtonTappedStatus = "Start Game Button Tapped Status";

		public const string AnalyzeEmotion = "Analyze Emotion";

		const string AppCenterApiKey_iOS = "11275693-c46c-43b8-bae7-9a60432f7c3c";
		const string AppCenterApiKey_Android = "d879da67-619a-4928-94fc-f9ae217223ff";
		const string AppCenterApiKey_UWP = "4e34b8ba-f0dd-474f-9909-ab538c376dbd";

		public static string AppCenterApiKey => GetApiKey();

		static string GetApiKey() => Device.RuntimePlatform switch
		{
			Device.iOS => AppCenterApiKey_iOS,
			Device.Android => AppCenterApiKey_Android,
			Device.UWP => AppCenterApiKey_UWP,
			_ => throw new NotSupportedException($"{nameof(Device.RuntimePlatform)}, {Device.RuntimePlatform}, not supported"),
		};
	}
}