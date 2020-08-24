using System;

namespace CrazyFS {
    internal static class Program {
        private static void Main(string[] args) {
            Environment.ExitCode = new CrazyFSService().Run();
        }
    }
}
