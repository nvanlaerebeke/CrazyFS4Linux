using System.Collections.Generic;

namespace CrazyFS.FileSystem;

class CrazyFSRequest
{
    public CrazyFSRequestName Name { get; }
    public Dictionary<string, string> Parameters { get; }

    public CrazyFSRequest(CrazyFSRequestName name, KeyValuePair<string, string>[] parameters)
    {
        Name = name;
        Parameters = new Dictionary<string, string>(parameters);
    }
}