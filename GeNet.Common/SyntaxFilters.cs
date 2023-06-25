using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GeNet.Common;

public static class SyntaxFilters
{
    public static bool HasAtLeastOneAttribute(SyntaxNode node) =>
        node is TypeDeclarationSyntax { AttributeLists.Count: > 0 };

    public static bool HasAtLeastOneBaseType(SyntaxNode node) =>
        node is TypeDeclarationSyntax { BaseList.Types.Count: > 0 };

    public static bool IsClassOrStruct(SyntaxNode node) =>
        node is ClassDeclarationSyntax or StructDeclarationSyntax;

    public static bool IsClassOrStructOrRecord(SyntaxNode node) =>
        IsClassOrStruct(node) || node is RecordDeclarationSyntax;
}
