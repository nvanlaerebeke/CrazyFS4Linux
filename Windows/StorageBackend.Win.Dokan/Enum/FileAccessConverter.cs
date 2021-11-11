namespace StorageBackend.Win.Dokan.Enum {

    internal static class FileAccessConverter {

        public static System.IO.FileAccess Get(DokanNet.FileAccess access) {
            switch (access) {
                case DokanNet.FileAccess.AccessSystemSecurity:
                case DokanNet.FileAccess.Execute:
                case DokanNet.FileAccess.None:
                case DokanNet.FileAccess.ReadAttributes:
                case DokanNet.FileAccess.ReadData:
                case DokanNet.FileAccess.ReadExtendedAttributes:
                case DokanNet.FileAccess.ReadPermissions:
                    return System.IO.FileAccess.Read;

                case DokanNet.FileAccess.Delete:
                case DokanNet.FileAccess.DeleteChild:
                case DokanNet.FileAccess.ChangePermissions:
                case DokanNet.FileAccess.AppendData:
                case DokanNet.FileAccess.SetOwnership:
                case DokanNet.FileAccess.WriteAttributes:
                case DokanNet.FileAccess.WriteData:
                case DokanNet.FileAccess.WriteExtendedAttributes:
                    return System.IO.FileAccess.Write;

                case DokanNet.FileAccess.GenericAll:
                case DokanNet.FileAccess.GenericExecute:
                case DokanNet.FileAccess.GenericRead:
                case DokanNet.FileAccess.GenericWrite:
                case DokanNet.FileAccess.MaximumAllowed:
                case DokanNet.FileAccess.Synchronize:
                    return System.IO.FileAccess.ReadWrite;

                default:
                    return System.IO.FileAccess.ReadWrite;
            }
        }
    }
}