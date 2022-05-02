using System;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;
using FFM.nanoframework.ad4116;
using nanoFramework.Hardware.Esp32;
using System.Device.Gpio;

namespace FFM.nanoframework.ESP32
{
    public class Program
    {
        public static void Main()
        {

            SpiDevice spiDevice;
            SpiConnectionSettings connectionSettings;

            GpioController gpioController = new GpioController();

            Configuration.SetPinFunction(35, DeviceFunction.SPI1_MOSI);
            Configuration.SetPinFunction(36, DeviceFunction.SPI1_CLOCK);
            Configuration.SetPinFunction(37, DeviceFunction.SPI1_MISO);
            //var chipSelectLine = gpioController.OpenPin(10, PinMode.Output);
            
            //Configuration.SetPinFunction(37, DeviceFunction.);

            SpiBusInfo spiBusInfo = SpiDevice.GetBusInfo(1);
            Debug.WriteLine($"{nameof(spiBusInfo.MaxClockFrequency)}: {spiBusInfo.MaxClockFrequency}"); //40000000
            Debug.WriteLine($"{nameof(spiBusInfo.MinClockFrequency)}: {spiBusInfo.MinClockFrequency}"); //78125

            //if (gpioController.GetPinMode(14) == PinMode.Output)
            //{
            //    gpioController.ClosePin(14);
            //}
            
            connectionSettings = new SpiConnectionSettings(1 , 10)
            {
                ClockFrequency = 4000000,
                ChipSelectLine = 10,
                ChipSelectLineActiveState = false,
                BusId = 1,
                DataBitLength = 8,
                Mode = SpiMode.Mode3,
                DataFlow = DataFlow.MsbFirst,
                SharingMode = SpiSharingMode.Exclusive
            };

            spiDevice = SpiDevice.Create(connectionSettings);

            Thread.Sleep(10);
            AD4116 ad4116 = new AD4116(spiDevice, true);


            ad4116.reset();

            Thread.Sleep(10);

            while (true)
            {
                var results = ad4116.get_register(0x7, 2);
            }

            /* set ADC input channel configuration */
            /* enable channel 0 and channel 1 and connect each to 2 analog inputs for bipolar input */
            /* CH0 - CH15 */
            /* true/false to enable/disable channel */
            /* SETUP0 - SETUP7 */
            /* AIN0 - AIN16 */
            //ad4116.set_channel_config(ad4116_register_t.CH0, true, ad4116_register_t.SETUP0, analog_input_t.AIN0, analog_input_t.AIN1);
            ad4116.get_register(0x10, 2);
            ad4116.set_register(new byte[] { 0x10, 0x80, 0x01});
            ad4116.get_register(0x10, 2);


            /* set the ADC SETUP0 coding mode to BIPLOAR output */
            /* SETUP0 - SETUP7 */
            /* BIPOLAR, UNIPOLAR */
            /* AIN_BUF_DISABLE, AIN_BUF_ENABLE */
            /* REF_EXT, REF_AIN, REF_INT, REF_PWR */
            ad4116.set_setup_config(ad4116_register_t.SETUP0, coding_mode_t.BIPOLAR, ain_buf_mode_t.AIN_BUF_ENABLE, setup_ref_source_t.REF_EXT);


            /* set ADC OFFSET0 offset value */
            /* OFFSET0 - OFFSET7 */
            ad4116.set_offset_config(ad4116_register_t.OFFSET0, 0x00);

            /* set the ADC FILTER0 ac_rejection to false and samplingrate to 1007 Hz */
            /* FILTER0 - FILTER7 */
            /* SPS_1, SPS_2, SPS_5, SPS_10, SPS_16, SPS_20, SPS_49, SPS_59, SPS_100, SPS_200 */
            /* SPS_381, SPS_503, SPS_1007, SPS_2597, SPS_5208, SPS_10417, SPS_15625, SPS_31250 */
            ad4116.set_filter_config(ad4116_register_t.FILTER0, data_rate_t.SPS_100);

            /* set the ADC data and clock mode */
            /* CONTINUOUS_CONVERSION_MODE, SINGLE_CONVERSION_MODE */
            /* in SINGLE_CONVERSION_MODE after all setup channels are sampled the ADC goes into STANDBY_MODE */
            /* to exit STANDBY_MODE use this same function to go into CONTINUOUS or SINGLE_CONVERSION_MODE */
            /* INTERNAL_CLOCK, INTERNAL_CLOCK_OUTPUT, EXTERNAL_CLOCK_INPUT, EXTERNAL_CRYSTAL */
            /* REF_DISABLE, REF_ENABLE */
            ad4116.set_adc_mode_config(data_mode_t.CONTINUOUS_CONVERSION_MODE, clock_mode_t.INTERNAL_CLOCK, ref_mode_t.REF_DISABLE);


            /* enable/disable CONTINUOUS_READ_MODE and appending STATUS register to data */
            /* to exit CONTINUOUS_READ_MODE use AD4116.reset(); */
            /* AD4116.reset(); returns all registers to default state, so everything has to be setup again */
            /* true / false to enable / disable appending status register to data, 4th byte */
            ad4116.set_interface_mode_config(true, true);

            /* wait for ADC */
            Thread.Sleep(30000);




            //SpanByte datax = new byte[2] { 0x00, 0x00 };
            //while (true)
            //{

            //    // clean up the communication register by sending 0x00 
            //    spiDevice.WriteByte(0x00);

            //    Thread.Sleep(5);

            //    // send communication register the read command and the address to read
            //    SpanByte writeBuffer = new byte[1] { (byte)(0x40 | 0x07) };
            //    SpanByte readBuffer = new byte[4];

            //    // send communication register the read command and the address to read
            //    // then read back the results
            //    spiDevice.TransferFullDuplex(writeBuffer, readBuffer);

            //    Debug.WriteLine(readBuffer[0].ToString());
            //    Debug.WriteLine(readBuffer[1].ToString("x"));
            //    Debug.WriteLine(readBuffer[2].ToString("x"));
            //    Debug.WriteLine(readBuffer[3].ToString());

            //    Debug.WriteLine("__");
            //    Thread.Sleep(250);

            //    datax = ad4116.get_register(0x07, 2);
            //    Debug.WriteLine(datax[0].ToString("x"));
            //    Debug.WriteLine(datax[1].ToString("x"));
            //    Debug.WriteLine("__");

            //}


            SpanByte data = new byte[4] { 0x00, 0x00, 0x00, 0x00 };

            while (true)
            {
                //var X = ad4116.get_register((byte)ad4116_register_t.ID_REG, 2);

                //Debug.WriteLine($"{X[0].ToString("x")}{X[1].ToString("x")}");



                data = ad4116.get_register(0x02, 2);

                Debug.WriteLine($"{data[0].ToString("x")}{data[1].ToString("x")}");





                data = ad4116.get_register(0x04, 5);

                Debug.WriteLine($"{data[0].ToString("x")}{data[1].ToString("x")}{data[2].ToString("x")}");




                data = ad4116.get_data();


                UInt64 id3 = (UInt64)((data[0] << 16) | (data[1] << 8) | data[2]);

                Debug.WriteLine(id3.ToString());

                double InputResultsScaled = 0.000001 * (5000000 * (id3 - 8387126) / 1676914);
                Debug.WriteLine(InputResultsScaled.ToString());

                Thread.Sleep(1000);

            }
        }
    }
}
