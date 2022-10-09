using UnityEngine;

namespace UI
{
    public class ConnectionElement : MonoBehaviour
    {
        public void Setup(Vector2 pos1, Vector2 pos2)
        {
            var centerPos = (pos1 + pos2) / 2;
            var direction = pos2 - pos1;

            transform.rotation = Quaternion.Euler(0,0,Vector2.SignedAngle(Vector2.up, direction));
            transform.localPosition = centerPos;

            ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, direction.magnitude);
        }
    }
}