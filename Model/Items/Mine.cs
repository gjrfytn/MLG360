﻿namespace MLG360.Model.Items
{
    public class Mine : Item
    {
        public const int TAG = 2;

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(TAG);
        }
    }
}
