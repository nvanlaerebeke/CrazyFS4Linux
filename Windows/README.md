# CrazyFS ToDo List

- Test timezone stuff, creation/lastaccess/lastwrite none utc methods are set by toutc() methods but they should be in the current time zone i think?

- Check Mount vs MountEx: MountEx uses multiple threads? for FS requests?
- StorageBackend interface IFilesystem and IWindowsBackend have the same methods Mount and UnMount

- Add ability to switch between winfsp and dokany
- Add fuse wrapper support for both winfsp and dokany