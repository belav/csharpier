using System;
using System.IO;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace CSharpier.VisualStudio
{
    public class VSStreamWrapper : Stream
    {
        private readonly IStream iStream;

        public VSStreamWrapper(IStream vsStream)
        {
            this.iStream = vsStream;
        }

        public override bool CanRead => this.iStream != null;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override void Flush()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            this.iStream.Commit(0);
        }

        public override long Length
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var stat = new STATSTG[1];
                this.iStream.Stat(stat, (uint)STATFLAG.STATFLAG_DEFAULT);

                return (long)stat[0].cbSize.QuadPart;
            }
        }

        public override long Position
        {
            get
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                return Seek(0, SeekOrigin.Current);
            }
            set
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                Seek(value, SeekOrigin.Begin);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer cannot be null.");
            }

            var b = buffer;

            if (offset != 0)
            {
                b = new byte[buffer.Length - offset];
                buffer.CopyTo(b, 0);
            }

            this.iStream.Read(b, (uint)count, out var byteCounter);

            if (offset != 0)
            {
                b.CopyTo(buffer, offset);
            }

            return (int)byteCounter;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            var l = new LARGE_INTEGER();
            var ul = new ULARGE_INTEGER[1] { new ULARGE_INTEGER() };
            l.QuadPart = offset;
            this.iStream.Seek(l, (uint)origin, ul);
            return (long)ul[0].QuadPart;
        }

        public override void SetLength(long value)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (!CanWrite)
            {
                throw new InvalidOperationException();
            }

            var ul = new ULARGE_INTEGER { QuadPart = (ulong)value };
            this.iStream.SetSize(ul);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer cannot be null.");
            }
            if (!CanWrite)
            {
                throw new InvalidOperationException();
            }

            if (count <= 0)
            {
                return;
            }
            var b = buffer;

            if (offset != 0)
            {
                b = new byte[buffer.Length - offset];
                buffer.CopyTo(b, 0);
            }

            this.iStream.Write(b, (uint)count, out var byteCounter);
            if (byteCounter != count)
            {
                throw new IOException("Failed to write the total number of bytes to IStream!");
            }

            if (offset != 0)
            {
                b.CopyTo(buffer, offset);
            }
        }
    }
}
