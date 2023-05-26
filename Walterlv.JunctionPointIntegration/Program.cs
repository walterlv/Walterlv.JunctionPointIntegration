using Pastel;

using System.Drawing;

FluentConsole console = new()
{
    UsageText = $"""
        用法: junction-point {"[link]".Pastel("#0087D8")} {"[target]".Pastel("#F9F1A5")}
        用法: junction-point {"[selected-link-folder]".Pastel("#0087D8")} {"[link-folder]".Pastel(Color.Gray)} {"[selected-target-folder]".Pastel("#F9F1A5")} {"[target-folder]".Pastel(Color.Gray)}

        {"link".Pastel("#0087D8")}:
            目录联接的路径。
        {"target".Pastel("#F9F1A5")}:
            目标目录的路径。
        {"selected-link-folder".Pastel("#0087D8")}:
            选中的目录联接的路径。
        {"link-folder".Pastel(Color.Gray)}:
            目录联接所在的目录。
        {"selected-target-folder".Pastel("#F9F1A5")}:
            选中的目标目录的路径。
        {"target-folder".Pastel(Color.Gray)}:
            目标目录所在的目录。

        示例:
            junction-point {@"C:\Users\walterlv\Documents\GitHub".Pastel("#0087D8")} {@"C:\Users\walterlv\GitHub".Pastel("#F9F1A5")}
            junction-point {@"C:\Users\walterlv\Documents\GitHub".Pastel("#0087D8")} {@"C:\Users\walterlv\Documents".Pastel(Color.Gray)} {@"C:\Users\walterlv\GitHub".Pastel("#F9F1A5")} {@"C:\Users\walterlv".Pastel(Color.Gray)}
        """,
};

try
{
    return Run(console, args);
}
catch (Exception ex)
{
    return console
        .PrintError(ex.Message)
        .Return(-1, 3000);
}

static int Run(in FluentConsole console, string[] args)
{
    if (args.Length == 2)
    {
        // 传入参数 "联接目录" "目标目录"
        if (!string.IsNullOrWhiteSpace(args[1]))
        {
            FluentLink(args[0], args[1]);
            return 0;
        }
        else
        {
            return console
                .PrintError("没有指定目标目录。")
                .PrintUsage()
                .Return(-1, 3000);
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
            return console
                .PrintError("请打开双栏显示，这样才可以将非激活的文件夹作为联接的目标目录。")
                .PrintUsage()
                .Return(-1, 3000);
        }
        else
        {
            FluentLink(linkDirectory, targetDirectory);
            return 0;
        }
    }
    else
    {
        return console
            .PrintUsage()
            .Return(0);
    }
}

static void FluentLink(string link, string target)
{
    Console.WriteLine($"""
        创建目录联接 🗂️
         🔗 联接: {link.Pastel("#0087D8")}
         🎯 目标: {target.Pastel("#F9F1A5")}
        """);
    JunctionPointHelper.Link(link, target);
    Console.WriteLine("✅ 已成功创建".Pastel(Color.Green));
}
