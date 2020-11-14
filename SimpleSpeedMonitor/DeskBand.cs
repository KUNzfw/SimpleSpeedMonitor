using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.SharpDeskBand;

namespace SimpleSpeedMonitor
{
    [ComVisible(true)]
    [DisplayName("Hello World")]
    public class DeskBand : SharpDeskBand
    {
        protected override UserControl CreateDeskBand()
        {
            return new DeskBandUI();
        }

        protected override BandOptions GetBandOptions()
        {
            return new BandOptions
            {
                HasVariableHeight = false,
                IsSunken = false,
                ShowTitle = true,
                Title = "Hello World",
                UseBackgroundColour = false,
                AlwaysShowGripper = true
            };
        }
    }
}
