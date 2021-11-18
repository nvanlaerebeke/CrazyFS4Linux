using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CrazyFS.FileSystem;
using CrazyFS.Passthrough.Linux;
using Fuse.NET;
using Mono.Unix.Native;
using OpenFlags = CrazyFS.FileSystem.OpenFlags;

namespace CrazyFS {
	internal class CrazyFsFileSystem : Fuse.NET.FileSystem {
        public string MountPoint { get; }

        private readonly LinuxPassthroughFileSystem _fileSystem;
		public CrazyFsFileSystem (string source, string destination)
		{
			MountPoint = destination;
			_fileSystem = new LinuxPassthroughFileSystem(new PassthroughFileSystem(source, destination));
		}

		protected override Errno OnGetPathStatus (string path, out Stat buf)
		{
			buf = new Stat();
			var result = _fileSystem.GetPathInfo(path, out var info);
			if (result.Status == ResultStatus.Success)
			{
				buf = info.ToStat();
			}
			return result.ToErrno();
		}

		protected override Errno OnAccessPath (string path, AccessModes mask)
		{
			return _fileSystem.CheckAccess(path, mask).ToErrno();
		}

		protected override Errno OnReadSymbolicLink (string path, out string target)
		{

			return _fileSystem.GetSymbolicLinkTarget(path, out target).ToErrno();
		}

		protected override Errno OnReadDirectory (string path, OpenedPathInfo fi, out IEnumerable<DirectoryEntry> paths)
		{
			var result = _fileSystem.Ls(path, out var items).ToErrno();
			paths = items.ToList().ConvertAll(x => new DirectoryEntry(x.Name));
			return result;
		}

		protected override Errno OnCreateSpecialFile (string path, FilePermissions mode, ulong rdev)
		{
			return _fileSystem.CreateSpecialFile(path, mode, rdev).ToErrno();
		}

		protected override Errno OnCreateDirectory (string path, FilePermissions mode)
		{
			return _fileSystem.CreateDirectory(path, mode).ToErrno();
		}

		protected override Errno OnRemoveFile (string path)
		{
			return _fileSystem.RemoveFile(path).ToErrno();
		}

		protected override Errno OnRemoveDirectory (string path)
		{
			return _fileSystem.RemoveDirectory(path).ToErrno();
		}

		protected override Errno OnCreateSymbolicLink (string from, string to)
		{
			return _fileSystem.CreateSymlink(from, to).ToErrno();
		}

		protected override Errno OnRenamePath (string from, string to)
		{
			return _fileSystem.Move(from, to).ToErrno();
		}

		protected override Errno OnCreateHardLink (string from, string to)
		{
			return _fileSystem.CreateHardLink(from, to).ToErrno();
		}

		protected override Errno OnChangePathPermissions (string path, FilePermissions mode)
		{
			return _fileSystem.Chmod(path, mode).ToErrno();
		}

		protected override Errno OnChangePathOwner (string path, long uid, long gid)
		{
			return _fileSystem.Chown(path, uid, gid).ToErrno();
		}

		protected override Errno OnTruncateFile (string path, long size)
		{
			return _fileSystem.Truncate(path, size).ToErrno();
		}

		protected override Errno OnChangePathTimes (string path, ref Utimbuf buf)
		{
			return _fileSystem.ChangeTimes(path, buf.actime, buf.modtime).ToErrno();
		}

		protected override Errno OnOpenHandle (string path, OpenedPathInfo info)
		{
			return _fileSystem.Open(path, (OpenFlags) info.OpenFlags).ToErrno();
		}

		protected override Errno OnReadHandleUnsafe (string file, OpenedPathInfo info, IntPtr buf, ulong size, long offset, out int bytesWritten)
		{
			var r = _fileSystem.Read(file, offset, size, out var buffer, out bytesWritten).ToErrno();
			Marshal.Copy(buffer, 0, buf, (int)size);
			return r;
		}
		
		protected override Errno OnWriteHandle (string path, OpenedPathInfo info, byte[] buf, long offset, out int bytesWritten)
		{
			return _fileSystem.Write(path, buf, out bytesWritten, offset).ToErrno();
		}

		protected override Errno OnGetFileSystemStatus (string path, out Statvfs stbuf)
		{
			return _fileSystem.GetFileSystemStatus(path, out stbuf);
		}

		protected override Errno OnReleaseHandle (string path, OpenedPathInfo info)
		{
			return 0;
		}

		protected override Errno OnSynchronizeHandle (string path, OpenedPathInfo info, bool onlyUserData)
		{
			return 0;
		}

		protected override Errno OnSetPathExtendedAttribute (string path, string name, byte[] value, XattrFlags flags)
		{
			return _fileSystem.SetPathExtendedAttribute(path, name, value, flags).ToErrno();
		}

		protected override Errno OnGetPathExtendedAttribute (string path, string name, byte[] value, out int bytesWritten)
		{
			return _fileSystem.GetPathExtendedAttribute(path, name, value, out bytesWritten).ToErrno();
		}

		protected override Errno OnListPathExtendedAttributes (string path, out string[] names)
		{
			return _fileSystem.ListPathExtendedAttributes(path, out names).ToErrno();
		}

		protected override Errno OnRemovePathExtendedAttribute (string path, string name)
		{
			return _fileSystem.RemovePathExtendedAttribute(path, name).ToErrno();
		}

		protected override Errno OnLockHandle (string file, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock)
		{
			return _fileSystem.Lock(file, (OpenFlags)info.OpenFlags, cmd, ref @lock);
		}
	}
}