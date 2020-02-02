using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleSample_Live
{
    public partial class View : Form
    {
        String Cam_IP;
        int m_PlayStatus;
        private AxIPROPSAPILib.AxipropsapiCtrl axipropsapiCtrl1;
        public View(String IP)
        {
            InitializeComponent();
            Cam_IP = IP;
        }
        private void View_Load(object sender, EventArgs e)
        {
            //Set properties to connect the device
            axipropsapiCtrl1.IPAddr = Cam_IP;
            axipropsapiCtrl1.DeviceType = 2;
            axipropsapiCtrl1.HttpPort = 80;
            axipropsapiCtrl1.UserName = "admin";
            axipropsapiCtrl1.Password = "ibst2997730!";

            //Set properties for display area
            axipropsapiCtrl1.StreamFormat = 0;
            axipropsapiCtrl1.JPEGResolution = 720;
            axipropsapiCtrl1.MPEG4Resolution = 720;
            axipropsapiCtrl1.H264Resolution = 720;

            //Set properties for event
            axipropsapiCtrl1.OnErrorEnable = 1;
            axipropsapiCtrl1.OnDevStatusEnable = 0;
            axipropsapiCtrl1.OnRecStatusEnable = 0;
            axipropsapiCtrl1.OnPlayStatusEnable = 1;
            axipropsapiCtrl1.OnImageRefreshEnable = 0;
            axipropsapiCtrl1.OnRecordStatusEnable = 0;
            axipropsapiCtrl1.OnOpStatusEnable = 0;
            axipropsapiCtrl1.OnAlmStatusEnable = 0;
            axipropsapiCtrl1.MouseDownEnable = 1;

            axipropsapiCtrl1.OnRecStatusCBEnable = 0;
            axipropsapiCtrl1.OnSearchCBEnable = 0;
            axipropsapiCtrl1.OnSearchExCBEnable = 0;
            axipropsapiCtrl1.OnPlayStatusCBEnable = 0;
            axipropsapiCtrl1.OnOpStatusCBEnable = 0;
            axipropsapiCtrl1.OnAlmStatusCBEnable = 0;
            axipropsapiCtrl1.OnFtpStatusCBEnable = 0;
            axipropsapiCtrl1.DblClickEnable = 1;
            //Define variables
            int iRet;
            int iChannel;
            int iBlockingMode;
            iRet = axipropsapiCtrl1.Open();
            //Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = axipropsapiCtrl1.PlayLive(iChannel, iBlockingMode);
                //Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    // m_PlayStatus = 1;
                }
                else
                {
                    axipropsapiCtrl1.Close();
                    //      Logging("[Function] Close");
                }
            }
        }

        private void AxipropsapiCtrl1_OnError(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnErrorEvent e)
        {

        }

        private void AxipropsapiCtrl1_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            this.Close();
        }

        private void View_Paint(object sender, PaintEventArgs e)
        {
            axipropsapiCtrl1.Width = this.ClientSize.Width;
            axipropsapiCtrl1.Height = this.ClientSize.Height;
        }
    }
}
