using System.Collections.Generic;

namespace MLG360.Model
{
    public class Versioned
    {
        public IReadOnlyDictionary<int, UnitAction> Inner { get; }

        public Versioned(IReadOnlyDictionary<int, UnitAction> inner)
        {
            Inner = inner;
        }

        public static Versioned ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var innerSize = reader.ReadInt32();
            var inner = new Dictionary<int, UnitAction>(innerSize);
            for (var i = 0; i < innerSize; i++)
            {
                int innerKey;
                innerKey = reader.ReadInt32();
                UnitAction innerValue;
                innerValue = UnitAction.ReadFrom(reader);
                inner.Add(innerKey, innerValue);
            }

            var result = new Versioned(inner);

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(43981);
            writer.Write(Inner.Count);
            foreach (var innerEntry in Inner)
            {
                var innerKey = innerEntry.Key;
                var innerValue = innerEntry.Value;
                writer.Write(innerKey);
                innerValue.WriteTo(writer);
            }
        }
    }
}
