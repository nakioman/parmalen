using System;
using System.ComponentModel.Composition;

namespace Parmalen.Contracts
{
    [MetadataAttribute]
    public class NameAttribute : Attribute
    {
        public string Name { get; }

        public NameAttribute(string name)
        {
            Name = name;
        }
    }
}