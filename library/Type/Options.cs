using System.Diagnostics;
using System.Text;

namespace ArgumentParser.Type
{
    public class Options
    {
        public readonly string name = "";
        public readonly List<Options> Children = new();
        public readonly List<Option> Siblings = new();


        public Options Add(Options child)
        {
            Children.Add(child);
            return child;
        }

        public Options Add(Option option_root)
        {
            Siblings.Add(option_root);
            return this;
        }

        public Options(Option option_root)
        {
            name = option_root.name;
            Siblings.Add(option_root);
        }

        public Options()
        {
            name = "";
        }

        public string ToPath()
        {
            return ToPath(name);
        }

        public string ToPath(string parent)
        {
            var builder = new StringBuilder();
            #if DEBUG

            foreach (var option in Siblings)
            {
                if (parent.EndsWith(option.name))
                {
                    if (option.type == Option.Type.PROPERTY)
                    {
                        builder.AppendLine($"{parent} <value>");
                    }

                    continue;
                }

                builder.AppendLine($"{parent}/{option.name}");
            }

            foreach (var child in Children)
            {
                builder.Append($"{child.ToPath($"{parent}/{child.name}")}");
            }

            #else
            builder.Append("path output is not enabled in release builds.");
            #endif
            return builder.ToString();
        }

        public string ToDot(bool root = true)
        {
            var builder = new StringBuilder();
            #if DEBUG

            if (root)
            {
                builder.AppendLine("graph {");
                builder.AppendLine("node [shape=box];");
                builder.AppendLine("edge [color=black];");
                builder.AppendLine($"root [label=\"root\"];");
            }

            var id = string.IsNullOrEmpty(name) ? "root" : name;
            foreach (var option in Siblings)
            {
                var color = option.type == Option.Type.PROPERTY ? "green" : "blue";
                builder.AppendLine($"{option.name} [color={color} label=\"{option.name}\"];");
                
                if (id == option.name)
                {
                    continue;
                }

                builder.AppendLine($"{id} -- {option.name};");
            }

            foreach (var child in Children)
            {
                builder.Append($"{child.ToDot(false)}");
                builder.AppendLine($"{id} -- {child.name};");
            }

            if (root)
            {
                builder.Append('}');
            }

            #else
            builder.Append("dot graph output is not enabled in release builds.");
            #endif
            return builder.ToString();
        }
    }
}