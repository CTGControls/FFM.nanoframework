namespace FFM.nanoframework.ad4116
{
    public enum analog_input_t : byte
    {
		AIN0 = 0x00,
		AIN1 = 0x01,
		AIN2 = 0x02,
		AIN3 = 0x03,
		AIN4 = 0x04,
		AIN5 = 0x05,
		AIN6 = 0x06,
		AIN7 = 0x07,
		AIN8 = 0x08,
		AIN9 = 0x09,
		AIN10 = 0x0A,
		AIN11 = 0x0B,
		AIN12 = 0x0C,
		AIN13 = 0x0D,
		AIN14 = 0x0E,
		AIN15 = 0x0F,
		AIN16 = 0x10,
		/* other ADC analog inputs */
		TEMP_SENSOR_POS = 0x11,
		TEMP_SENSOR_NEG = 0x12,
		REF_POS = 0x15,
		REF_NEG = 0x16
	}
}
