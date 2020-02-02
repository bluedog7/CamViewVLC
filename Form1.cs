using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using SimpleSample_Live;
using Vlc.DotNet.Forms;
namespace SimpleSample_Audio
{
    public partial class Form1 : Form
    {
        AxIPROPSAPILib.AxipropsapiCtrl[] CameraControls;
        VlcControl[] CameraControls2;
        String[] RTSP;

        int SelectedCamera = 0;
        bool fullscreen = false;
        Point saved_posiztion;
        int save_Width, save_Height;
        //CodeVendor.Controls.Grouper[] Borders;
        RadioButton[] Ctlchecked;
        AppSettingsReader ar;
        public Form1()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------
        // Define variables
        //---------------------------------------------------------------------
       // private Form2 frmForm2;

        int m_PlayStatus = 0;    //LiveStatus 0:Stop 1:Live

        //---------------------------------------------------------------------
        // Function Name        : Load
        // Overview             : Load PSAPI and Initialize
        //---------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            // MessageBox.Show("Please confirm beforehand that the audio function of a target device is available.", "SimpleSample_Audio", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Show the log window
            //  frmForm2 = new Form2();
            //frmForm2.Show();
            CameraControls = new AxIPROPSAPILib.AxipropsapiCtrl[1];
            CameraControls2 = new VlcControl[24];
            RTSP = new String[24];
            Ctlchecked = new RadioButton[24];
            ar = new AppSettingsReader();
            //Set properties to connect the device
            for (int i = 0; i < 1; i++)
            {
                CameraControls[i] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
                CameraControls[i].IPAddr = (String)ar.GetValue("cam" + (i + 1).ToString(), typeof(String));
                CameraControls[i].DeviceType = 2;
                CameraControls[i].HttpPort = 80;
                CameraControls[i].UserName = "admin";
                CameraControls[i].Password = "ibst2997730!";

                //Set properties for display area
                CameraControls[i].StreamFormat = 0;
                CameraControls[i].JPEGResolution = 320;
                CameraControls[i].MPEG4Resolution = 320;
                CameraControls[i].H264Resolution = 320;

                //Set properties for event
                CameraControls[i].OnErrorEnable = 1;
                CameraControls[i].OnDevStatusEnable = 0;
                CameraControls[i].OnRecStatusEnable = 0;
                CameraControls[i].OnPlayStatusEnable = 1;
                CameraControls[i].OnImageRefreshEnable = 0;
                CameraControls[i].OnRecordStatusEnable = 0;
                CameraControls[i].OnOpStatusEnable = 0;
                CameraControls[i].OnAlmStatusEnable = 0;
                CameraControls[i].MouseDownEnable = 1;

                CameraControls[i].OnRecStatusCBEnable = 0;
                CameraControls[i].OnSearchCBEnable = 0;
                CameraControls[i].OnSearchExCBEnable = 0;
                CameraControls[i].OnPlayStatusCBEnable = 0;
                CameraControls[i].OnOpStatusCBEnable = 0;
                CameraControls[i].OnAlmStatusCBEnable = 0;
                CameraControls[i].OnFtpStatusCBEnable = 0;

                CameraControls[i].Width = 227;
                CameraControls[i].Height = 157;
            }
            for (int i = 0; i < 24; i++)
            {
                CameraControls2[i] = (VlcControl)this.Controls["vlcControl" + (i + 1).ToString()];
                //vlcControl1.Play(new Uri("rtsp://admin:ibst2997730!@10.230.5.204:554/MediaInput/h264/stream_1"), options);
                RTSP[i] = "rtsp://admin:ibst2997730!@" + (String)ar.GetValue("cam" + (i + 1).ToString(), typeof(String)) + ":554/MediaInput/h264/stream_3";
             
                CameraControls2[i].Width = 227 ;
                CameraControls2[i].Height = 227*3/4;
                CameraControls2[i].Video.AspectRatio = "4:3";
                CameraControls2[i].Video.IsMouseInputEnabled = false;
                CameraControls2[i].Video.IsKeyInputEnabled = false;

                CameraControls2[i].Audio.Volume = 0;
                CameraControls2[i].Audio.IsMute = true;
            }
            Ctlchecked = new RadioButton[24];
            for (int i = 0; i < 24; i++)
            {
                Ctlchecked[i] = (RadioButton)this.Controls["radioButton" + (i + 1).ToString()];
                Ctlchecked[i].Checked = false;
                Ctlchecked[i].Text = (String)ar.GetValue("group" + (i + 1).ToString(), typeof(String));
            }
            for (int i = 0; i < 1; i++)
            {
                CameraControls[i].MouseDownEnable = 1;
                CameraControls[i].DblClickEnable = 1;
                //Audio setting
                axipropsapiCtrl1.AudioRcvEnable = 1;
                axipropsapiCtrl1.AudioRcvVolume = 0;
                axipropsapiCtrl1.AudioRcvMute = 0;
            }

