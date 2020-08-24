using Fsp;
using StorageBackend;
using System;

namespace CrazyFS.CommandLine {
    public class Input {
        public Options Get(string[] Args) {
            var o = new Options();
            try {
                int I;
                for (I = 1; Args.Length > I; I++) {
                    string Arg = Args[I];
                    if ('-' != Arg[0])
                        break;
                    switch (Arg[1]) {
                        case '?':
                            o.ShowHelp = true;
                            break;
                        case 'd':
                            argtol(Args, ref I, ref o.DebugFlags);
                            break;
                        case 'D':
                            argtos(Args, ref I, ref o.LogFile);
                            break;
                        case 'm':
                            argtos(Args, ref I, ref o.MountPoint);
                            o.MountPoint = PathNormalizer.GetDriveLetter(o.MountPoint);
                            break;
                        case 'p':
                            argtos(Args, ref I, ref o.SourcePath);
                            break;
                        case 'u':
                            argtos(Args, ref I, ref o.UNCPrefix);
                            break;
                        default:
                            throw new UsageException();
                    }
                }

                if (Args.Length > I) {
                    throw new UsageException();
                }

                if (string.IsNullOrEmpty(o.SourcePath) || string.IsNullOrEmpty(o.MountPoint)) {
                    throw new UsageException();
                }
            } catch (UsageException) {
                Service.Log(Service.EVENTLOG_ERROR_TYPE, GetHelp("Invalid Usage"));
                throw;
            } catch (Exception ex) {
                Service.Log(Service.EVENTLOG_ERROR_TYPE, string.Format("{0}", ex.Message));
                throw;
            }

            return o;
        }

        private static void argtos(string[] Args, ref int I, ref string V) {
            if (Args.Length > ++I) {
                V = Args[I];
            } else {
                throw new UsageException();
            }
        }

        private static void argtol(string[] Args, ref int I, ref uint V) {
            if (Args.Length > ++I) {
                V = int.TryParse(Args[I], out var R) ? (uint)R : V;
            } else {
                throw new UsageException();
            }
        }

        private string GetHelp(string pMessage) {
            return string.Format(
                "{0}" +
                "usage: {1} OPTIONS\n" +
                "\n" +
                "options:\n" +
                "    -d DebugFlags       [-1: enable all debug logs]\n" +
                "    -D DebugLogFile     [file path; use - for stderr]\n" +
                "    -u \\Server\\Share    [UNC prefix (single backslash)]\n" +
                "    -p Directory        [directory to expose as pass through file system]\n" +
                "    -m MountPoint       [X:|*|directory]\n",
                pMessage + "\n",
                "CrazyFSFilesystem"
            );
        }
    }
}
