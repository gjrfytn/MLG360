using System.Numerics;

namespace MLG360.Strategy
{
    internal class WeaponOperation
    {
        public enum ActionType
        {
            None,
            Shoot,
            Reload
        }

        public Vector2 Aim { get; }
        public ActionType Action { get; }

        public WeaponOperation(Vector2 aim, ActionType action)
        {
            Aim = aim;
            Action = action;
        }
    }
}