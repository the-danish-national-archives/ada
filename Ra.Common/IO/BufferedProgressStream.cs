namespace Ra.Common
{
    #region Namespace Using

    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Microsoft.VisualBasic.Devices;

    #endregion

    public class BufferedProgressStream : Stream
    {
        #region  Fields

//        private Stream _posStream => bufferedStream ?? origStream;


        private long _position;
        private long bufferedLength;

        private Stream bufferedStream;

//        private Stream internalStream;

        private long bufferSizeUsed;

        private IntPtr memIntPtr = IntPtr.Zero;

        private readonly Stream origStream;

        #endregion

        #region  Constructors

        public BufferedProgressStream(FileInfo fileInfo)
        {
            origStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read);
            file = fileInfo;
        }

        public BufferedProgressStream(Stream stream)
        {
            origStream = stream;
            file = null;
        }

        #endregion

        #region Properties

        public static int BufferedFileCount { get; private set; }

        public static bool BufferingEnabled { get; set; }
//
//        public override void WriteByte(byte value)
//        {
//            this.origStream.WriteByte(value);
////            this.OnPositionChanged();
//        }

        public override bool CanRead => origStream.CanRead;

        public override bool CanSeek => origStream.CanSeek;

        public override bool CanWrite => false; //this.origStream.CanWrite;

        public FileInfo file { get; }

        public bool IsBuffered { get; private set; }

        public bool IsBufferingPossible => BufferingEnabled && !IsBuffered
                                                            && MaximumFilesBuffered > BufferedFileCount
//                       && BufferedProgressStream.MaximumFileSizeBuffered > this.Length
                                                            && MaximumTotalBufferSize > TotalBufferSizeUsed
                                                            && MinimumFreeSpaceAllowed < TotalAvailableMemory();

        public bool KeepAlive { get; set; }

        public override long Length => origStream.Length;

        public static int MaximumFilesBuffered { get; set; }

        public static int MaximumFileSizeBuffered { get; set; }

        public static long MaximumTotalBufferSize { get; set; }

        public static ulong MinimumFreeSpaceAllowed { get; set; }

        public override long Position
        {
            get => _position;

            set => _position = value;
        }

        public static long TotalBufferSizeUsed { get; private set; }

        public static bool UnmanagedBufferingEnabled { get; set; }

        #endregion

        #region

//        public event EventHandler PositionChanged;

        public unsafe void Buffer()
        {
            if (IsBufferingPossible)
            {
                bufferedLength = Math.Min(Length, MaximumFileSizeBuffered);
                Stream stream;
                if (UnmanagedBufferingEnabled)
                {
                    memIntPtr = Marshal.AllocHGlobal(Convert.ToInt32(bufferedLength));
                    var memBytePtr = (byte*) memIntPtr.ToPointer();
                    stream = new UnmanagedMemoryStream(memBytePtr, bufferedLength, bufferedLength, FileAccess.ReadWrite);
                }
                else
                {
                    stream = new MemoryStream();
                }

                origStream.Position = 0;
//                                this.origStream.CopyTo(stream);
                InternalCopyTo(stream, bufferedLength);
                origStream.Position = 0;
                bufferedStream = stream;
                bufferedStream.Position = 0;
                IsBuffered = true;
                Position = 0;
                BufferedFileCount++;
                bufferSizeUsed = bufferedLength;
                TotalBufferSizeUsed += bufferSizeUsed;
                _position = 0;
            }
        }

        public override void Close()
        {
            base.Close();
        }

//        public override int EndRead(IAsyncResult asyncResult)
//        {
//            int endRead = this.origStream.EndRead(asyncResult);
////            this.OnPositionChanged();
//            return endRead;
//        }
//
//        public override void EndWrite(IAsyncResult asyncResult)
//        {
//            this.origStream.EndWrite(asyncResult);
////            this.OnPositionChanged();
//        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !KeepAlive)
            {
                if (memIntPtr != IntPtr.Zero) Marshal.FreeHGlobal(memIntPtr);

                TotalBufferSizeUsed -= bufferSizeUsed;
                if (IsBuffered)
                    BufferedFileCount--;

                origStream?.Dispose();
                bufferedStream?.Dispose();
            }
        }

        public override void Flush()
        {
            origStream.Flush();
        }

        // Stolen from somewhere "Stream.cs", and length added added
        private void InternalCopyTo(Stream destination, long length, int bufferSize = 81920)
        {
            var buffer = new byte[bufferSize];
            int count;
            while ((count = Read(buffer, 0, (int) Math.Min(buffer.Length, length))) != 0)
            {
                destination.Write(buffer, 0, count);
                length -= count;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read;

            if (bufferedStream == null)
            {
                read = origStream?.Read(buffer, offset, count) ?? 0;
            }
            else
            {
                if (Position + count > bufferedLength)
                {
                    origStream.Seek(Position, SeekOrigin.Begin);
                    read = origStream?.Read(buffer, offset, count) ?? 0;
                }
                else
                {
                    bufferedStream.Seek(Position, SeekOrigin.Begin);
                    read = bufferedStream?.Read(buffer, offset, count) ?? 0;
                }
            }

            if (read != 0)
                Position += read;

            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            //            long seek;

            if (bufferedStream == null)
            {
                Position = origStream.Seek(offset, origin);
                return Position;
            }

            if (origin == SeekOrigin.Begin)
                if (offset < 0) // would lead to seek before start of stream
                    offset = (uint) offset;
            var oldPos = Position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = origStream.Length + offset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            if (Position >= 0) return Position;

            Position = oldPos;
            throw new IOException("IO.IO_SeekBeforeBegin");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
//            this.origStream.SetLength(value);
        }

//        protected virtual void OnPositionChanged()
//        {
//            this.PositionChanged?.Invoke(this, EventArgs.Empty);
//        }

        public override string ToString()
        {
            return GetType() + ">" + file;
        }

        public static ulong TotalAvailableMemory()
        {
            var computerInfo = new ComputerInfo();
            return computerInfo.AvailablePhysicalMemory;
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
//            this.origStream.Write(buffer, offset, count);
////            this.OnPositionChanged();
        }

        #endregion
    }
}