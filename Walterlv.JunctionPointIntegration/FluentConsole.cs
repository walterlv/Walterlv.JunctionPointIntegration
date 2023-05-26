using Pastel;

using System.Drawing;

namespace Walterlv.JunctionPointIntegration;
/// <summary>
/// 流式输出的控制台辅助类。
/// </summary>
public readonly ref struct FluentConsole
{
    /// <summary>
    /// 用于输出用法的文本。
    /// </summary>
    public required string UsageText { get; init; }

    /// <summary>
    /// 输出错误文本。
    /// </summary>
    /// <param name="errorText">错误文本。</param>
    /// <returns>流式输出。</returns>
    public FluentConsole PrintError(string errorText)
    {
        Console.WriteLine(errorText.Pastel(Color.Red));
        return this;
    }

    /// <summary>
    /// 输出用法文本。
    /// </summary>
    /// <returns>流式输出。</returns>
    public FluentConsole PrintUsage()
    {
        Console.WriteLine(UsageText);
        return this;
    }

    /// <summary>
    /// 流式返回。
    /// </summary>
    /// <typeparam name="T">返回值类型。</typeparam>
    /// <param name="value">返回值。</param>
    /// <returns>返回值。</returns>
    public T Return<T>(T value)
    {
        return value;
    }
}
