namespace VirtualTerminal.FileSystem
{
    public struct FileDataStruct
    {
        public string Name;
        public byte Permission;
        public string UID;
        public FileType FileType;
        public long LastTouchTime;
        private string? _content;

        public string? Content
        {
            get => _content;
            set
            {
                _content = value;
                LastTouchTime = DateTimeOffset.UtcNow.AddHours(9).ToUnixTimeSeconds();
            }
        }

        public FileDataStruct(string name, string uid, byte permission, FileType fileType, string? content = null, long? lastTouchTime = null)
        {
            Name = name;
            Permission = permission;
            UID = uid;
            FileType = fileType;
            lastTouchTime ??= DateTimeOffset.UtcNow.AddHours(9).ToUnixTimeSeconds();
            LastTouchTime = lastTouchTime.Value;
            _content = content;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}