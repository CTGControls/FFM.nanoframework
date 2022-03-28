using System;
using System.Device.Spi;
using System.Diagnostics;
using System.Threading;
using FFM.nanoframework.ad4116;

namespace FFM.nanoframework.ESP32
{
    public class Program
    {
        public static void Main()
        {
            SpiDevice spiDevice;
            SpiConnectionSettings connectionSettings;

            AD4116 ad = new AD4116();
            /* set ADC input channel configuration */
            /* enable channel 0 and channel 1 and connect each to 2 analog inputs for bipolar input */
            /* CH0 - CH15 */
            /* true/false to enable/disable channel */
            /* SETUP0 - SETUP7 */
            /* AIN0 - AIN16 */
            AD4116.set_channel_config(ad4116_register_t.CH0, true, ad4116_register_t.SETUP0, analog_input_t.AIN0, analog_input_t.AIN1);

            Debug.WriteLine("Hello from nanoFramework!");

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
