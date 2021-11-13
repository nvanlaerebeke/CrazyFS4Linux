using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Security.AccessControl;
using System.Text;
using CrazyFS.FileSystem;
using CrazyFS.Passthrough;
using Fuse.NET;
using Mono.Unix.Native;

namespace CrazyFS {
	class Redirect : Fuse.NET.FileSystem {

		private readonly string basedir;
		private readonly IFuse _fileSystem;
		public Redirect (string source, string destination)
		{
			basedir = source;
			MountPoint = destination;
			_fileSystem = new Fuse<PassthroughFileSystem>();
		}

		protected override Errno OnGetPathStatus (string path, out Stat buf)
		{
			buf = new Stat();
			var result = _fileSystem.GetPathInfo(GetPath(path), out IFileSystemInfo info);
			if (result.Status == ResultStatus.Success)
			{
				buf = info.ToStat();
			}
			return result.ToErrno();
		}

		protected override Errno OnAccessPath (string path, AccessModes mask)
		{
			return _fileSystem.CheckAccess(GetPath(path), (PathAccessModes) mask).ToErrno();
		}

		protected override Errno OnReadSymbolicLink (string path, out string target)
		{
			target = null;
			StringBuilder buf = new StringBuilder (256);
			do {
				int r = Syscall.readlink (basedir+path, buf);
				if (r < 0) {
					return Stdlib.GetLastError ();
				}
				else if (r == buf.Capacity) {
					buf.Capacity *= 2;
				}
				else {
					target = buf.ToString (0, r);
					return 0;
				}
			} while (true);
		}

		protected override Errno OnReadDirectory (string path, OpenedPathInfo fi,
				out IEnumerable<DirectoryEntry> paths)
		{
			IntPtr dp = Syscall.opendir (basedir+path);
			if (dp == IntPtr.Zero) {
				paths = null;
				return Stdlib.GetLastError ();
			}

			Dirent de;
			List<DirectoryEntry> entries = new List<DirectoryEntry> ();
			while ((de = Syscall.readdir (dp)) != null) {
				DirectoryEntry e = new DirectoryEntry (de.d_name);
				e.Stat.st_ino  = de.d_ino;
				e.Stat.st_mode = (FilePermissions) (de.d_type << 12);
				entries.Add (e);
			}
			Syscall.closedir (dp);

			paths = entries;
			return 0;
		}

		protected override Errno OnCreateSpecialFile (string path, FilePermissions mode, ulong rdev)
		{
			return _fileSystem.CreateSpecialFile(GetPath(path), mode, rdev);
		}

		protected override Errno OnCreateDirectory (string path, FilePermissions mode)
		{
			return _fileSystem.CreateDirectory(GetPath(path), mode).ToErrno();
		}

		protected override Errno OnRemoveFile (string path)
		{
			int r = Syscall.unlink (basedir+path);
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnRemoveDirectory (string path)
		{
			int r = Syscall.rmdir (basedir+path);
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnCreateSymbolicLink (string from, string to)
		{
			return _fileSystem.CreateSymlink(GetPath(from), GetPath(to)).ToErrno();
		}

		protected override Errno OnRenamePath (string from, string to)
		{
			return _fileSystem.Move(from, to).ToErrno();
		}

		protected override Errno OnCreateHardLink (string from, string to)
		{
			return _fileSystem.CreateHardLink(GetPath(from), GetPath(to)).ToErrno();
		}

		protected override Errno OnChangePathPermissions (string path, FilePermissions mode)
		{
			return _fileSystem.Chmod(GetPath(path), mode).ToErrno();
		}

		protected override Errno OnChangePathOwner (string path, long uid, long gid)
		{
			return _fileSystem.Chown(GetPath(path), uid, gid).ToErrno();
		}

		protected override Errno OnTruncateFile (string path, long size)
		{
			int r = Syscall.truncate (basedir+path, size);
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnChangePathTimes (string path, ref Utimbuf buf)
		{
			return _fileSystem.ChangeTimes(GetPath(path), buf.actime, buf.modtime).ToErrno();
		}

		protected override Errno OnOpenHandle (string path, OpenedPathInfo info)
		{
			return ProcessFile (basedir+path, info.OpenFlags, delegate (int fd) {return 0;});
		}

		private delegate int FdCb (int fd);
		private static Errno ProcessFile (string path, OpenFlags flags, FdCb cb)
		{
			int fd = Syscall.open (path, flags);
			if (fd == -1)
				return Stdlib.GetLastError ();
			int r = cb (fd);
			Errno res = 0;
			if (r == -1)
				res = Stdlib.GetLastError ();
			Syscall.close (fd);
			return res;
		}

		protected override unsafe Errno OnReadHandle (string path, OpenedPathInfo info, byte[] buf, long offset, out int bytesRead)
		{
			int br = 0;
			Errno e = ProcessFile (basedir+path, OpenFlags.O_RDONLY, delegate (int fd) {
				fixed (byte *pb = buf) {
					return br = (int) Syscall.pread (fd, pb, (ulong) buf.Length, offset);
				}
			});
			bytesRead = br;
			return e;
		}

		protected override Errno OnWriteHandle (string path, OpenedPathInfo info, byte[] buf, long offset, out int bytesWritten)
		{
			return _fileSystem.Write(GetPath(path), buf, out bytesWritten, offset).ToErrno();
		}

		protected override Errno OnGetFileSystemStatus (string path, out Statvfs stbuf)
		{
			return _fileSystem.GetFileSystemStatus(GetPath(path), out stbuf);
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
			int r = Syscall.lsetxattr (basedir+path, name, value, (ulong) value.Length, flags);
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnGetPathExtendedAttribute (string path, string name, byte[] value, out int bytesWritten)
		{
			int r = bytesWritten = (int) Syscall.lgetxattr (basedir+path, name, value, (ulong) (value?.Length ?? 0));
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnListPathExtendedAttributes (string path, out string[] names)
		{
			int r = (int) Syscall.llistxattr (basedir+path, out names);
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnRemovePathExtendedAttribute (string path, string name)
		{
			int r = Syscall.lremovexattr (basedir+path, name);
			if (r == -1)
				return Stdlib.GetLastError ();
			return 0;
		}

		protected override Errno OnLockHandle (string file, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock)
		{
			Flock _lock = @lock;
			Errno e = ProcessFile (basedir+file, info.OpenFlags, fd => Syscall.fcntl (fd, cmd, ref _lock));
			@lock = _lock;
			return e;
		}
		private string GetPath(string path)
		{
			return Path.Combine(basedir, path.TrimStart(Path.DirectorySeparatorChar));
		}
	}
}