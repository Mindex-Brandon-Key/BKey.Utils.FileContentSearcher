using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BKey.Utils.FileContentSearcher
{
    public class FunctionLocator
    {
        public string GetFunctionName(string filePath, int lineNumber)
        {
            if (lineNumber < 1)
            {
                throw new ArgumentException("Line numbers must be 1-based", nameof(lineNumber));
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The file does not exist.", filePath);
            }

            var tree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));

            var root = tree.GetRoot();
            var lineSpan = tree.GetText().Lines[lineNumber - 1].Span;
            var token = root.DescendantTokens(lineSpan).First();

            var method = token.Parent.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            var constructor = token.Parent.AncestorsAndSelf().OfType<ConstructorDeclarationSyntax>().FirstOrDefault();
            var property = token.Parent.AncestorsAndSelf().OfType<PropertyDeclarationSyntax>().FirstOrDefault();
            var classDeclaration = token.Parent.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();

            if (method != null)
            {
                return $"{classDeclaration.Identifier.Text}.{method.Identifier.Text}";
            }
            else if (constructor != null)
            {
                return $"{classDeclaration.Identifier.Text}.{constructor.Identifier.Text}";
            }
            else if (property != null)
            {
                return $"{classDeclaration.Identifier.Text}.{property.Identifier.Text}";
            }
            else
            {
                return $"{classDeclaration.Identifier.Text}";
            }
        }
    }
}
