using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods
{

    public static bool IsEmpty<T>(this T[] self)
    {
        return self == null || self.Length <= 0;
    }

    public static bool IsEmpty<T>(this IList<T> self)
    {
        return self == null || self.Count <= 0;
    }

    public static void SnapToTarget(this ScrollRect self, RectTransform target, Vector2 margin)
    {
        self.content.localPosition -= self.SnapLocalPosition(target, margin);
    }

    public static Vector3 SnapLocalPosition(this ScrollRect self, RectTransform target, Vector2 margin)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 viewPosMin = self.viewport.rect.min;
        Vector2 viewPosMax = self.viewport.rect.max;

        Vector2 targetPosMin = self.viewport.InverseTransformPoint(target.TransformPoint(target.rect.min));
        Vector2 targetPosMax = self.viewport.InverseTransformPoint(target.TransformPoint(target.rect.max));

        targetPosMin -= margin;
        targetPosMax += margin;

        Vector2 move = Vector2.zero;

        if (targetPosMax.y > viewPosMax.y)
            move.y = targetPosMax.y - viewPosMax.y;
        if (targetPosMin.x < viewPosMin.x)
            move.x = targetPosMin.x - viewPosMin.x;
        if (targetPosMax.x > viewPosMax.x)
            move.x = targetPosMax.x - viewPosMax.x;
        if (targetPosMin.y < viewPosMin.y)
            move.y = targetPosMin.y - viewPosMin.y;

        Vector3 worldMove = self.viewport.TransformDirection(move);

        return self.content.InverseTransformDirection(worldMove);
    }

}
