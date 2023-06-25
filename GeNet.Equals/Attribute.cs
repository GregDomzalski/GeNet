namespace GeNet.Equals;

public static class Attribute
{
    public const string Text = $@"{Header.Value}

namespace GeNet
{{
    [System.Runtime.CompilerServices.CompilerGenerated]
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class GenerateEqualsAttribute : System.Attribute
    {{
        [System.Runtime.CompilerServices.CompilerGenerated]
        public GenerateEqualsAttribute()
        {{
        }}
    }}

}}";

}
