namespace VirtualTerminal.FileSystem
{
    // 트리 고친 후 FileData 혹은 FileDataStruct로 이름 변경 예정
    public struct FileDataStruct(string name, string UID, byte permission, FileType fileType, string? content = null)
    {
        public string Name { get; set; } = name;
        public byte Permission { get; set; } = permission;
        public string UID { get; set; } = UID;
        public FileType FileType { get; set; } = fileType;
        public string? Content { get; set; } = content;

        public override string ToString()
        {
            return Name;
        }
    }
}