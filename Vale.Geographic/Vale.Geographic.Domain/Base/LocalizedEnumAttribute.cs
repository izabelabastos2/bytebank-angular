using System;
using System.ComponentModel;
using System.Resources;
using System.Diagnostics.CodeAnalysis;


namespace Vale.Geographic.Domain.Base
{
    [ExcludeFromCodeCoverage]
    public class LocalizedEnumAttribute : DescriptionAttribute
    {
        private readonly ResourceManager _resource;
        private readonly string _resourceKey;

        public LocalizedEnumAttribute(string resourceKey, Type resourceType)
        {
            _resource = new ResourceManager(resourceType);
            _resourceKey = resourceKey;
        }

        public override string Description
        {
            get
            {
                var displayName = _resource.GetString(_resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", _resourceKey)
                    : displayName;
            }
        }
    }
}