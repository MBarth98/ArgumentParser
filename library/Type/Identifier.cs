using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgumentParser.Type
{
    public class Identifier
    {
        public string Text { get; set; } = "";

        public Guid GUID { get; } = Guid.NewGuid();
    }
}