namespace MLG360.Model
{
    public class UnitAction
    {
        public double Velocity { get; set; }
        public bool Jump { get; set; }
        public bool JumpDown { get; set; }
        public Vec2Double Aim { get; set; }
        public bool Shoot { get; set; }
        public bool SwapWeapon { get; set; }
        public bool PlantMine { get; set; }

        public UnitAction(double velocity, bool jump, bool jumpDown, Vec2Double aim, bool shoot, bool swapWeapon, bool plantMine)
        {
            Velocity = velocity;
            Jump = jump;
            JumpDown = jumpDown;
            Aim = aim;
            Shoot = shoot;
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
            writer.Write(SwapWeapon);
            writer.Write(PlantMine);
        }
    }
}
