using AForge.Imaging.Filters;
using G1ANT.Addon.Images.Enums;
using System;

namespace G1ANT.Addon.Images.Factories
{
    public class SmoothingFilterFactory
    {
        public BaseUsingCopyPartialFilter GetSmoothingFilter (SmoothingFilter filter)
        {
            switch(filter)
            {
                case SmoothingFilter.Mean:
                    return new Mean();
                case SmoothingFilter.Median:
                    return new Median();
                case SmoothingFilter.ConservativeSmoothing:
                    return new ConservativeSmoothing();
                case SmoothingFilter.BilateralSmoothing:
                    return new BilateralSmoothing();
                case SmoothingFilter.AdaptiveSmoothing:
                    return new AdaptiveSmoothing();
                default:
                    throw new ArgumentException("Incorrect filter name");
            }
        }
    }
}
