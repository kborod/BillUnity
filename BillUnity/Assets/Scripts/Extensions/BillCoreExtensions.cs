using Kborod.BilliardCore;
using UnityEngine;

namespace Kborod.Extensions
{
    public static class BillCoreExtensions
    {
        public static Vector2 ToVector2(this Point p) => new Vector2(p.x.ToFloat(), p.y.ToFloat());
        public static Point ToPoint(this Vector2 v) => new Point(Fixed64.FromFloat(v.x), Fixed64.FromFloat(v.y));
    }
}
