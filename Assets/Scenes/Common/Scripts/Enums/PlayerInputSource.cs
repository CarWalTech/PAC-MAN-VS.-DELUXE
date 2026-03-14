public enum PlayerInputSource
{
    CPU = -1,
    Gamepad1 = 0,
    Gamepad2 = 1,
    Gamepad3 = 2,
    Gamepad4 = 3,
    Gamepad5 = 4
}

static class PlayerInputSourceMethods
{
    public static PlayerSlot? ToSlot(this PlayerInputSource s1)
    {
        switch (s1)
        {
            case PlayerInputSource.Gamepad1:
                return PlayerSlot.P1;
            case PlayerInputSource.Gamepad2:
                return PlayerSlot.P2;
            case PlayerInputSource.Gamepad3:
                return PlayerSlot.P3;
            case PlayerInputSource.Gamepad4:
                return PlayerSlot.P4;
            case PlayerInputSource.Gamepad5:
                return PlayerSlot.P5;
            default:
                return null;
        }
    }
}