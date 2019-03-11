namespace Ra.Common
{
    #region Namespace Using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using log4net;

    #endregion

    public class SimpleCachedStream : Stream
    {
        #region  Fields

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Dictionary<(long p, int c), (int c, byte[] data)> cache;

        private int lastCount;
        private readonly long MinLocation;
        private readonly Stream origStream;
        private readonly Random rnd;

        #endregion

        #region  Constructors

        public SimpleCachedStream(Stream origStream)
        {
            if ((origStream as BufferedProgressStream)?.IsBuffered ?? false)
                MinLocation = BufferedProgressStream.MaximumFileSizeBuffered;


            this.origStream = origStream;
            rnd = new Random();
            cache = new Dictionary<(long p, int c), (int c, byte[] data)>();
        }

        #endregion

        #region Properties

        public override bool CanRead => origStream.CanRead;
        public override bool CanSeek => origStream.CanSeek;
        public override bool CanWrite => false;
        public override long Length => origStream.Length;

        public override long Position
        {
            get => origStream.Position;

            set
            {
                if (value == 0)
                    Prune();
                origStream.Position = value;
                //                this.origStream.Position = value;
                //                this.OnPositionChanged();
            }
        }

        #endregion

        #region

        protected override void Dispose(bool disposing)
        {
            if (disposing) origStream?.Dispose();
        }

        public override void Flush()
        {
            throw new NotSupportedException();
        }

        private void Prune()
        {
            if (cache.Count < 100)
                return;

            if (rnd.NextDouble() > 0.001f * (cache.Count - lastCount + 0f) / lastCount)
                return;

//            Debug.WriteLine($"pruning: {lastCount}->{cache.Count}");


            var toRemove = cache.Where(p => rnd.NextDouble() < Math.Pow(0.5, p.Value.c))
                .Select(p => p.Key)
                .ToList();

            foreach (var key in toRemove) cache.Remove(key);
//            Debug.WriteLine($"pruning: {lastCount}->{cache.Count} ({toRemove.Count})");
//            log.Error(($"pruning: {lastCount}->{cache.Count} ({toRemove.Count})"));

            lastCount = cache.Count;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // Logically it should be (origStream.Position - count)
            if (origStream.Position > MinLocation && count < 64)
            {
                var key = (origStream.Position, count);
                if (cache.TryGetValue(key, out var value))
                {
                    Array.Copy(value.data, 0, buffer, offset, count);
                    origStream.Seek(count, SeekOrigin.Current);
                    value.c++;
                }
                else
                {
                    var read = origStream.Read(buffer, offset, count);
                    if (read != count)
                        return read;
                    var data = new byte[count];
                    Array.Copy(buffer, offset, data, 0, count);
                    value = (1, data);
                }

                if (value.c < 100)
                    cache[key] = value;
                return count;
            }

            return origStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (offset == 0 && origin == SeekOrigin.Begin)
                Prune();
            return origStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}