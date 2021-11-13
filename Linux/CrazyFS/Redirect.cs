using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
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
			_fileSystem = new FileSystem.Fuse(new PassthroughFileSystem(basedir));
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

			return _fileSystem.GetSymbolicLinkTarget(GetPath(path), out target).ToErrno();
		}

		protected override Errno OnReadDirectory (string path, OpenedPathInfo fi, out IEnumerable<DirectoryEntry> paths)
		{
			var result = _fileSystem.Ls(GetPath(path), out var items).ToErrno();
			paths = items.ToList().ConvertAll(x => new DirectoryEntry(x.Name));
			return result;
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
			return _fileSystem.RemoveFile(GetPath(path)).ToErrno();
		}

		protected override Errno OnRemoveDirectory (string path)
		{
			return _fileSystem.RemoveDirectory(GetPath(path)).ToErrno();
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
			return _fileSystem.Truncate(GetPath(path), size).ToErrno();
		}

		protected override Errno OnChangePathTimes (string path, ref Utimbuf buf)
		{
			return _fileSystem.ChangeTimes(GetPath(path), buf.actime, buf.modtime).ToErrno();
		}

		protected override Errno OnOpenHandle (string path, OpenedPathInfo info)
		{
			return _fileSystem.Open(GetPath(path), info);
		}

		protected override Errno OnReadHandleUnsafe (string file, OpenedPathInfo info, IntPtr buf, ulong size, long offset, out int bytesWritten)
		{
			var r = _fileSystem.Read(GetPath(file), offset, size, out var buffer, out bytesWritten).ToErrno();
			Marshal.Copy(buffer, 0, buf, (int)size);
			return r;
			/*
			
			
			int br = 0;
			Errno e = ProcessFile (basedir+path, OpenFlags.O_RDONLY, delegate (int fd) {
				fixed (byte *pb = buf) {
					return br = (int) Syscall.pread (fd, pb, (ulong) buf.Length, offset);
				}
			});
			bytesRead = br;
			return e;
			bytesWritten = 0;
			return Errno.ENOSYS;*/
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
			return _fileSystem.SetPathExtendedAttribute(GetPath(path), name, value, flags);
		}

		protected override Errno OnGetPathExtendedAttribute (string path, string name, byte[] value, out int bytesWritten)
		{
			return _fileSystem.GetPathExtendedAttribute(GetPath(path), name, value, out bytesWritten);
		}

		protected override Errno OnListPathExtendedAttributes (string path, out string[] names)
		{
			return _fileSystem.ListPathExtendedAttributes(GetPath(path), out names);
		}

		protected override Errno OnRemovePathExtendedAttribute (string path, string name)
		{
			return _fileSystem.RemovePathExtendedAttribute(GetPath(path), name);
		}

		protected override Errno OnLockHandle (string file, OpenedPathInfo info, FcntlCommand cmd, ref Flock @lock)
		{
			return _fileSystem.Lock(GetPath(file), info, cmd, ref @lock);
		}
		private string GetPath(string path)
		{
			return Path.Combine(basedir, path.TrimStart(Path.DirectorySeparatorChar));
		}
		
		private delegate int FdCb (int fd);
		private static Errno ProcessFile (string path, OpenFlags flags, FdCb cb)
		{
			int fd = Syscall.open (path, flags);
			if (fd == -1)
			{
				return Stdlib.GetLastError();
			}

			int r = cb (fd);
			Errno res = 0;
			if (r == -1)
			{
				res = Stdlib.GetLastError();
			}
			Syscall.close (fd);
			return res;
		}
	}
}