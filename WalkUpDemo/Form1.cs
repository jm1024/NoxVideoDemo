using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;


namespace VideoDemo
{
    public partial class frmMain : Form
    {
        public const String settingsFile = "settings.txt";

        private string m_host = "";
        private string m_zone = "";
        private string m_touchZone = "";
        private bool m_enableNames = false;
        private Int32 m_secondsPre = 5;
        private Int32 m_secondsPost = 10;

        private String m_touchId = "";

        private String orientation = "l";

        private Int32 m_mode = MODE_MAIN;
        private const Int32 MODE_MAIN = 0;
        private const Int32 MODE_TOUCH = 1;

        //private int animateCounter = 0;
        private bool timerCheck = false;

        private int videoCount = 0;

        //private StateItem m_onDeckId = new StateItem();

        public frmMain()
        {
            InitializeComponent();
        }

        ///////////////////////////////////////////////////////////////
        private void frmMain_Load(object sender, EventArgs e)
        {
            settingsForm sf = new settingsForm();
            sf.ShowDialog();

            if (sf.DialogResult == DialogResult.OK)
            {
                m_host = sf.Host;
                m_zone = sf.VideoZone;
                m_enableNames = sf.EnableNames;
                m_touchZone = sf.WalkUpZone;
                try
                {
                    m_secondsPre = sf.secondsPre;
                    m_secondsPost = sf.secondsPost;
                }
                catch (Exception) { }

                tmrState.Enabled = true;
                saveSettings();
            }
            else
            {
                this.Close();
            }


            orientation = "l";
            videoCount = 9;
            if (this.Width < this.Height)
            {
                orientation = "p";
                videoCount = 8;
            }

            for (int i = 0; i < videoCount; i++)
            {
                VideoDisplay vdObj = new VideoDisplay();

                this.Controls.Add(vdObj);

            }

            this.BackColor = Color.Black; //Color.LightGray;

            setupFormMain();
        }

        /////////////////////////////////////////////////
        public void saveSettings()
        {
            String s = "host=" + m_host + System.Environment.NewLine;
            s += "zone=" + m_zone + System.Environment.NewLine;
            s += "touchZone=" + m_touchZone + System.Environment.NewLine;
            s += "enableNames=" + m_enableNames.ToString() + System.Environment.NewLine;
            s += "secondsPre=" + m_secondsPre.ToString() + System.Environment.NewLine;
            s += "secondsPost=" + m_secondsPost.ToString() + System.Environment.NewLine;

            try
            {
                System.IO.File.WriteAllText(settingsFile, s);
            }
            catch (Exception e)
            {
                String err = e.Message;
            }
        }

        /////////////////////////////////////////////////
        private void shutdown()
        {
            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {
                vdObj.halt();
            }
        }

        /////////////////////////////////////////////////
        private void msg(String s)
        {
            //frmMessage.Left = 0;
            //frmMessage.Top = 0;
            //frmMessage.Width = this.Width;
            //frmMessage.Height = this.Height;
            //frmMessage.BringToFront();
            //frmMessage.Visible = true;

            lblMessage.BringToFront();
            lblMessage.Text = s;
            lblMessage.Top = (this.Height / 2) - (lblMessage.Height /2);
            lblMessage.Left = (this.Width / 2) - (lblMessage.Width /2);
            lblMessage.Visible = true;
            Application.DoEvents();
            this.Refresh();
        }

        /////////////////////////////////////////////////
        private void msgOff()
        {
            lblMessage.Visible = false;

            //frmMessage.Visible = false;
        }
            
        /////////////////////////////////////////////////
        private void setupFormMain()
        {
            m_mode = MODE_MAIN;

            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {
                vdObj.Visible = false;
            }

            msg("Switching");

            int rowCount = 0;
            int colCount = 0;
            double height = 0;
            double width = 0;

            //landscape
            if (orientation == "l")
            {
                videoCount = 9;
                rowCount = 3; //(int)Math.Sqrt(NumImages);
                colCount = 3; //(int)Math.Sqrt(NumImages);
                height = this.Height / rowCount;
                width = height * 1.33333333;
            }

            //portrait
            if (orientation == "p")
            {
                videoCount = 8;
                rowCount = 4;//(int)Math.Sqrt(NumImages);
                colCount = 2; //(int)Math.Sqrt(NumImages);
                width = this.Width / colCount;
                height = width * 0.75;
            }
            
            int topAdjust = (this.Height - ((int)height * rowCount)) / 2;
            int leftAdjust = (this.Width - ((int)width * colCount)) / 2;
            int left = leftAdjust;
            int top = topAdjust;  // was 0

            int thisImage = 0;
            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {

                vdObj.halt();
                //vdObj.waitStop();
                vdObj.isLogo = false;
                vdObj.EnableNames = m_enableNames;
                vdObj.BackgroundColor = this.BackColor;
                vdObj.Height = (int)Math.Round(height);
                vdObj.Width = (int)Math.Round(width);
                vdObj.Host = m_host;
                vdObj.Zone = m_zone;
                vdObj.Left = left;
                vdObj.Top = top;
                
                if ((thisImage + 1) % colCount != 0)
                    left += vdObj.Width;
                else
                {
                    top += vdObj.Height;
                    left = leftAdjust;
                }

                if (thisImage == 4 && orientation == "l")
                {
                    vdObj.clear();
                    vdObj.isLogo = true;
                    vdObj.FixedLogoIndex = 0;
                }

                thisImage++;
            }

            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {
                vdObj.Visible = true;
            }

            msgOff();
        }

