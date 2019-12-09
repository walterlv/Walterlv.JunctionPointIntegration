using System;
using System.IO;
using System.Linq;
using Walterlv.IO.PackageManagement.Core;

namespace Walterlv.JunctionPointIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                // 传入参数 "联接目录" "目标目录"
                Link(args[0], args[1]);
            }
            else if (args.Length == 3)
            {
                // 传入参数 "联接目录" "联接所在的目录" "目标目录"
                if (string.IsNullOrWhiteSpace(args[0]))
                {
                    // 如果来源程序没有传入 "联接目录"，意味着没有选中文件夹，那么联接当前目录。
                    Link(args[1], args[2]);
                }
                else
                {
                    // 如果来源程序传入了 "联接目录"，意味着选中了文件夹，那么联接选中的文件夹。
                    Link(args[0], args[2]);
                }
            }
            else
            {
                Environment.FailFast("必须传入两个参数：junction-point Link Target");
            }
        }

        private static void Link(string link, string target)
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
                    // 如果是文件夹，则检查里面是否存在文件，是否需要备份。
                    var hasContent = linkDirectory.EnumerateFileSystemInfos().Any();
                    if (hasContent)
                    {
                        linkDirectory.MoveTo($"{linkDirectory.FullName}.bak");
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
}
