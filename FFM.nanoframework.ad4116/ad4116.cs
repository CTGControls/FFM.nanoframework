using System;
using System.Device.Spi;
using System.Threading;
using FFM.nanoframework.ad4116;

namespace FFM.nanoframework
{
    public class AD4116
    {

        private readonly ushort read_write_delay = 50;

        private data_mode_t m_data_mode;
        private bool append_status_reg;
        public SpiDevice _spiDevice;


        public AD4116(SpiDevice spiDevice)
        {
            _spiDevice = spiDevice;
        }

        public void reset()
        {
            for (int i = 0; i < 8; i++)
            {
                _spiDevice.WriteByte(0xFF);
            }

            Thread.Sleep(read_write_delay);

        }

        public void Sync()
        {

            throw new NotImplementedException();
        }




        public SpanByte get_register(byte registerAddress, byte numberOfBytes)
        {
            // clean up the communication register by sending 0x00 
            _spiDevice.WriteByte(0x00);

            Thread.Sleep(5);

            // send communication register the read command and the address to read
            SpanByte writeBuffer = new byte[1] { (byte)(0x40 | registerAddress) };
            SpanByte readBuffer = new byte[numberOfBytes + 1];

            // send communication register the read command and the address to read
            // then read back the results
            _spiDevice.TransferFullDuplex(writeBuffer, readBuffer);

            Thread.Sleep(read_write_delay);
            // Cean up the data by removing the first byte
            return readBuffer.Slice(1);
        }

        public void set_register(SpanByte writeData)
        {

            // clean up the communication register by sending 0x00 
            _spiDevice.WriteByte(0x00);
            Thread.Sleep(5);

            // Write the requested data to the controller
            // First byte is the address 
            _spiDevice.Write(writeData);

            Thread.Sleep(read_write_delay);

        }


        public int set_adc_mode_config(data_mode_t data_mode, clock_mode_t clock_mode, ref_mode_t ref_mode)
        {

            SpanByte writeData = new byte[3] { 0x00, 0x00, 0x00 };

            writeData[0] = (byte)ad4116_register_t.ADCMODE_REG;
            writeData[1] = (byte)((byte)ref_mode << 7);
            writeData[2] = (byte)(((byte)data_mode << 4) | ((byte)clock_mode << 2));

            set_register(writeData);

            get_register((byte)ad4116_register_t.ADCMODE_REG, 2);

            return 0;
        }

        public int set_interface_mode_config(bool continuous_read, bool append_status_reg)
        {
            /* Address: 0x02, Reset: 0x0000, Name: IFMODE */

            /* prepare the configuration value */
            /* RESERVED [15:13], ALT_SYNC [12], IOSTRENGTH [11], HIDE_DELAY [10], RESERVED [9], DOUT_RESET [8], CONTREAD [7], DATA_STAT [6], REG_CHECK [5], RESERVED [4], CRC_EN [3:2], RESERVED [1], WL16 [0] */
            SpanByte writeData = new byte[3] { 0x00, 0x00, 0x00 };

            writeData[0] = (byte)ad4116_register_t.IFMODE_REG;
            if (continuous_read)
            {
                writeData[2] = (byte)(writeData[2] | (1 << 7));
            }

            if (append_status_reg)
            {
                writeData[2] = (byte)(writeData[2] | (1 << 6));
            }

            /* update the configuration value */
            set_register(writeData);

            /* verify the updated configuration value */
            get_register((byte)ad4116_register_t.IFMODE_REG, 2);

            /* when continuous read mode */
            if (continuous_read)
            {
                /* update the data mode */
                m_data_mode = data_mode_t.CONTINUOUS_READ_MODE;
            }

            /* enable or disable appending status reg to data */
            this.append_status_reg = append_status_reg;

            return 0;
        }

        public SpanByte get_data()
        {
            /* when not in continuous read mode, send the read command */
            if (this.m_data_mode != data_mode_t.CONTINUOUS_READ_MODE)
            {
                return get_data_once();

            }

            return get_data_continuous();

        }

