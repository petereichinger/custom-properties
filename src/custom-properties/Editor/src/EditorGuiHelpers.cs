using UnityEngine;

namespace UnityExtensions.CustomProperties.Editor {

    /// <summary>Helper methods for GUI drawing in the inspector.</summary>
    internal static class EditorGuiHelpers {

        /// <summary>Split a rect into two parts. <paramref name="a"/> defines the split.</summary>
        /// <param name="rectToSplit">Rect to split</param>
        /// <param name="left">       Left rectangle</param>
        /// <param name="right">      Right rectangle</param>
        /// <param name="a">          
        /// A value (will be clamped 0 to 1) that defines the percentage where to split.
        /// </param>
        public static void SplitRectVertically(Rect rectToSplit, out Rect left, out Rect right, float a) {
            a = Mathf.Clamp01(a);
            var splitPoint = Mathf.LerpUnclamped(rectToSplit.xMin, rectToSplit.xMax, a);
            left = new Rect(rectToSplit) {xMax = splitPoint};
            right = new Rect(rectToSplit) {xMin = splitPoint};
        }
    }
}
