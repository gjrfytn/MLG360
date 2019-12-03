namespace MLG360.Model
{
    public class UnitAction
    {
        public double Velocity { get; }
        public bool Jump { get; }
        public bool JumpDown { get; }
        public Vec2Double Aim { get; }
        public bool Shoot { get; }
        public bool Reload { get; }
        public bool SwapWeapon { get; }
        public bool PlantMine { get; }

        public UnitAction(double velocity, bool jump, bool jumpDown, Vec2Double aim, bool shoot, bool reload, bool swapWeapon, bool plantMine)
        {
            Velocity = velocity;
            Jump = jump;
            JumpDown = jumpDown;
            Aim = aim;
            Shoot = shoot;
            Reload = reload;
            SwapWeapon = swapWeapon;
            PlantMine = plantMine;
        }

        public static UnitAction ReadFrom(System.IO.BinaryReader reader)
        {
            if (reader == null)
                throw new System.ArgumentNullException(nameof(reader));

            var result = new UnitAction(
                reader.ReadDouble(),
                reader.ReadBoolean(),
                reader.ReadBoolean(),
                Vec2Double.ReadFrom(reader),
                reader.ReadBoolean(),
                reader.ReadBoolean(),
                reader.ReadBoolean(),
                reader.ReadBoolean());

            return result;
        }

        public void WriteTo(System.IO.BinaryWriter writer)
        {
            if (writer == null)
                throw new System.ArgumentNullException(nameof(writer));

            writer.Write(Velocity);
            writer.Write(Jump);
            writer.Write(JumpDown);
            Aim.WriteTo(writer);
            writer.Write(Shoot);
            writer.Write(Reload);
            writer.Write(SwapWeapon);
            writer.Write(PlantMine);
        }
    }
}
