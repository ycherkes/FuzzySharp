﻿using System;

namespace FuzzySharp.SimilarityRatio.Strategy
{
    internal static class PartialRatioStrategy
    {
        public static int Calculate(string input1, string input2)
        {
            string shorter;
            string longer;

            if (input1.Length == 0 || input2.Length == 0)
            {
                return 0;
            }

            if (input1.Length < input2.Length)
            {
                shorter = input1;
                longer  = input2;
            }
            else
            {
                shorter = input2;
                longer  = input1;
            }

            var matchingBlocks = Levenshtein.GetMatchingBlocks(shorter, longer);

            double maxScore = 0;

            foreach (var matchingBlock in matchingBlocks)
            {
                var dist = matchingBlock.DestPos - matchingBlock.SourcePos;

                var longStart = dist > 0 ? dist : 0;
                var longEnd   = longStart + shorter.Length;

                if (longEnd > longer.Length) longEnd = longer.Length;

                var longSubstr = longer.AsSpan()[longStart..longEnd];

                var ratio = Levenshtein.GetRatio(shorter, longSubstr);

                if (ratio > .995)
                {
                    return 100;
                }

                if (ratio > maxScore)
                {
                    maxScore = ratio;
                }
            }

            return (int)Math.Round(100 * maxScore);
        }
    }
}
