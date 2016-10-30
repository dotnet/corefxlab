namespace System.Buffers
{
    public struct ReservedReadOnlyMemory<T> : IDisposable
    {
        IReadOnlyMemory<T> _owner;
        ReadOnlyMemory<T> _memory;
        long _versionId;
        int _reservationId;

        internal ReservedReadOnlyMemory(ref ReadOnlyMemory<T> memory, OwnedMemory<T> owner, long versionId, int reservationId)
        {
            _versionId = versionId;
            _owner = owner;
            _reservationId = _owner.AddReference(versionId, reservationId);
            _memory = new ReadOnlyMemory<T>(ref memory, _reservationId);
        }

        public bool IsDisposed => _owner.IsDependancyDisposed(_versionId, _reservationId);

        public ReadOnlyMemory<T> Memory
        {
            get
            {
                _owner.ThrowIfDisposed(_versionId, _reservationId);
                return _memory;
            }
        }

        public void Dispose()
        {
            _owner.Release(_versionId, _reservationId);
        }
    }
}
