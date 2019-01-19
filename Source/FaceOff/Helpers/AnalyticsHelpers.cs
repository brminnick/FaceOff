using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FaceOff
{
    public static class AnalyticsHelpers
    {
        #region Methods
        public static void Start() => Start(AnalyticsConstants.AppCenterApiKey);

        public static void Track(string trackIdentifier, IDictionary<string, string> table = null) => Analytics.TrackEvent(trackIdentifier, table);

        public static void Track(string trackIdentifier, string key, string value) => Analytics.TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });

        public static TimedEvent TrackTime(string trackIdentifier, IDictionary<string, string> table = null) => new TimedEvent(trackIdentifier, table);

        public static TimedEvent TrackTime(string trackIdentifier, string key, string value) => TrackTime(trackIdentifier, new Dictionary<string, string> { { key, value } });

        public static void Report(Exception exception,
                                  IDictionary<string, string> properties = null,
                                  [CallerMemberName] string callerMemberName = "",
                                  [CallerLineNumber] int lineNumber = 0,
                                  [CallerFilePath] string filePath = "")
        {
            PrintException(exception, callerMemberName, lineNumber, filePath);

            Crashes.TrackError(exception, properties);
        }

        [Conditional("DEBUG")]
        static void PrintException(Exception exception, string callerMemberName, int lineNumber, string filePath)
        {
            var fileName = System.IO.Path.GetFileName(filePath);

            Debug.WriteLine(exception.GetType());
            Debug.WriteLine($"Error: {exception.Message}");
            Debug.WriteLine($"Line Number: {lineNumber}");
            Debug.WriteLine($"Caller Name: {callerMemberName}");
            Debug.WriteLine($"File Name: {fileName}");
        }

        static void Start(string appCenterAPIKey) => AppCenter.Start(appCenterAPIKey, typeof(Crashes), typeof(Analytics));
        #endregion
    }

    #region Classes
    public class TimedEvent : IDisposable
    {
        readonly Stopwatch _stopwatch;
        readonly string _trackIdentifier;

        public TimedEvent(string trackIdentifier, IDictionary<string, string> dictionary)
        {
            Data = dictionary ?? new Dictionary<string, string>();
            _trackIdentifier = trackIdentifier;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public IDictionary<string, string> Data { get; }

        public void Dispose()
        {
            _stopwatch.Stop();
            Data.Add("Timed Event", $"{_stopwatch.Elapsed.ToString(@"ss\.fff")}s");
            AnalyticsHelpers.Track($"{_trackIdentifier} [Timed Event]", Data);
        }
    }
    #endregion
}

