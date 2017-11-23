using System;
using System.Collections.Generic;
using AspectCore.APM.Collector;

namespace AspectCore.APM.Core
{
    public class GlobalFieldReader : IGlobalFieldReader
    {
        private readonly IEnumerable<IGlobalFieldProvider> _globalFieldProviders;

        public GlobalFieldReader(IEnumerable<IGlobalFieldProvider> globalFieldProviders)
        {
            _globalFieldProviders = globalFieldProviders ?? throw new ArgumentNullException(nameof(globalFieldProviders));
        }

        public void Read(FieldCollection fieldCollection)
        {
            if (fieldCollection == null)
            {
                throw new ArgumentNullException(nameof(fieldCollection));
            }
            foreach (var fieldProvider in _globalFieldProviders)
            {
                var fields = fieldProvider.GetGlobalFields();
                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        fieldCollection[field.Key] = field.Value;
                    }
                }
            }
        }
    }
}