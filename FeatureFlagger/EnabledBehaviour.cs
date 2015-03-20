﻿namespace FeatureFlagger
{
    using System;

    public class EnabledBehaviour : IBehaviour
    {
        public Func<string[], bool> Behaviour()
        {
            return x => Convert.ToBoolean(x[0]);
        }
    }
}