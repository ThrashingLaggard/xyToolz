namespace xyToolz.Filesystem;

public static class FileInventory
{
    public static IEnumerable<FileInfo> Inventory(string path)
        => xyFiles.Inventory(path);

    public static IEnumerable<string> InventoryNames(string path)
        => xyFiles.InventoryNames(path);
}
