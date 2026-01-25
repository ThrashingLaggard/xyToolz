namespace xyToolz.Filesystem;

public static class FileRename
{
    public static Task<bool> RenameFileAsync(string completePath, string newName)
        => xyFiles.RenameFileAsync(completePath, newName);
}
