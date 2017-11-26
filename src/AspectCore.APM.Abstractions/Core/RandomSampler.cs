using System;

namespace AspectCore.APM.Core
{
    public class RandomSampler : ISampler
    {
        private readonly Random _random;
        private readonly float _samplingRate;

        public RandomSampler(Random random,float samplingRate)
        {
            _random = random ?? throw new ArgumentNullException(nameof(_random));
            if (samplingRate <= 0f || samplingRate > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(samplingRate), "The range of values should be 0 to 1");
            }
            _samplingRate = samplingRate;
        }

        public bool ShouldSample()
        {
            return _random.NextDouble() < _samplingRate;
        }
    }
}