        /////////////////////////////////////////////////
        private void setupFormTouch()
        {
            m_mode = MODE_TOUCH;

            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {
                vdObj.Visible = false;
            }

            msg("Switching");

            int rowCount = 0;
            int colCount = 0;
            double height = 0;
            double width = 0;

            videoCount = 6;
            rowCount = 3; //(int)Math.Sqrt(NumImages);
            colCount = 3; //(int)Math.Sqrt(NumImages);
            height = this.Height / rowCount;
            width = height * 1.33333333;

            int topAdjust = (this.Height - ((int)height * rowCount)) / 2;
            int leftAdjust = (this.Width - ((int)width * colCount)) / 2;
            int left = leftAdjust;
            int top = topAdjust;  // was 0

            int thisImage = 0;
            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {
                vdObj.halt();
                //vdObj.waitStop();
                vdObj.isLogo = false;
                vdObj.EnableNames = m_enableNames;
                vdObj.BackgroundColor = this.BackColor;
                vdObj.Host = m_host;
                vdObj.Zone = m_zone;

                vdObj.Left = left;
                vdObj.Top = top;

                vdObj.Height = (int)Math.Round(height);
                vdObj.Width = (int)Math.Round(width);

                //sizing and placement
                if (thisImage == 0) //first image is double size
                {
                    vdObj.Height = (int)Math.Round(height * 2);
                    vdObj.Width = (int)Math.Round(width * 2);
                }

                if (thisImage == 1)
                {
                    vdObj.Left += (int)Math.Round(width * 2);
                }

                if (thisImage == 2)
                {
                    vdObj.Top += (int)Math.Round(height);
                    vdObj.Left += (int)Math.Round(width * 2);
                }

                if (thisImage == 3)
                {
                    vdObj.Top += (int)Math.Round(height * 2);
                }
                if (thisImage == 4)
                {
                    vdObj.Top += (int)Math.Round(height * 2);
                    vdObj.Left += (int)Math.Round(width);
                }
                if (thisImage == 5)
                {
                    vdObj.Top += (int)Math.Round(height * 2);
                    vdObj.Left += (int)Math.Round(width * 2);
                    //vdObj.clear();
                    //vdObj.isLogo = true;
                    //vdObj.FixedLogoIndex = 0;
                    
                }

                thisImage++;
            }

            foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
            {
                vdObj.Visible = true;
            }
            msgOff();
        }
        
        ///////////////////////////////////////////////////////////////
        private void tmrState_Tick(object sender, EventArgs e)
        {
            if (timerCheck)
            {
                return;
            }
            
            timerCheck = true;

            try
            {
                if (!handleTouchZone())
                {
                    handleMainZone();
                }

                this.Refresh();
                Application.DoEvents();
            }
            catch (Exception er)
            {
                String err = er.Message;
            }

            timerCheck = false;
        }

        

