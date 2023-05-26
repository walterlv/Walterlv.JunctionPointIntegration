FluentConsole console = new()
{
    UsageText = """
        用法: junction-point [link] [target]
        用法: junction-point [selected-link-folder] [link-folder] [selected-target-folder] [target-folder]

        link:
            目录联接的路径。
        target:
            目标目录的路径。
        selected-link-folder:
            选中的目录联接的路径。
        link-folder:
            目录联接所在的目录。
        selected-target-folder:
            选中的目标目录的路径。
        target-folder:
            目标目录所在的目录。

        示例:
            junction-point "C:\Users\walterlv\Documents\GitHub" "C:\Users\walterlv\GitHub"
            junction-point "C:\Users\walterlv\Documents\GitHub" "C:\Users\walterlv\Documents" "C:\Users\walterlv\GitHub" "C:\Users\walterlv"
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
        .Return(-1);
}

static int Run(in FluentConsole console, string[] args)
{
    if (args.Length == 2)
    {
        // 传入参数 "联接目录" "目标目录"
        if (!string.IsNullOrWhiteSpace(args[1]))
        {
            Link(args[0], args[1]);
            return 0;
        }
        else
        {
            return console
                .PrintError("没有指定目标目录。")
                .PrintUsage()
                .Return(-1);
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
                .Return(-1);
        }
        else
        {
            Link(linkDirectory, targetDirectory);
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