        private SpanByte get_data_once()
        {
            if (this.append_status_reg)
            {
                return get_register((byte)ad4116_register_t.DATA_REG, 4);
            }

            return get_register((byte)ad4116_register_t.DATA_REG, 3);
        }

        private SpanByte get_data_continuous()
        {
            int bufferSize = 3;
            if (this.append_status_reg)
            {
                bufferSize = 4;
            }

            SpanByte readBuffer = new byte[bufferSize];
            _spiDevice.Read(readBuffer);
            return readBuffer;
        }

        public bool is_valid_id()
        {
            SpanByte readBuffer = new byte[2] { 0x00, 0x00 };
            readBuffer = get_register((byte)ad4116_register_t.ID_REG, 2);

            readBuffer[1] &= 0xF0;

            return readBuffer[0] == 0x30 && readBuffer[1] == 0xD0;

        }

        public int set_channel_config(ad4116_register_t channel, bool enable, ad4116_register_t setup, analog_input_t ain_pos, analog_input_t ain_neg)
        {
            /* Address: 0x10, Reset: 0x8001, Name: CH0 */
            /* Address Range: 0x11 to 0x1F, Reset: 0x0001, Name: CH1 to CH15 */

            /* prepare the configuration value */
            /* CH_EN0 [15], SETUP_SEL0 [14:12], RESERVED [11:10], AINPOS0 [9:5], AINNEG0 [4:0] */
            SpanByte writeBuffer = new byte[3] { (byte)channel, 0x00, 0x00 };

            if (enable) writeBuffer[1] = (byte)(writeBuffer[1] | (1 << 7));

            writeBuffer[1] = (byte)(writeBuffer[1] | ((byte)setup << 4) | ((byte)ain_pos >> 3));
            writeBuffer[2] = (byte)(((byte)ain_pos << 5) | (byte)ain_neg);

            /* update the configuration value */
            set_register(writeBuffer);

            /* verify the updated configuration value */
            //get_register((byte)channel, 2);

            /* return error code */
            return 0;
        }
        public int set_setup_config(ad4116_register_t setup, coding_mode_t coding_mode, ain_buf_mode_t ain_buf_mode, setup_ref_source_t setup_ref_source)
        {
            SpanByte writeBuffer = new byte[3] { (byte)setup, 0x00, 0x00 };

            writeBuffer[1] = (byte)(((byte)coding_mode << 4) | (byte)ain_buf_mode);
            writeBuffer[2] = (byte)((byte)setup_ref_source << 4);

            /* update the configuration value */
            set_register(writeBuffer);

            /* verify the updated configuration value */
            get_register((byte)setup, 2);

            /* return error code */
            return 0;


        }
        public int set_filter_config(ad4116_register_t filter, data_rate_t data_rate)
        {
            SpanByte writeBuffer = new byte[3] { (byte)filter, 0x00, 0x00 };
            writeBuffer[2] = (byte)data_rate;

            /* update the configuration value */
            set_register(writeBuffer);

            /* verify the updated configuration value */
            get_register((byte)filter, 2);

            /* return error code */
            return 0;

        }

        public int set_offset_config(ad4116_register_t offset, UInt32 offset_value)
        {

            /* add the default offset value */
            offset_value += 8388608;
            /* prepare the configuration value */
            SpanByte writeBuffer = new byte[4] { (byte)offset, 0x00, 0x00, 0x00 };

            writeBuffer[1] = (byte)(((byte)offset_value) >> 16);
            writeBuffer[2] = (byte)(((byte)offset_value) >> 8);
            writeBuffer[3] = (byte)offset_value;

            /* update the configuration value */
            set_register(writeBuffer);

            /* verify the updated configuration value */
            get_register((byte)offset, 2);

            /* return error code */
            return 0;

        }
        }
}
