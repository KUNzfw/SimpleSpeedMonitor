using System.Windows.Forms;
using System.Net.NetworkInformation;
using Timer = System.Timers.Timer;
using System;

namespace SimpleSpeedMonitor
{
    public partial class DeskBandUI : UserControl
    {
        private readonly NetworkInterface[] _interfaces;
        private NetworkInterface _activity;
        private readonly Timer _timer;
        private ToolStripMenuItem _checkedItem;

        private long oldr, olds;

        public DeskBandUI()
        {
            InitializeComponent();
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerElapsed;
            _interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface i in _interfaces)
            {
                this.contextMenuStrip1.Items.Add(i.Description, null, (sender, e) =>
              {
                  SetActivityInterface(i);
                  _checkedItem.Checked = false;
                  _checkedItem = (sender as ToolStripMenuItem);
                  _checkedItem.Checked = true;
              });
            }
            SetActivityInterface(_interfaces[0]);
            _checkedItem = (contextMenuStrip1.Items[0] as ToolStripMenuItem);
            _checkedItem.Checked = true;
        }

        private void SetActivityInterface(NetworkInterface i)
        {
            _timer.Stop();
            _activity = i;
            var statistics = i.GetIPStatistics();
            oldr = statistics.BytesReceived;
            olds = statistics.BytesSent;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var statistics = _activity.GetIPStatistics();
            long r = statistics.BytesReceived, s = statistics.BytesSent;
            this.labelText.Text = string.Format("U: {0}\nD: {1}", FormatSpeed(s - olds), FormatSpeed(r - oldr));
            oldr = r; olds = s;
        }

        private string FormatSpeed(long speed)
        {
            if (speed > 838_860) // 1024*1024*80%
            {
                double t = speed / (1024.0 * 1024);
                if (t >= 10.0) return string.Format("{0:F1}mB/s", t);
                else return string.Format("{0:F2}mB/s", t);
            }
            else
            {
                double t = speed / 1024;
                if (t >= 10.0) return string.Format("{0:F1}kB/s", t);
                else return string.Format("{0:F2}kB/s", t);
            }
        }
    }
}
