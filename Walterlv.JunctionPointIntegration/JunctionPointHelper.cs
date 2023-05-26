namespace Walterlv.JunctionPointIntegration;
/// <summary>
/// 目录联接辅助类。
/// </summary>
public static class JunctionPointHelper
{
    /// <summary>
    /// 创建目录联接。
    /// </summary>
    /// <param name="link">目录联接的路径。</param>
    /// <param name="target">目标目录的路径。</param>
    /// <exception cref="InvalidOperationException">如果目录中有文件，则不允许创建目录联接。</exception>
    public static void Link(string link, string target)
    {
        var linkDirectory = new DirectoryInfo(link);
        var targetDirectory = new DirectoryInfo(target);
        if (linkDirectory.Exists)
        {
            // 如果目标目录存在，则检查一下这是否是一个目录联接。
            if (JunctionPoint.Exists(linkDirectory.FullName))
            {
                // 如果是目录联接，则可以删除重建。
                LinkCore(linkDirectory, targetDirectory);
            }
            else
            {
                // 如果是文件夹，则检查里面是否存在文件，有文件则不允许联接。
                var hasContent = linkDirectory.EnumerateFileSystemInfos().Any();
                if (hasContent)
                {
                    throw new InvalidOperationException("目录中有文件，不允许创建目录联接。");
                }
                else
                {
                    linkDirectory.Delete();
                    LinkCore(linkDirectory, targetDirectory);
                }
            }
        }
        else
        {
            LinkCore(linkDirectory, targetDirectory);
        }
    }

    private static void LinkCore(DirectoryInfo link, DirectoryInfo target)
    {
        JunctionPoint.Create(link.FullName, target.FullName, overwrite: true);
    }
}
