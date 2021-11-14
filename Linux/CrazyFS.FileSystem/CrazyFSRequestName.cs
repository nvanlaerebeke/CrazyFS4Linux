namespace CrazyFS.FileSystem;

enum CrazyFSRequestName
{
    ChangeTimes,
    Chown,
    Chmod,
    CheckAccess,
    CreateDirectory,
    CreateHardLink,
    CreateSpecialFile,
    CreateSymlink,
    GetFileSystemStatus,
    GetPathExtendedAttribute,
    GetPathInfo,
    GetSymbolicLinkTarget,
    ListPathExtendedAttributes,
    Ls,
    Lock,
    Open,
    Move,
    Mount,
    Read,
    RemoveFile,
    RemoveDirectory,
    RemovePathExtendedAttribute,
    SetPathExtendedAttribute,
    Truncate,
    UnMount,
    Write
}