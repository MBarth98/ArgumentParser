using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArgumentParser.Type
{
    public class Identifier
    {
        public string Text
        {
            get => this.text;
            set => this.text = value;
        }

        public Guid GUID => this.m_guid;

        private string text = "";
        
        private readonly Guid m_guid = Guid.NewGuid();
    }
}