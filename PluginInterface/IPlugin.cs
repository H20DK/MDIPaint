using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PluginInterface
{
    public interface IPlugin
    {
        string Name { get; }
        string Author { get; }

        void Transform(
            PluginContext context,
            IProgress<int>? progress = null,
            IProgress<string>? status = null,
            CancellationToken cancellationToken = default
        );
    }
}
