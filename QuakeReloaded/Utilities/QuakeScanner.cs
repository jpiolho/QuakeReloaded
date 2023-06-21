using Reloaded.Memory.Sigscan.Definitions.Structs;
using Reloaded.Memory.SigScan.ReloadedII.Interfaces;
using System.Diagnostics;

namespace QuakeReloaded.Utilities
{
    internal class QuakeScanner
    {
        private IStartupScanner _scanner;
        private ProcessModule _mainModule;

        public ProcessModule MainModule => _mainModule;

        public QuakeScanner(ProcessModule mainModule, IStartupScanner startupScanner)
        {
            _mainModule = mainModule;
            _scanner = startupScanner;
        }

        public void Scan(string pattern, Action<ProcessModule, PatternScanResult> callback)
        {
            _scanner.AddMainModuleScan(pattern, result => callback(_mainModule, result));
        }


    }
}
