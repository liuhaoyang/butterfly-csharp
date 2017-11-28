using System;

namespace AspectCore.APM.Core
{
    public class RandomSamplerFactory : ISamplerFactory
    {
        private readonly Random _random = new Random();

        public ISampler CreateSampler(float samplingRate)
        {
            return new RandomSampler(_random, samplingRate);
        }
    }
}