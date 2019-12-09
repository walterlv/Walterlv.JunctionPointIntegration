using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Walterlv.IO.PackageManagement.Core;

namespace Walterlv.JunctionPointIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            Debugger.Launch();
            if (args.Length == 2)
            {
                // 传入参数 "联接目录" "目标目录"
                if (!string.IsNullOrWhiteSpace(args[1]))
                {
                    Link(args[0], args[1]);
                }
                else
                {
                    Console.WriteLine("没有指定目标目录。");
                }
            }
            else if (args.Length == 4)
            {
                // 传入参数 "联接目录" "联接所在的目录" "目标目录" "目标目录所在的目录" 
                // 如果来源程序没有传入 "目录"（参数 0 和 2），意味着没有选中文件夹，那么使用当前目录。
                // 如果来源程序传入了 "目录"（参数 0 和 2），意味着选中了文件夹，那么使用选中的文件夹。
                var linkDirectory = string.IsNullOrWhiteSpace(args[0]) ? args[1] : args[0];
                var targetDirectory = string.IsNullOrWhiteSpace(args[2]) || !Directory.Exists(args[2]) ? args[3] : args[2];
                if (string.IsNullOrWhiteSpace(targetDirectory))
                {
                    Console.WriteLine("请打开双栏显示，这样才可以将非激活的文件夹作为联接的目标目录。");
                    Thread.Sleep(2000);
                }
                else
                {
                    Link(linkDirectory, targetDirectory);
                }
            }
            else
            {
                Console.WriteLine("必须传入两个或四个参数：");
                Console.WriteLine(" - junction-point Link Target");
                Console.WriteLine(" - junction-point SelectedLinkFolder LinkFolder SelectedTargetFolder TargetFolder");
                Thread.Sleep(2000);
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
