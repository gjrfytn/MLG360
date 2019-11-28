namespace MLG360.Model
{
    public abstract class PlayerMessageGame
    {
        public abstract void WriteTo(System.IO.BinaryWriter writer);

        public static PlayerMessageGame ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            switch (reader.ReadInt32())
            {
                case CustomDataMessage.TAG:
                    return CustomDataMessage.ReadFrom(reader);
                case ActionMessage.TAG:
                    return ActionMessage.ReadFrom(reader);
                default:
                    throw new System.Exception("Unexpected discriminant value");
            }
        }

        public class CustomDataMessage : PlayerMessageGame
        {
            public const int TAG = 0;

            public CustomData Data { get; set; }

            public CustomDataMessage() { }

            public CustomDataMessage(CustomData data)
            {
                Data = data;
            }

            public static new CustomDataMessage ReadFrom(System.IO.BinaryReader reader)
            {
                var result = new CustomDataMessage();
                result.Data = CustomData.ReadFrom(reader);

                return result;
            }

            public override void WriteTo(System.IO.BinaryWriter writer)
            {
                if (writer == null)
                    throw new System.ArgumentNullException(nameof(writer));

                writer.Write(TAG);
                Data.WriteTo(writer);
            }
        }

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
}
