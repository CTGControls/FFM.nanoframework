namespace FFM.nanoframework.ad4116
{
    public enum clock_mode_t : byte
    {
        INTERNAL_CLOCK = 0x00,
        INTERNAL_CLOCK_OUTPUT = 0x01,
        EXTERNAL_CLOCK_INPUT = 0x02,
        EXTERNAL_CRYSTAL = 0x03
    }
}
