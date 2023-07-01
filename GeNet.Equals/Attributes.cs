namespace GeNet.Equals;

public static class Attributes
{
    public const string GenerateEqualsText = $@"{Header.Value}

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

    public const string IgnoredByEqualsText = $@"{Header.Value}

namespace GeNet
{{
    [System.Runtime.CompilerServices.CompilerGenerated]
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    public class IgnoredByEqualsAttribute : System.Attribute
    {{
        [System.Runtime.CompilerServices.CompilerGenerated]
        public IgnoredByEqualsAttribute()
        {{
        }}
    }}
}}";

}
