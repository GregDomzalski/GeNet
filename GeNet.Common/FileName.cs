using System.Text;

namespace GeNet.Common;

public static class FileName
{
    public static string Create(string className, string generatorName)
    {
        StringBuilder sb = new(className);

        sb.Replace('<', '{');
        sb.Replace('>', '}');
        sb.Append(".");
        sb.Append(generatorName);
        sb.Append(".g.cs");

        return sb.ToString();
    }

}
