namespace VirtualTerminal.FileSystem
{
    public struct FileDataStruct(string name, string UID, byte permission, FileType fileType, string? content = null)
    {
        public string Name { get; set; } = name;
        public byte Permission { get; set; } = permission;
        public string UID { get; set; } = UID;
        public FileType FileType { get; set; } = fileType;
        //public long LastTouchTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public string? Content { get; set; } = content;

        public override string ToString()
        {
            return Name;
        }
    }
}