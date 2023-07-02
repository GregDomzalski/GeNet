using GeNet.IntegrationTests.Equals;

namespace GeNet.IntegrationTests;

public class EqualsGeneratorTests
{
    [Fact]
    public void ClassWithPropertiesAndFields_SameValues_EqualsTrue()
    {
        var c1 = new TestClass
        {
            Field1 = "a",
            Field2 = 1,
            IgnoredField = 5,
            SetOnlyProp = 3,
            Prop1 = "b",
            Prop2 = 2
        };

        var c2 = new TestClass
        {
            Field1 = "a",
            Field2 = 1,
            IgnoredField = 1,
            SetOnlyProp = 1,
            Prop1 = "b",
            Prop2 = 2
        };

        Assert.Equal(c1, c2);
        Assert.True(c1.Equals(c2));
        Assert.True(c2.Equals(c1));
        Assert.True(c1 == c2);
        Assert.False(c1 != c2);
    }

    [Fact]
    public void ClassWithPropertiesAndFields_DifferentValues_EqualsFalse()
    {
        var c1 = new TestClass
        {
            Field1 = "a",
            Field2 = 1,
            IgnoredField = 5,
            SetOnlyProp = 3,
            Prop1 = "b",
            Prop2 = 2
        };

        var c2 = new TestClass
        {
            Field1 = "a",
            Field2 = 2,
            IgnoredField = 1,
            SetOnlyProp = 1,
            Prop1 = "b",
            Prop2 = 3
        };

        Assert.NotEqual(c1, c2);
        Assert.False(c1.Equals(c2));
        Assert.False(c2.Equals(c1));
        Assert.False(c1 == c2);
        Assert.True(c1 != c2);
    }

    [Fact]
    public void StructWithFields_SameValues_EqualsTrue()
    {
        var s1 = new TestStruct
        {
            Field1 = "a",
            Field2 = 1,
            IgnoredField = 5,
            SetOnlyProp = 3,
            Prop1 = "b",
            Prop2 = 2
        };

        var s2 = new TestStruct
        {
            Field1 = "a",
            Field2 = 1,
            IgnoredField = 1,
            SetOnlyProp = 1,
            Prop1 = "b",
            Prop2 = 2
        };

        Assert.Equal(s1, s2);
        Assert.True(s1.Equals(s2));
        Assert.True(s2.Equals(s1));
        Assert.True(s1 == s2);
        Assert.False(s1 != s2);
    }

    [Fact]
    public void StructWithPropertiesAndFields_DifferentValues_EqualsFalse()
    {
        var s1 = new TestStruct
        {
            Field1 = "a",
            Field2 = 1,
            IgnoredField = 5,
            SetOnlyProp = 3,
            Prop1 = "b",
            Prop2 = 2
        };

        var s2 = new TestStruct
        {
            Field1 = "a",
            Field2 = 2,
            IgnoredField = 1,
            SetOnlyProp = 1,
            Prop1 = "b",
            Prop2 = 3
        };

        Assert.NotEqual(s1, s2);
        Assert.False(s1.Equals(s2));
        Assert.False(s2.Equals(s1));
        Assert.False(s1 == s2);
        Assert.True(s1 != s2);
    }
}
