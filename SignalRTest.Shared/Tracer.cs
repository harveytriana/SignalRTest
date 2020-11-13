// ===============================
// ©Copyright VISIONARY S.A.S.
// ===============================
using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SignalRTest.Shared
{
    public class Tracer
    {
        bool _isOpen;
        string _logFile;
        CultureInfo _ci = new CultureInfo("en");

        // Not for Azure, only development

        public void Start(string logFile)
        {
            try {
                _isOpen = false;
                _logFile = logFile;
                if (!_logFile.EndsWith(".log")) _logFile += ".log";
                var p = string.Empty;
                if (_logFile.Contains("\\") || logFile.Contains("/")) {
                    p = Path.GetDirectoryName(logFile);
                } else {
                    p = $@"{Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory)}_TraceLog/";
                    _logFile = p + _logFile;
                    Console.WriteLine($"Exists a Tracer in {LogFile}");
                }
                if (Directory.Exists(p) == false) {
                    Directory.CreateDirectory(p);
                }
                _isOpen = true;
            }
            catch { }
        }

        public void Log(string text, bool useConsole = false)
        {
            if (_isOpen) {
                using (var fs = File.Open(LogFile, FileMode.Append, FileAccess.Write, FileShare.Read))
                using (var tw = new StreamWriter(fs)) {
                    tw.WriteLine($"{DateTime.Now.ToString("dd-MM-yy HH:mm:ss")} {text}");
                }
                if (useConsole) {
                    Console.WriteLine(text);
                }
            }
        }

        string LogFile {
            get {
                if (_logFile != null) {
                    var d = DateTime.Today;
                    if (_logFile.Contains("{Date}")) return _logFile.Replace("{Date}", d.ToString("dd-MM-yyyy"));
                    if (_logFile.Contains("{Month}")) return _logFile.Replace("{Month}", FormatMonth(d));
                    if (_logFile.Contains("{Week}")) {
                        var day = d.Day;
                        if (day <= 7) return _logFile.Replace("{Week}", "Q1-" + FormatMonth(d));
                        if (day <= 14) return _logFile.Replace("{Week}", "Q2-" + FormatMonth(d));
                        if (day <= 21) return _logFile.Replace("{Week}", "Q3-" + FormatMonth(d));
                        return _logFile.Replace("{Week}", "Q4-" + FormatMonth(d));
                    }
                    return _logFile;
                }
                return null;
            }
        }

        //.. a whim and standardize
        private string FormatMonth(DateTime today)
        {
            var f = (DateTimeFormatInfo)_ci.DateTimeFormat.Clone();

            f.AbbreviatedMonthGenitiveNames = f.AbbreviatedMonthGenitiveNames
                                               .Select(m => m.TrimEnd('.'))
                                               .ToArray();
            return today.ToString("MMM-yyyy", f).ToUpper();
        }
    }
}
