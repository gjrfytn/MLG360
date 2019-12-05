namespace MLG360.Model.Messages
{
    public class ActionMessage : PlayerMessage
    {
        public const int TAG = 1;

        public Versioned Action { get; }

        public ActionMessage(Versioned action)
        {
            Action = action;
        }

        public static new ActionMessage ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            return new ActionMessage(Versioned.ReadFrom(reader));
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
            Action.WriteTo(writer);
        }
    }
}