        ///////////////////////////////////////////////////////////////
        private void handleMainZone()
        {
            // switch setup if needed
            if (m_mode == MODE_TOUCH)
            {
                setupFormMain();
            }

            List<StateItem> stateList;

            try
            {
                stateList = loadStateData(m_host, m_zone);

                //mark everything as out of view for now
                foreach (VideoDisplay videoControl in this.Controls.OfType<VideoDisplay>())
                {
                    videoControl.inView = false;
                }

                //add whatever you can/need
                foreach (StateItem item in stateList)
                {

                    bool found = false;
                    foreach (VideoDisplay videoObj in this.Controls.OfType<VideoDisplay>())
                    {
                        if (videoObj.id == item.id)
                        {
                            videoObj.weight = Convert.ToDouble(item.weight);
                            videoObj.inView = true;
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        addVideoItem(item.id, item.name, Convert.ToDouble(item.weight));
                    }
                }

                //deactivate anything that's not inView
                foreach (VideoDisplay videoControl in this.Controls.OfType<VideoDisplay>())
                {
                    if (!videoControl.inView && videoControl.running)
                    {
                        videoControl.stop();
                    }
                }

            }
            catch (Exception er)
            {
                String err = er.Message;
            }

        }

        ///////////////////////////////////////////////////////////////
        private void addVideoItem(String id, String name, double weight)
        {
            Boolean bAdded = false;

            //look for an empty slot to add it to
            foreach (VideoDisplay videoControl in this.Controls.OfType<VideoDisplay>())
            {
                if (videoControl.running == false && !videoControl.isLogo)
                {
                    //does this tag have any video
                    //if (!tagHasVideo(id))
                    //{
                    //    return;
                    //}

                    videoControl.EnableNames = m_enableNames;
                    videoControl.inView = true;
                    videoControl.weight = weight;
                    videoControl.secondsPre = m_secondsPre;
                    videoControl.secondsPost = m_secondsPost;
                    videoControl.start(id, name);
                    bAdded = true;
                    break;
                }
            }

            //if there wasn't a slot is there anything lower?
            if (!bAdded)
            {
                //find the lowest weighted thing
                Double lowestWeight = 9999;
                foreach (VideoDisplay videoControl in this.Controls.OfType<VideoDisplay>())
                {
                    if (videoControl.weight < lowestWeight && !videoControl.isLogo)
                    {
                        lowestWeight = videoControl.weight;
                    }
                }

                //if there's something lower than this
                if (lowestWeight < weight)
                {
                    foreach (VideoDisplay videoControl in this.Controls.OfType<VideoDisplay>())
                    {
                        if (videoControl.weight == lowestWeight + .5 && !videoControl.isLogo)
                        {
                            //does this tag have any video
                            //if (!tagHasVideo(id))
                            //{
                            //    return;
                            //}

                            videoControl.EnableNames = m_enableNames;
                            videoControl.inView = true;
                            videoControl.weight = weight;
                            videoControl.start(id, name);
                            bAdded = true;
                            break;
                        }
                    }
                }
            }
        }

        ///////////////////////////////////////////////////////////////
        private Boolean tagHasVideo(String id)
        {
            string history = "";

            try
            {
                history = NoxHelper.getNoxData(m_host, "historyList", false, new NoxApiParameter("data", id), new NoxApiParameter("max", "1"), new NoxApiParameter("eventType", "present"), new NoxApiParameter("videoExistsOnly", "true"));
                if (history != "")
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        ///////////////////////////////////////////////////////////////
        private Boolean handleTouchZone()
        {

            List<StateItem> stateList;
            List<NoxHistoryItem> eventList = new List<NoxHistoryItem>();
            String thisId = "";
            Double thisWeight = 0;
            Double fattestWeight = 0;

            try
            {
                //see if there's anything in the state list for the touch zone
                stateList = loadStateData(m_host, m_touchZone);
                if (stateList.Count < 1)
                {
                    m_touchId = "";
                    return false;
                }

                //get the fattest id
                foreach (StateItem si in stateList)
                {
                    thisWeight = Convert.ToDouble(si.weight);

                    //this is the current id
                    if (si.id == m_touchId)
                    {
                        if (thisWeight + 1 > fattestWeight)
                        {
                            fattestWeight = thisWeight;
                            thisId = si.id;
                        }
                    }
                    else if ( thisWeight > fattestWeight + 1 || fattestWeight == 0)
                    {
                        fattestWeight = thisWeight;
                        thisId = si.id;
                    }
                }



                //is it the last one you had? - don't keep reloading
                if (thisId == m_touchId)
                {
                    return true;
                }

                m_touchId = thisId;

                //get the event list
                eventList = getHistory(thisId);
                if (eventList.Count < 1)
                {
                    return false;
                }

                // switch setup if needed
                if (m_mode == MODE_MAIN)
                {
                    setupFormTouch();
                }

                Int32 iEvent = 0;
                foreach (VideoDisplay vdObj in this.Controls.OfType<VideoDisplay>())
                {
                    if (iEvent >= eventList.Count)
                    {
                        break;
                    }
                    if (!vdObj.isLogo)
                    {
                        //vdObj.name = "";
                        if (iEvent == 0)
                        {
                            vdObj.EnableNames = true;
                            vdObj.name = eventList[iEvent].name;
                        }
                        else
                        {
                            vdObj.EnableNames = false;
                        }
                        eventList[iEvent].secondsPre = m_secondsPre;
                        eventList[iEvent].secondsPost = m_secondsPost;
                        vdObj.setImageStream(eventList[iEvent]);
                        iEvent++;
                        vdObj.start();
                    }
                }
            }
            catch (Exception er)
            {
                String err = er.Message;
            }

            return true;
        }

        ///////////////////////////////////////////////////////////////
        private List<NoxHistoryItem> getHistory(string id)
        {
            string history = "";
            string[] splitHistory;

            List<NoxHistoryItem> eventList = new List<NoxHistoryItem>();
            List<NoxHistoryItem> bList = new List<NoxHistoryItem>();

            try
            {
                history = NoxHelper.getNoxData(m_host, "historyList", false, new NoxApiParameter("data", id), new NoxApiParameter("max", "30"), new NoxApiParameter("eventType", "present"), new NoxApiParameter("videoExistsOnly", "true"));
                splitHistory = history.Split(Environment.NewLine.ToCharArray());
            }
            catch (Exception)
            {
                return eventList;
            }

            NoxHistoryItem thisEvent = new NoxHistoryItem();

            foreach (string historyLine in splitHistory)
            {
                if (historyLine != "")
                {

                    string value = historyLine.Substring(historyLine.IndexOf("|") + 1);
                    if (historyLine.StartsWith("id|"))
                    {
                        if (thisEvent.eventId != "")
                        {
                            if (thisEvent.thumbnail != "none" && thisEvent.thumbnail != "unknown" && thisEvent.thumbnail != "")
                            {
                                eventList.Add(thisEvent);
                            }
                            thisEvent = new NoxHistoryItem();
                        }
                        thisEvent.eventId = value;
                    }
                    else if (historyLine.StartsWith("zone|"))
                    {
                        thisEvent.zone = value;
                    }
                    else if (historyLine.StartsWith("dataName|"))
                    {
                        thisEvent.name = value;
                    }
                    else if (historyLine.StartsWith("date|"))
                    {
                        thisEvent.time = value;
                    }
                    else if (historyLine.StartsWith("thumbnail|"))
                    {
                        thisEvent.thumbnail = value;
                    }
                }
            }

            //add last event to event list
            if (thisEvent.eventId != "" && thisEvent.thumbnail != "none" && thisEvent.thumbnail != "unknown" && thisEvent.thumbnail != "")
            {
                if (thisEvent.zone != m_touchZone)
                {
                    eventList.Add(thisEvent);
                }
                else
                {
                    bList.Add(thisEvent);
                }
            }

            //add the bList stuff
            foreach (NoxHistoryItem bI in bList)
            {
                eventList.Add(bI);
            }

            return eventList;
        }

        /////////////////////////////////////////////////////////////////
        //private void tmrAnimate_Tick(object sender, EventArgs e)
        //{
        //    this.Controls[8].Width += 1;
        //    this.Controls[8].Height += 1;
        //    if (animateCounter > 99)
        //        tmrAnimate.Enabled = false;
        //    animateCounter += 1;
        //}

        ///////////////////////////////////////////////////////////////
        private List<StateItem> loadStateData(string host, string zone)
        {
            List<StateItem> stateList = new List<StateItem>();

            string state = "";
            string[] stateSplit;

            try
            {
                state = NoxHelper.getNoxData(host, "stateList", false, new NoxApiParameter("zone", zone));
                stateSplit = state.Split(Environment.NewLine.ToCharArray());
            }
            catch (Exception)
            {
                return stateList;
            }
            StateItem thisItem = new StateItem();
            foreach (string stateLine in stateSplit)
            {
                if (stateLine != "")
                {
                    if (stateLine.StartsWith("id|"))
                    {
                        if (thisItem.id != null && thisItem.name != null && thisItem.weight != null && (thisItem.id != thisItem.name) && (thisItem.name != "")) //only load valid items
                        {
                            if (thisItem.state == "present")
                                stateList.Add(thisItem);
                            thisItem = new StateItem();
                            thisItem.id = stateLine.Substring(stateLine.IndexOf("|") + 1).Trim();
                        }
                        else
                        {
                            thisItem.id = stateLine.Substring(stateLine.IndexOf("|") + 1).Trim();
                        }
                    }
                    else if (stateLine.StartsWith("weight|"))
                    {
                        thisItem.weight = stateLine.Substring(stateLine.IndexOf("|") + 1).Trim();
                    }
                    else if (stateLine.StartsWith("idName|"))
                    {
                        thisItem.name = stateLine.Substring(stateLine.IndexOf("|") + 1).Trim();
                    }
                    else if (stateLine.StartsWith("state|"))
                    {
                        thisItem.state = stateLine.Substring(stateLine.IndexOf("|") + 1).Trim();
                    }

                }
                

            }

            //last one
            if (thisItem.id != null && thisItem.name != null && thisItem.weight != null && (thisItem.id != thisItem.name) && (thisItem.name != "")) //only load valid items
            {
                if (thisItem.state == "present")
                    stateList.Add(thisItem);
                thisItem = new StateItem();
            }

            return stateList;
        }

        ///////////////////////////////////////////////////////////////
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            shutdown();
        }
    }

    ///////////////////////////////////////////////////////////////
    public class StateItem
    {
        public string id = "";
        public string weight = "";
        public string name = "";
        public string state = "";
    }

    
}
