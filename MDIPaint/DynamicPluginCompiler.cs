using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using PluginInterface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Linq;

namespace MDIPaint
{
    public static class DynamicPluginCompiler
    {
        public static IPlugin Compile(string userCode)
        {
            string code = $@"
using System;
using System.Drawing;
using PluginInterface;

public class UserPlugin : IPlugin
{{
    public string Name => ""Пользовательский фильтр"";
    public string Author => ""User"";

    public void Transform(
        PluginContext context,
        IProgress<int>? progress = null,
        IProgress<string>? status = null,
        System.Threading.CancellationToken cancellationToken = default)
    {{
        Bitmap image = context.Image;

        {userCode}
    }}
}}";

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var references = AppDomain.CurrentDomain
    .GetAssemblies()
    .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
    .Select(a => MetadataReference.CreateFromFile(a.Location))
    .ToList();

            var compilation = CSharpCompilation.Create(
                "UserPluginAssembly",
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using var ms = new MemoryStream();

            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = string.Join("\n", result.Diagnostics);
                throw new Exception(errors);
            }

            ms.Seek(0, SeekOrigin.Begin);

            var assembly = Assembly.Load(ms.ToArray());

            var type = assembly.GetType("UserPlugin");

            return (IPlugin)Activator.CreateInstance(type);
        }
    }
}
