using UnityEditor;
using UnityEngine;

namespace CustomProperties.Editor {

    /// <summary>Helper methods for GUI drawing in the inspector.</summary>
    public static class EditorGuiHelpers {

        /// <summary>Split a position rect into the label part and the content part.</summary>
        /// <param name="position">Rect to split in two.</param>
        /// <param name="label">   Label part.</param>
        /// <param name="content"> Content part.</param>
        public static void GetLabelContentRects(Rect position, out Rect label, out Rect content) {
            label = new Rect(position);
            label.xMax = label.xMin + EditorGUIUtility.labelWidth;
            content = new Rect(position);
            content.xMin += EditorGUIUtility.labelWidth;
        }

        /// <summary>Split a rect into two parts. <paramref name="a"/> defines the split.</summary>
        /// <param name="rectToSplit">Rect to split</param>
        /// <param name="left">       Left rectangle</param>
        /// <param name="right">      Right rectangle</param>
        /// <param name="a">          
        /// A value (will be clamped 0 to 1) that defines the percentage where to split.
        /// </param>
        public static void SplitRectVertically(Rect rectToSplit, out Rect left, out Rect right, float a) {
            a = Mathf.Clamp01(a);
            float splitPoint = Mathf.LerpUnclamped(rectToSplit.xMin, rectToSplit.xMax, a);
            left = new Rect(rectToSplit) { xMax = splitPoint };
            right = new Rect(rectToSplit) { xMin = splitPoint };
        }
    }
}
