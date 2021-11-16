using System;
// ReSharper disable InconsistentNaming

namespace CrazyFS.FileSystem
{
    [Flags]
    public enum PathAccessModes
    {
        R_OK = 1,
        W_OK = 2,
        X_OK = 4,
        F_OK = 8,
    }
}