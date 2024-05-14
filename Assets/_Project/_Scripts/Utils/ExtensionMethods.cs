using UnityEngine;
using UnityEngine.UI;

public static class ExtensionMethods
{

    public static void SnapToTarget(this ScrollRect self, RectTransform target)
    {
        self.content.localPosition -= self.SnapLocalPosition(target);
    }

    public static Vector3 SnapLocalPosition(this ScrollRect self, RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 viewPosMin = self.viewport.rect.min;
        Vector2 viewPosMax = self.viewport.rect.max;

        Vector2 childPosMin = self.viewport.InverseTransformPoint(target.TransformPoint(target.rect.min));
        Vector2 childPosMax = self.viewport.InverseTransformPoint(target.TransformPoint(target.rect.max));

        Vector2 move = Vector2.zero;

        if (childPosMax.y > viewPosMax.y)
            move.y = childPosMax.y - viewPosMax.y;
        if (childPosMin.x < viewPosMin.x)
            move.x = childPosMin.x - viewPosMin.x;
        if (childPosMax.x > viewPosMax.x)
            move.x = childPosMax.x - viewPosMax.x;
        if (childPosMin.y < viewPosMin.y)
            move.y = childPosMin.y - viewPosMin.y;

        Vector3 worldMove = self.viewport.TransformDirection(move);

        return self.content.InverseTransformDirection(worldMove);
    }

}
