namespace FFM.nanoframework.ad4116
{
    public enum ad4116_register_t : byte
    {
		/* other ADC registers */
		COMMS_REG = 0x00,
		STATUS_REG = 0x00,
		ADCMODE_REG = 0x01,
		IFMODE_REG = 0x02,
		REGCHECK_REG = 0x03,
		DATA_REG = 0x04,
		GPIOCON_REG = 0x06,
		ID_REG = 0x07,
		/* ADC channel registers */
		CH0 = 0x10,
		CH1 = 0x11,
		CH2 = 0x12,
		CH3 = 0x13,
		CH4 = 0x14,
		CH5 = 0x15,
		CH6 = 0x16,
		CH7 = 0x17,
		CH8 = 0x18,
		CH9 = 0x19,
		CH10 = 0x1A,
		CH11 = 0x1B,
		CH12 = 0x1C,
		CH13 = 0x1D,
		CH14 = 0x1E,
		CH15 = 0x1F,
		/* ADC setup config register */
		SETUP0 = 0x20,
		SETUP1 = 0x21,
		SETUP2 = 0x22,
		SETUP3 = 0x23,
		SETUP4 = 0x24,
		SETUP5 = 0x25,
		SETUP6 = 0x26,
		SETUP7 = 0x27,
		/* ADC filter config registers */
		FILTER0 = 0x28,
		FILTER1 = 0x29,
		FILTER2 = 0x2A,
		FILTER3 = 0x2B,
		FILTER4 = 0x2C,
		FILTER5 = 0x2D,
		FILTER6 = 0x2E,
		FILTER7 = 0x2F,
		/* ADC offset registers */
		OFFSET0 = 0x30,
		OFFSET1 = 0x31,
		OFFSET2 = 0x32,
		OFFSET3 = 0x33,
		OFFSET4 = 0x34,
		OFFSET5 = 0x35,
		OFFSET6 = 0x36,
		OFFSET7 = 0x37,
		/* ADC gain registers */
		GAIN0 = 0x38,
		GAIN1 = 0x39,
		GAIN2 = 0x3A,
		GAIN3 = 0x3B,
		GAIN4 = 0x3C,
		GAIN5 = 0x3D,
		GAIN6 = 0x3E,
		GAIN7 = 0x3F

	}
}
