using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace FaceOff
{
    static class AnalyticsService
    {
        public static void Start() => Start(AnalyticsConstants.AppCenterApiKey);

        public static void Track(in string trackIdentifier, in IDictionary<string, string>? table = null) => Analytics.TrackEvent(trackIdentifier, table);

        public static void Track(in string trackIdentifier, in string key, in string value) => Analytics.TrackEvent(trackIdentifier, new Dictionary<string, string> { { key, value } });

        public static TimedEvent TrackTime(in string trackIdentifier, in IDictionary<string, string>? table = null) => new TimedEvent(trackIdentifier, table);

        public static TimedEvent TrackTime(in string trackIdentifier, in string key, in string value) => TrackTime(trackIdentifier, new Dictionary<string, string> { { key, value } });

        public static void Report(in Exception exception,
                                  in IDictionary<string, string>? properties = null,
                                  [CallerMemberName] in string callerMemberName = "",
                                  [CallerLineNumber] in int lineNumber = 0,
                                  [CallerFilePath] in string filePath = "")
        {
            PrintException(exception, callerMemberName, lineNumber, filePath);

            Crashes.TrackError(exception, properties);
        }

        [Conditional("DEBUG")]
        static void PrintException(in Exception exception, in string callerMemberName, in int lineNumber, in string filePath)
        {
            var fileName = System.IO.Path.GetFileName(filePath);

            Debug.WriteLine(exception.GetType());
            Debug.WriteLine($"Error: {exception.Message}");
            Debug.WriteLine($"Line Number: {lineNumber}");
            Debug.WriteLine($"Caller Name: {callerMemberName}");
            Debug.WriteLine($"File Name: {fileName}");
        }

        static void Start(in string appCenterAPIKey) => AppCenter.Start(appCenterAPIKey, typeof(Crashes), typeof(Analytics));

        public class TimedEvent : IDisposable
        {
            readonly Stopwatch _stopwatch;
            readonly string _trackIdentifier;

            public TimedEvent(string trackIdentifier, IDictionary<string, string>? dictionary = null)
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
                Data.Add("Timed Event", $"{_stopwatch.Elapsed:ss\\.fff}s");
                AnalyticsService.Track($"{_trackIdentifier} [Timed Event]", Data);
            }
        }
    }
}

