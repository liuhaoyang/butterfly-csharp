using System;
using System.Collections.Generic;
using AspectCore.APM.Collector;

namespace AspectCore.APM.Core
{
    public class GlobalTagReader : IGlobalTagReader
    {
        private readonly IEnumerable<IGlobalTagProvider> _globalTagProviders;

        public GlobalTagReader(IEnumerable<IGlobalTagProvider> globalTagProviders)
        {
            _globalTagProviders = globalTagProviders ?? throw new ArgumentNullException(nameof(globalTagProviders));
        }

        public void Read(TagCollection tagCollection)
        {
            if (tagCollection == null)
            {
                throw new ArgumentNullException(nameof(tagCollection));
            }
            foreach (var tagProvider in _globalTagProviders)
            {
                var tags = tagProvider.GetGlobalTags();
                if (tags != null)
                {
                    foreach(var tag in tags)
                    {
                        tagCollection[tag.Key] = tag.Value;
                    }
                }
            }
        }
    }
}