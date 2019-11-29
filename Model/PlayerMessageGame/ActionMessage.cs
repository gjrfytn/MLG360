namespace MLG360.Model.PlayerMessageGame
{
    public class ActionMessage : PlayerMessageGame
    {
        public const int TAG = 1;

        public System.Collections.Generic.IDictionary<int, UnitAction> Action { get; set; }

        public ActionMessage() { }

        public ActionMessage(System.Collections.Generic.IDictionary<int, UnitAction> action)
        {
            Action = action;
        }

        public static new ActionMessage ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new ActionMessage();
            var actionSize = reader.ReadInt32();
            result.Action = new System.Collections.Generic.Dictionary<int, UnitAction>(actionSize);

            for (var i = 0; i < actionSize; i++)
            {
                int actionKey;
                actionKey = reader.ReadInt32();
                UnitAction actionValue;
                actionValue = UnitAction.ReadFrom(reader);
                result.Action.Add(actionKey, actionValue);
            }

            return result;
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            writer.Write(Action.Count);

            foreach (var actionEntry in Action)
            {
                var actionKey = actionEntry.Key;
                var actionValue = actionEntry.Value;
                writer.Write(actionKey);
                actionValue.WriteTo(writer);
            }
        }
    }
}
