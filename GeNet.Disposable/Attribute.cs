namespace GeNet.Disposable;

public class Attribute
{
    public const string Text = $@"{Header.Value}

namespace GeNet
{{
    [System.Runtime.CompilerServices.CompilerGenerated]
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct)]
    public class GenerateDisposeAttribute : System.Attribute
    {{
        [System.Runtime.CompilerServices.CompilerGenerated]
        public GenerateDisposeAttribute()
        {{
        }}
    }}
}}";

}
