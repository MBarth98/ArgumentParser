using System.Diagnostics;
using System.Text;

namespace ArgumentParser.Type
{
    public class Group
    {
        public readonly string name = "";
        public readonly List<Group> SubGroups = new();
        public readonly List<Token> Tokens = new();


        public Group Add(Group child)
        {
            SubGroups.Add(child);
            return child;
        }

        public Group Add(Token token)
        {
            Tokens.Add(token);
            return this;
        }

        public Group(Token token)
        {
            name = token.name;
            Tokens.Add(token);
        }

        public Group()
        {
            name = "";
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

            var id = string.IsNullOrEmpty(this.name) ? "root" : this.name;
            foreach (var token in this.Tokens)
            {
                var color = token.type == Token.Type.PROPERTY ? "green" : "blue";
                builder.AppendLine($"{token.name} [color={color} label=\"{token.name}\"];");
                
                if (id == token.name)
                {
                    continue;
                }

                builder.AppendLine($"{id} -- {token.name};");
            }

            foreach (var group in this.SubGroups)
            {
                builder.Append($"{group.ToDot(false)}");
                builder.AppendLine($"{id} -- {group.name};");
            }

            if (root)
            {
                builder.Append('}');
            }

            #else
            builder.AppendLine("dot graph output is not enabled in release builds.");
            #endif
            return builder.ToString();
        }
    }
}