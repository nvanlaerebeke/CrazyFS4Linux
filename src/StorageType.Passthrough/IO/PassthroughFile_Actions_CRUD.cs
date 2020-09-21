using StorageBackend;
using System.IO;

namespace StorageType.Passthrough.IO {

    internal partial class PassthroughFile {

        public Result Flush() {
            Stream?.Flush();
            return new Result(ResultStatus.Success);
        }

        public Result Read(out byte[] pBuffer, long pOffset, int pLength, out int pBytesTransferred) {
            if (pOffset >= Stream.Length) {
                pBytesTransferred = 0;
                pBuffer = default;
                return new Result(ResultStatus.EndOfFile);
            }
            _ = Stream.Seek(pOffset, SeekOrigin.Begin);

            pBuffer = new byte[pLength];
            pBytesTransferred = Stream.Read(pBuffer, 0, pLength);
            return new Result(ResultStatus.Success);
        }

        public Result Write(byte[] pBuffer, long pOffset, out int pBytesTransferred) {
            _ = Stream.Seek(pOffset, SeekOrigin.Begin);
            Stream.Write(pBuffer, 0, pBuffer.Length);
            pBytesTransferred = pBuffer.Length;
            return new Result(ResultStatus.Success);
        }
    }
}