            SelectedCamera = 0;

        }

        //---------------------------------------------------------------------
        // Function Name        : FormClosed
        // Overview             : Destroy log window
        //---------------------------------------------------------------------
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //----------------------------------------
            //Set properties
            //----------------------------------------
            //Set properties for event
            axipropsapiCtrl1.OnErrorEnable = 0;
            axipropsapiCtrl1.OnDevStatusEnable = 0;
            axipropsapiCtrl1.OnRecStatusEnable = 0;
            axipropsapiCtrl1.OnPlayStatusEnable = 0;
            axipropsapiCtrl1.OnImageRefreshEnable = 0;
            axipropsapiCtrl1.OnRecordStatusEnable = 0;
            axipropsapiCtrl1.OnOpStatusEnable = 0;
            axipropsapiCtrl1.OnAlmStatusEnable = 0;

            axipropsapiCtrl1.OnRecStatusCBEnable = 0;
            axipropsapiCtrl1.OnSearchCBEnable = 0;
            axipropsapiCtrl1.OnSearchExCBEnable = 0;
            axipropsapiCtrl1.OnPlayStatusCBEnable = 0;
            axipropsapiCtrl1.OnOpStatusCBEnable = 0;
            axipropsapiCtrl1.OnAlmStatusCBEnable = 0;
            axipropsapiCtrl1.OnFtpStatusCBEnable = 0;

   
        }

        //---------------------------------------------------------------------
        // Function Name        : Logging
        // Overview             : Output Logs
        //---------------------------------------------------------------------
        private void Logging(String str)
        {
            return;


        }

        //---------------------------------------------------------------------
        // Function Name        : ShowResult
        // Overview             : Output list Search result
        //---------------------------------------------------------------------
        private void ShowResult()
        {
            //Set Search Result to list
        }

        //---------------------------------------------------------------------
        // Function Name        : ShowResultEx
        // Overview             : Output list SearchEx/VMDSearchEx result
        //---------------------------------------------------------------------
        private void ShowResultEx()
        {
            //Set SearchEx Result to list
        }

        //---------------------------------------------------------------------
        // Function Name        : StartLive
        // Overview             : Start live video play
        //---------------------------------------------------------------------
        private void buttonLiveStart_Click(object sender, EventArgs e)
        {
            //Define variables
            int iRet;
            int iChannel;
            int iBlockingMode;

            //전체 카메라 play

                //Connect to the device
                iRet = CameraControls[0].Open();
                Logging("[Function] Open:" + iRet.ToString());

                if (iRet > -1)
                {
                    //Audio setting
                    CameraControls[0].AudioRcvEnable = 1;
                    CameraControls[0].AudioRcvVolume = 0;
                    CameraControls[0].AudioRcvMute = 0;
                    //CameraControls[i].RcvAudioDec = 0;

                    //Start Live
                    iChannel = 1;
                    iBlockingMode = 0;
                    iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                    Logging("[Function] PlayLive(Start):" + iRet.ToString());

                    if (iRet == 0)
                    {
                        m_PlayStatus = 1;
                    }
                    else
                    {
                        CameraControls[0].Close();
                        Logging("[Function] Close");
                    }
                }

            string[] options = { ":network-caching=100" };
            for (int i = 0; i < 12; i++)
                CameraControls2[i].Play(new Uri(RTSP[i]), options);
         /*   vlcControl1.Play(new Uri(RTSP[0]),options);
            vlcControl2.Play(new Uri(RTSP[1]), options);
            vlcControl3.Play(new Uri(RTSP[2]), options);
            vlcControl4.Play(new Uri(RTSP[3]), options);
            vlcControl5.Play(new Uri(RTSP[4]), options);
            vlcControl6.Play(new Uri(RTSP[5]), options);
            vlcControl7.Play(new Uri(RTSP[6]), options);
            vlcControl8.Play(new Uri(RTSP[7]), options);
            vlcControl9.Play(new Uri(RTSP[8]), options);
            vlcControl10.Play(new Uri(RTSP[9]), options);
            vlcControl11.Play(new Uri(RTSP[10]), options);
            vlcControl12.Play(new Uri(RTSP[11]), options);
            vlcControl7.Play(new Uri(RTSP[6]), options);
            vlcControl8.Play(new Uri(RTSP[7]), options);
            vlcControl9.Play(new Uri(RTSP[8]), options);
            vlcControl10.Play(new Uri(RTSP[9]), options);
            vlcControl11.Play(new Uri(RTSP[10]), options);
            vlcControl12.Play(new Uri(RTSP[11]), options);*/
            //vlcControl12.Play(new Uri("rtsp://admin:ibst2997730!@10.230.5.217:554/MediaInput/h264/stream_1"),options);

        }

        //---------------------------------------------------------------------
        // Function Name        : StopLive
        // Overview             : Stop live video play
        //---------------------------------------------------------------------
        private void buttonLiveStop_Click(object sender, EventArgs e)
        {
            //Define variables
            int iRet;
            int iCommand;
            int iSpeed;
            int iBlockMode;

            if (m_PlayStatus == 1)
            {
                //Stop Live
                iCommand = 1;
                iSpeed = 0;
                iBlockMode = 0;
                iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
                Logging("[Function] PlayLive(Stop):" + iRet.ToString());

                //Close connection to the device
                axipropsapiCtrl1.Close();
                Logging("[Function] Close");

                //ClearImage
                axipropsapiCtrl1.ClearImage();
                Logging("[Function] ClearImage");

                //Change status
                m_PlayStatus = 0;
            }
            else
            {
                Logging("[Message] No live.");
            }
        }

        //---------------------------------------------------------------------
        // Function Name        : Audio Start
        // Overview             : Start Audio transmission
        //---------------------------------------------------------------------
        private void bAudioStart_Click(object sender, EventArgs e)
        {
            //Define variables
            int iRet;
            int iCommand;
            int iStatus;
            axipropsapiCtrl1.AudioRcvMute = 0;
            if (m_PlayStatus == 1)
            {
                //Audio setting
                axipropsapiCtrl1.AudioSendVolume = 10;
                axipropsapiCtrl1.AudioSendMute = 0;

                //Start audio transmission
                iStatus = axipropsapiCtrl1.GetAudioSendStatus();
                if (iStatus == 0)
                {
                    iCommand = 1;
                    iRet = axipropsapiCtrl1.AudioSend(iCommand);
                    Logging("[Function] AudioSend(Start):" + iRet.ToString());
                }
                else
                {
                    Logging("[Message] No audio transmission.");
                }
            }
            else
            {
                Logging("[Message] No live.");
            }
        }

        //---------------------------------------------------------------------
        // Function Name        : Audio Stop
        // Overview             : Stop Audio transmission
        //---------------------------------------------------------------------
        private void bAudioStop_Click(object sender, EventArgs e)
        {
            //Define variables
            int iRet;
            int iCommand;
            int iStatus;

            if (m_PlayStatus == 1)
            {
                //Stop audio transmission
                iStatus = axipropsapiCtrl1.GetAudioSendStatus();
                if (iStatus == 1)
                {
                    iCommand = 0;
                    iRet = axipropsapiCtrl1.AudioSend(iCommand);
                    Logging("[Function] AudioSend(Stop):" + iRet.ToString());
                }
                else
                {
                    Logging("[Message] Cannot use audio transmission.");
                }
            }
            else
            {
                Logging("[Message] No live.");
            }
        }

        //---------------------------------------------------------------------
        // Function Name        : OnError
        // Overview             : OnError listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnError(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnErrorEvent e)
        {
            //Output Logs
            Logging("[OnError] errorCode[" + e.errorCode.ToString() + "] description[" + e.description + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnDevStatus
        // Overview             : OnDevStatus listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnDevStatus(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnDevStatusEvent e)
        {
            //Output Logs
            Logging("[OnDevStatus] channel[" + e.channel + "] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnRecStatu
        // Overview             : OnRecStatu listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnRecStatus(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnRecStatusEvent e)
        {
            //Output Logs
            Logging("[OnRecStatu] channel[" + e.channel + "] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnPlayStatus
        // Overview             : OnPlayStatus listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnPlayStatus(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnPlayStatusEvent e)
        {
            //Output Logs
            Logging("[OnPlayStatus] channel[" + e.channel + "] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnRecordStatus
        // Overview             : OnRecordStatus listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnRecordStatus(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnRecordStatusEvent e)
        {
            //Output Logs
            Logging("[OnRecordStatus] type[" + e.recType + "] timeDate[" + e.timeDate + "] nextRecTime[" + e.nextRecTime + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnImageRefresh
        // Overview             : OnImageRefresh listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnImageRefresh(object sender, EventArgs e)
        {
            //Output Logs
            Logging("[OnImageRefresh] No argument.");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnOpStatus
        // Overview             : OnOpStatus listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnOpStatus(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnOpStatusEvent e)
        {
            //Output Logs
            Logging("[OnOpStatus] channel[" + e.channel + "] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnAlmStatus
        // Overview             : OnAlmStatus listener event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnAlmStatus(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnAlmStatusEvent e)
        {
            //Output Logs
            Logging("[OnAlmStatus] channel[" + e.channel + "] type[" + e.alarmtype + "] timeDate[" + e.timeDate + "] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnRecStatusCB
        // Overview             : OnRecStatusCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnRecStatusCB(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnRecStatusCBEvent e)
        {
            //Output Logs
            Logging("[OnRecStatusCB] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnSearchCB
        // Overview             : OnSearchCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnSearchCB(object sender, EventArgs e)
        {
            //Output Logs
            Logging("[OnSearchCB] Show Search result.");
            ShowResult();
        }

        //---------------------------------------------------------------------
        // Function Name        : OnSearchExCB
        // Overview             : OnSearchExCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnSearchExCB(object sender, EventArgs e)
        {
            //Output Logs
            Logging("[OnSearchExCB] Show SearchEx result.");
            ShowResultEx();
        }

        //---------------------------------------------------------------------
        // Function Name        : OnPlayStatusCB
        // Overview             : OnPlayStatusCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnPlayStatusCB(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnPlayStatusCBEvent e)
        {
            //Output Logs
            Logging("[OnPlayStatusCB] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnOpStatusCB
        // Overview             : OnOpStatusCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnOpStatusCB(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnOpStatusCBEvent e)
        {
            //Output Logs
            Logging("[OnOpStatusCB] status[" + e.status + "]");
        }

        //---------------------------------------------------------------------
        // Function Name        : OnAlmStatusCB
        // Overview             : OnAlmStatusCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnAlmStatusCB(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnAlmStatusCBEvent e)
        {
            //Output Logs
            Logging("[OnAlmStatusCB] status[" + e.status + "]");
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button2.Visible = true;
            button1.Visible = false;
            //Audio setting
            CameraControls[0].AudioRcvVolume = 80;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            button2.Visible = false;
            button1.Visible = true;
            //Audio setting
            CameraControls[0].AudioRcvVolume = 0;
        }

        private void AxipropsapiCtrl1_OnError_1(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnErrorEvent e)
        {

        }

        private void AxipropsapiCtrl1_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 0;
          //  CamView.View v = new CamView.View(CameraControls[SelectedCamera].IPAddr);
          //  v.Show();
        }

        private void AxipropsapiCtrl1_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 0;
            Ctlchecked[0].Checked = true;
        }
        private void AxipropsapiCtrl5_OnError(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnErrorEvent e)
        {
            SelectedCamera = 4;
            //   button3.PerformClick();
        }

        private void AxipropsapiCtrl5_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {

            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 4;
            Ctlchecked[4].Checked = true;
        }



        private void AxipropsapiCtrl2_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 1;
            Ctlchecked[1].Checked = true;
        }

        private void AxipropsapiCtrl3_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 2;
            Ctlchecked[2].Checked = true;
        }

        private void AxipropsapiCtrl4_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 3;
            Ctlchecked[3].Checked = true;
        }

        private void AxipropsapiCtrl6_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 5;
            Ctlchecked[5].Checked = true;
        }

        private void AxipropsapiCtrl7_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 6;
            Ctlchecked[6].Checked = true;
        }

        private void AxipropsapiCtrl8_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 7;
            Ctlchecked[7].Checked = true;
        }

        private void AxipropsapiCtrl9_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 8;
            Ctlchecked[8].Checked = true;
        }

        private void AxipropsapiCtrl11_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 10;
            Ctlchecked[10].Checked = true;
        }

        private void AxipropsapiCtrl10_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 9;
            Ctlchecked[9].Checked = true;
        }

        private void AxipropsapiCtrl12_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 11;
            Ctlchecked[11].Checked = true;
        }


        private void AxipropsapiCtrl2_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 1;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl3_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 2;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl4_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 3;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl9_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 8;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl6_OnError(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnErrorEvent e)
        {

        }

        private void AxipropsapiCtrl6_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 5;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl7_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 6;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl8_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 7;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl10_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 9;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl11_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 10;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl12_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 11;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void AxipropsapiCtrl5_DblClick(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_DblClickEvent e)
        {
            SelectedCamera = 4;

            SimpleSample_Live.View v = new SimpleSample_Live.View(CameraControls[SelectedCamera].IPAddr);
            v.Show();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = -30;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            System.Threading.Thread.Sleep(1000);
            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = axipropsapiCtrl1.CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 30;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            System.Threading.Thread.Sleep(1000);
            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void Button5_Click(object sender, EventArgs e)
        {

            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = -30;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            System.Threading.Thread.Sleep(1000);
            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 30;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            System.Threading.Thread.Sleep(1000);
            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (!fullscreen)
            {
                fullscreen = true;
                saved_posiztion = CameraControls2[SelectedCamera].Location;
                //Borders[0].Location = new Point(0, 0);
                save_Width = CameraControls2[SelectedCamera].Width;
                save_Height = CameraControls2[SelectedCamera].Height;
                CameraControls2[SelectedCamera].Width = 1024; //349,255
                CameraControls2[SelectedCamera].Height = 768;
                CameraControls2[SelectedCamera].BringToFront();
                // CameraControls[SelectedCamera].Location.X = 0;
                CameraControls2[SelectedCamera].Location = new Point(300, 150);

            }
            else
            {
                fullscreen = false;

                //Borders[0].Location = new Point(0, 0);
                CameraControls2[SelectedCamera].Width = save_Width; //349,255
                CameraControls2[SelectedCamera].Height = save_Height;
                CameraControls2[SelectedCamera].BringToFront();
                // CameraControls[SelectedCamera].Location.X = 0;
                CameraControls2[SelectedCamera].Location = saved_posiztion;
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 1;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[SelectedCamera].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            Logging("[Function] CamControl(Tele):" + iRet.ToString());
            System.Threading.Thread.Sleep(1000);
            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[SelectedCamera].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = -1;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[SelectedCamera].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            Logging("[Function] CamControl(Tele):" + iRet.ToString());
            System.Threading.Thread.Sleep(1000);
            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[SelectedCamera].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RadioButton1_MouseDown(object sender, MouseEventArgs e)
        {
            //Define variables

            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 0;
            Ctlchecked[0].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }

        }

        private void RadioButton2_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 1;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton3_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 2;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton4_MouseEnter(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 3;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton5_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 4;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton6_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 5;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton7_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 6;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton8_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 7;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton9_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 8;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton10_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 9;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton11_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 10;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void RadioButton12_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 11;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void axipropsapiCtrl13_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 12;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void axipropsapiCtrl14_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 13;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void axipropsapiCtrl15_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 14;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void axipropsapiCtrl16_MouseMoveEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseMoveEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 15;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void axipropsapiCtrl17_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 16;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void axipropsapiCtrl18_MouseDownEvent(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_MouseDownEvent e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 17;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton14_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 13;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton15_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 14;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton16_MouseDown(object sender, MouseEventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 15;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void button3_MouseDown(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = -30;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);

        }

        private void button3_MouseUp(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            System.Threading.Thread.Sleep(1000);
        }

        private void button5_MouseDown(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = -30;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);

        }

        private void button5_MouseUp(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
            System.Threading.Thread.Sleep(1000);
        }

        private void button6_MouseDown(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 30;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);

        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 30;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);

        }

        private void button4_MouseUp(object sender, MouseEventArgs e)
        {
            int iRet;
            int iChannel;
            int iPan;
            int iTilt;
            int iZoom;
            int iFocus;
            int iIris;


            iChannel = 1;
            iPan = 0;
            iTilt = 0;
            iZoom = 0;
            iFocus = 0;
            iIris = 0;
            iRet = CameraControls[0].CameraControl(iChannel, iPan, iTilt, iZoom, iFocus, iIris);
        }

        private void vlcControl1_Click(object sender, EventArgs e)
        {
            radioButton1.PerformClick();
        }

        private void vlcControl2_Click(object sender, EventArgs e)
        {
            radioButton2.PerformClick();
        }

        private void radioButton12_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 11;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton11_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 10;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 1;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 2;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 3;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void vlcControl5_Click(object sender, EventArgs e)
        {
            radioButton5.PerformClick();
        }

        private void radioButton5_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 4;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton6_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 5;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton7_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 6;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton8_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 7;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton9_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 8;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton10_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 9;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton13_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 12;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton14_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 13;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton15_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 14;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton16_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 15;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton17_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 16;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton18_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 17;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton19_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 18;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton20_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 19;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton21_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 20;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton22_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 21;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton23_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 22;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void radioButton24_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 23;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }

        private void vlcControl3_Click(object sender, EventArgs e)
        {
            radioButton3.PerformClick();
        }

        private void vlcControl4_Click(object sender, EventArgs e)
        {
            radioButton4.PerformClick();
        }

        private void vlcControl6_Click(object sender, EventArgs e)
        {
            radioButton6.PerformClick();
        }

        private void vlcControl7_Click(object sender, EventArgs e)
        {
            radioButton7.PerformClick();
        }

        private void vlcControl8_Click(object sender, EventArgs e)
        {
            radioButton8.PerformClick();
        }

        private void vlcControl9_Click(object sender, EventArgs e)
        {
            radioButton9.PerformClick();
        }

        private void vlcControl10_Click(object sender, EventArgs e)
        {
            radioButton10.PerformClick();
        }

        private void vlcControl11_Click(object sender, EventArgs e)
        {
            radioButton11.PerformClick();
        }

        private void vlcControl12_Click(object sender, EventArgs e)
        {
            radioButton12.PerformClick();
        }

        private void vlcControl13_Click(object sender, EventArgs e)
        {
            radioButton13.PerformClick();
        }

        private void vlcControl14_Click(object sender, EventArgs e)
        {
            radioButton14.PerformClick();
        }

        private void vlcControl15_Click(object sender, EventArgs e)
        {
            radioButton15.PerformClick();
        }

        private void vlcControl16_Click(object sender, EventArgs e)
        {
            radioButton16.PerformClick();
        }

        private void vlcControl17_Click(object sender, EventArgs e)
        {
            radioButton17.PerformClick();
        }

        private void vlcControl18_Click(object sender, EventArgs e)
        {
            radioButton18.PerformClick();
        }

        private void vlcControl19_Click(object sender, EventArgs e)
        {
            radioButton19.PerformClick();
        }

        private void vlcControl20_Click(object sender, EventArgs e)
        {
            radioButton20.PerformClick();
        }

        private void vlcControl21_Click(object sender, EventArgs e)
        {
            radioButton21.PerformClick();
        }

        private void vlcControl22_Click(object sender, EventArgs e)
        {
            radioButton22.PerformClick();
        }

        private void vlcControl23_Click(object sender, EventArgs e)
        {
            radioButton23.PerformClick();
        }

        private void vlcControl24_Click(object sender, EventArgs e)
        {
            radioButton24.PerformClick();
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            Ctlchecked[SelectedCamera].Checked = false;
            SelectedCamera = 0;
            Ctlchecked[SelectedCamera].Checked = true;
            //Define variables
            int iRet;
            int iChannel, iCommand, iSpeed, iBlockMode;
            int iBlockingMode;

            //      CameraControls[0] = (AxIPROPSAPILib.AxipropsapiCtrl)this.Controls["axipropsapiCtrl" + (i + 1).ToString()];
            //Stop Live
            iCommand = 1;
            iSpeed = 0;
            iBlockMode = 0;
            iRet = axipropsapiCtrl1.PlayControl(iCommand, iSpeed, iBlockMode);
            CameraControls[0].Close();
            axipropsapiCtrl1.ClearImage();
            CameraControls[0].IPAddr = (String)ar.GetValue("cam" + (SelectedCamera + 1).ToString(), typeof(String));
            iRet = CameraControls[0].Open();
            Logging("[Function] Open:" + iRet.ToString());

            if (iRet > -1)
            {
                //Audio setting
                CameraControls[0].AudioRcvEnable = 1;
                CameraControls[0].AudioRcvVolume = 0;
                CameraControls[0].AudioRcvMute = 0;
                //CameraControls[i].RcvAudioDec = 0;

                //Start Live
                iChannel = 1;
                iBlockingMode = 0;
                iRet = CameraControls[0].PlayLive(iChannel, iBlockingMode);
                Logging("[Function] PlayLive(Start):" + iRet.ToString());

                if (iRet == 0)
                {
                    m_PlayStatus = 1;
                }
                else
                {
                    CameraControls[0].Close();
                    Logging("[Function] Close");
                }
            }
        }



        //---------------------------------------------------------------------
        // Function Name        : OnFtpStatusCB
        // Overview             : OnFtpStatusCB callback event
        //---------------------------------------------------------------------
        private void axipropsapiCtrl1_OnFtpStatusCB(object sender, AxIPROPSAPILib._IipropsapiCtrlEvents_OnFtpStatusCBEvent e)
        {
            //Output Logs
            Logging("[OnFtpStatusCB] status[" + e.status + "]");
        }
    }
}