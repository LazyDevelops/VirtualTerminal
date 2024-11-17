namespace VirtualTerminal.FileSystem
{
    public struct FileDataStruct
    {
        public string Name;
        public byte Permission;
        public string UID;
        public FileType FileType;
        public long LastTouchTime;
        public string? Content;

        public FileDataStruct(string _name, string _UID, byte _permission, FileType _fileType, string? _content = null, long? _lastTouchTime = null)
        {
            Name = _name;
            Permission = _permission;
            UID = _UID;
            FileType = _fileType;
            _lastTouchTime ??= DateTimeOffset.UtcNow.AddHours(9).ToUnixTimeSeconds();
            LastTouchTime = _lastTouchTime.Value;
            Content = _content;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}