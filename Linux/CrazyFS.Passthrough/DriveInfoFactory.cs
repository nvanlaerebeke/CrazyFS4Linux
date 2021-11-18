using System;
using System.IO;
using System.IO.Abstractions;

namespace CrazyFS.Passthrough {
    
    [Serializable]
    public class DriveInfoFactory : IDriveInfoFactory
    {
        private readonly IFileSystem _fileSystem;

        public DriveInfoFactory(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Retrieves the drive names of all logical drives on a computer.
        /// </summary>
        /// <returns>An array of type <see cref="DriveInfoBase"/> that represents the logical drives on a computer.</returns>
        public IDriveInfo[] GetDrives()
        {
            var driveInfos = DriveInfo.GetDrives();
            var driveInfoWrappers = new IDriveInfo[driveInfos.Length];
            for (var index = 0; index < driveInfos.Length; index++)
            {
                var driveInfo = driveInfos[index];
                driveInfoWrappers[index] = new DriveInfoWrapper(_fileSystem, driveInfo);
            }

            return driveInfoWrappers;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveInfoBase"/> class, which acts as a wrapper for a logical drive.
        /// </summary>
        /// <param name="driveName">A valid drive path or drive letter.</param>
        public IDriveInfo FromDriveName(string driveName)
        {
            var realDriveInfo = new DriveInfo(driveName);
            return new DriveInfoWrapper(_fileSystem, realDriveInfo);
        }
    }
}