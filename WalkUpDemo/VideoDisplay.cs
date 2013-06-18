using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace VideoDemo
{
    public partial class VideoDisplay : UserControl
    {
        public Boolean isLogo = false;

        //private List<string> eventImageList = new List<string>();
        private List<Image> eventImageImageList = new List<Image>();
        private NoxQuickId m_videoId = new NoxQuickId();
        private Random rand = new Random(DateTime.Now.Millisecond);
        private int m_fixedLogoIndex = -1;

        public String noVideoIDs = "";

        public string Host = "";
        public string Zone = "";

        public Boolean running = false;
        private Boolean m_run = false;
        private Boolean m_halt = false;
        public Boolean inView = false;
        public String id;
        public String name;
        public double weight = 0;
        public Int32 secondsPre = 5;
        public Int32 secondsPost = 10;

        public Boolean resizeAdjust = true;

        //private Boolean threadWait = false;

        int FPS = 5;
        int imageCount = 0;
        int thisImage = 0;

        Thread thisThread;
        
        public bool EnableNames = false;

        private Color m_bgColor = Color.Gray;

        //delegate void msgDelegate(String text);
        delegate void LoadImageDelegate(Image img);

        ///////////////////////////////////////////////////////////////
        public VideoDisplay()
        {
            InitializeComponent();
        }

        public void clear()
        {
            eventImageImageList.Clear();
        }

        ///////////////////////////////////////////////////////////////
        public void start(String vId, String vName)
        {
            m_halt = false;
            running = true;

            id = vId;
            name = vName;
            imageCount = 0;
            thisImage = 0;
            name = vName;

            eventImageImageList.Clear();

            System.Diagnostics.Debug.Print("Start: " + id);

            ////get event
            //try
            //{
            //    NoxHistoryItem priorityEvent = getNextHistoryEventId(id);
            //    setImageStream(priorityEvent);
            //}
            //catch (Exception e)
            //{
            //    String err = e.Message;
            //}

            m_run = true;

            //thread
            thisThread = new Thread(new ThreadStart(threadStub));
            thisThread.Start();
        }

        ///////////////////////////////////////////////////////////////
        public void start()
        {
            if (isLogo)
            {
                return;
            }

            m_halt = false;
            running = true;

            m_run = true;

            //thread
            thisThread = new Thread(new ThreadStart(threadStub));
            thisThread.Start();
        }

        ///////////////////////////////////////////////////////////////
        public void stop()
        {
            System.Diagnostics.Debug.Print("Stop: " + id);

            id = "";
            name = "";

            m_run = false;
        }

        ///////////////////////////////////////////////////////////////
        public void halt()
        {
            System.Diagnostics.Debug.Print("Halt: " + id);

            id = "";
            name = "";

            m_run = false;
            m_halt = true;

            //waitStop();
        }

        ///////////////////////////////////////////////////////////////
        public Boolean waitStop()
        {
            while (running)
            {
                Application.DoEvents();
                //String s = "1";
            }
            return true;
        }

        ///////////////////////////////////////////
        private void threadStub()
        {

            Boolean go = true;
            Boolean m_lastRun = false;
            int m_lastCount = 0;

            if(eventImageImageList.Count < 1)
            {
                //get event
                try
                {
                    NoxHistoryItem priorityEvent = getNextHistoryEventId(id);
                    setImageStream(priorityEvent);
                }
                catch (Exception e)
                {
                    String err = e.Message;
                }
            }

            while (go && !m_halt)
            {
                try
                {
                    //tick through images
                    if (imageCount < 1)
                    {
                        //no images so shut yourself down
                        if (!noVideoIDs.Contains(id))
                        {
                            noVideoIDs += "|" + id;
                        }
                        stop();
                        break;
                    }
                    thisImage++;
                    if (thisImage >= imageCount)
                    {
                        thisImage = 0;
                    }

                    if (m_lastRun && !m_halt)
                    {
                        LoadImage(fade(eventImageImageList[thisImage]));
                        //pbDisplay.Image = Lighter(eventImageImageList[thisImage]);
                    }
                    else if(!m_halt)
                    {
                        LoadImage(eventImageImageList[thisImage]);
                        //pbDisplay.Image = eventImageImageList[thisImage];
                    }

                    if (m_halt)
                    {
                        break;
                    }

                    Thread.Sleep(1000 / FPS);
                    Application.DoEvents();

                    if (!m_run)
                    {
                        m_lastRun = true;
                    }

                    if (m_lastRun)
                    {
                        m_lastCount++;
                        if (m_lastCount >= imageCount)
                        {
                            go = false;
                        }
                    }
                }
                catch (Exception)
                {
                    thisImage = 0;
                    imageCount = eventImageImageList.Count();
                }
            }

            //if you were halted display the next image as lighter
            if (m_halt)
            {
                try
                {
                    LoadImage(fade(eventImageImageList[thisImage]));
                }
                catch (Exception e)
                {
                    String err = e.Message;
                }
            }

            running = false;

            //System.Diagnostics.Debug.Print("THREAD DEAD: " + id);

        }


        ///////////////////////////////////////////////////////////////
        public void LoadImage(Image img)
        {
            if (isLogo)
            {
                return;
            }
            if (this.pbDisplay.InvokeRequired)
            {
                LoadImageDelegate d = new LoadImageDelegate(LoadImage);
                this.Invoke(d, new object[] { img });
            }
            else
            {
                pbDisplay.Image = img;
            }
            //this.BeginInvoke(new LoadImageDelegate(LoadImage), new object[] { img });
            //return;
            //}
        }

        ///////////////////////////////////////////////////////////////
        //initialize the image
        public void loadFirstImage()
        {
            if (isLogo || eventImageImageList.Count < 1)
            {
                return;
            }

            LoadImage(eventImageImageList[0]);
        }

        /////////////////////////////////////////////////////////////////
        //private void tmrVideoFrame_Tick(object sender, EventArgs e)
        //{
        //    if (!run)
        //    {
        //        return;
        //    }

        //    if (loadingImages)
        //        return;

        //    if (timerCheck)
        //        return;


        //    timerCheck = true;
        //    if (cooldown == 0)
        //    {
                
        //            if (pbDisplay.Tag == null || (int)pbDisplay.Tag >= eventImageImageList.Count)
        //            {
        //                //loadRandomBackgroundImage();
        //                //setCountdownImage();
        //                if (pbDisplay.Image != null && !lighter)
        //                    pbDisplay.Image = Lighter(pbDisplay.Image);

        //                m_videoId = new NoxQuickId(); //Reset the video
        //                //eventImageList = new List<string>();
        //                eventImageImageList = new List<Image>();
        //                //pbDisplay.Image = null;
        //                cooldown = 0;

        //            }
        //            else
        //            {
        //                if (smoothing < 6)
        //                {
        //                    smoothing++;
        //                    timerCheck = false;
        //                    return;
        //                }
        //                smoothing = 0;

        //                lighter = false;
        //                pbDisplay.Image = eventImageImageList[(int)pbDisplay.Tag];
        //                pbDisplay.Tag = (int)pbDisplay.Tag + 1;
        //            }

        //    }
        //    else
        //    {
        //        cooldown -= 1;
        //    }
        //    timerCheck = false;
        //}

        ///////////////////////////////////////////////////////////////
        public Color BackgroundColor
        {
            set
            {
                m_bgColor = value;
                pbDisplay.BackColor = m_bgColor;
                this.BackColor = m_bgColor;
            }
            get
            {
                return m_bgColor;
            }

        }

        ///////////////////////////////////////////////////////////////
        public int FixedLogoIndex
        {
            set
            {
                m_fixedLogoIndex = value;
                if (m_fixedLogoIndex == -1)
                {
                    //tmrVideoFrame.Enabled = true;
                    m_videoId = new NoxQuickId();
                }
                else
                {
                    //tmrVideoFrame.Enabled = false;
                    m_videoId = new NoxQuickId();
                    m_videoId.id = "1";
                }
                pbDisplay.Image = null;
                loadRandomBackgroundImage(m_fixedLogoIndex);
            }
        }

        /////////////////////////////////////////////////////////////////
        //public NoxQuickId VideoId
        //{
        //    get { return m_videoId; }
        //}

        /////////////////////////////////////////////////////////////////
        //private void getHistoryEvent(string id)
        //{
        //    string output = "";

        //    try
        //    {
        //        output = NoxHelper.getNoxData(Host, "getHistoryEvent", false, new NoxApiParameter("zone", Zone));
        //    }
        //    catch (Exception)
        //    {
        //        output = "";
        //    }
        //}

        //private int cooldown = 0;
        //private int smoothing = 0;
        //private int smoothingFPS = 6;

        /////////////////////////////////////////////////////////////////
        //private void tmrVideoFrame_TickXXXX(object sender, EventArgs e)
        //{
        //    if (loadingImages)
        //        return;

        //    if (timerCheck)
        //        return;
            

        //    timerCheck = true;
        //    if (cooldown == 0)
        //    {
        //        if (m_videoId.id == "")
        //        {
        //            //this is a new video so load one up
        //            if (m_onDeckId.id != "")
        //            {
        //                NoxHistoryItem priorityEvent = getNextHistoryEventId(m_onDeckId.id);
        //                m_videoId = m_onDeckId;
        //                m_onDeckId = new NoxQuickId();
        //                setImageStream(priorityEvent);
        //                pbDisplay.Tag = 0;
        //            }

        //        }
        //        else
        //        {
        //            if (pbDisplay.Tag == null || (int)pbDisplay.Tag >= eventImageImageList.Count)
        //            {
        //                //loadRandomBackgroundImage();
        //                //setCountdownImage();
        //                if (pbDisplay.Image != null && !lighter)
        //                    pbDisplay.Image = Lighter(pbDisplay.Image);

        //                m_videoId = new NoxQuickId(); //Reset the video
        //                //eventImageList = new List<string>();
        //                eventImageImageList = new List<Image>();
        //                //pbDisplay.Image = null;
        //                cooldown = 0;

        //            }
        //            else
        //            {
        //                if (smoothing < 6)
        //                {
        //                    smoothing++;
        //                    timerCheck = false;
        //                    return;
        //                }
        //                smoothing = 0;

        //                lighter = false;
        //                pbDisplay.Image = eventImageImageList[(int)pbDisplay.Tag];
        //                pbDisplay.Tag = (int)pbDisplay.Tag + 1;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        cooldown -= 1;
        //    }
        //    timerCheck = false;
        //}

        ///////////////////////////////////////////////////////////////
        private Image fade(Image src)
        {
            Image ret = (Image)src.Clone();

            Graphics graphics = Graphics.FromImage(ret);
            try
            {
                Pen pLight = new Pen(Color.FromArgb(128, this.BackColor), ret.Width * 2);
                graphics.DrawLine(pLight, -1, -1, ret.Width, ret.Height);
                graphics.Save();
                graphics.Dispose();
            }
            catch (Exception)
            {
                //String err = "xx";
            }
            return ret;
        }

        ///////////////////////////////////////////////////////////////
        private void setCountdownImage()
        {
            pbDisplay.Image = Image.FromFile(Environment.CurrentDirectory + "\\countdown2.gif");
        }

        ///////////////////////////////////////////////////////////////
        private void loadRandomBackgroundImage(int specificImageIndex = -1)
        {
            try
            {
                string[] files = Directory.GetFiles(Environment.CurrentDirectory + "\\logos\\");
                if (files.Length > 0)
                {
                    int index = specificImageIndex;
                    if (specificImageIndex == -1)
                        index = rand.Next(0, files.Length);

                    pbDisplay.BackgroundImage = Image.FromFile(files[index]);
                    
                }
            }
            catch (Exception)
            {
                //String s = "There was a problem loading your logo. Please make sure you have a 'logos' directory in the same place you are running the demo.";
                //MessageBox.Show(s);
            }

        }

        ///////////////////////////////////////////////////////////////
        public void setImageStream(NoxHistoryItem thisEvent)
        {
            string cameras = "";
            string[] splitCameras;

            //Get a list of valid cameras
            try
            {
                cameras = NoxHelper.getNoxData(Host, "cameraList", false, new NoxApiParameter("zone", thisEvent.zone));
                splitCameras = cameras.Split(Environment.NewLine.ToCharArray());
            }
            catch (Exception)
            {
                return;
            }
            List<string> cameraNames = new List<string>();
            foreach (string cameraLine in splitCameras)
            {
                if (cameraLine != "")
                {
                    if (cameraLine.StartsWith("name|"))
                    {
                        cameraNames.Add(cameraLine.Substring(cameraLine.IndexOf("|") + 1));
                        //break;
                    }
                }
            }

            //For every camera, try to find images until you find them. They are in there somewhere... trust me.
            foreach (string camera in cameraNames)
            {
                string images = "";

                try
                {
                    images = NoxHelper.getNoxData(Host, "videoList", false, new NoxApiParameter("camera", camera), new NoxApiParameter("beginDate", thisEvent.BeginTime.ToString()), new NoxApiParameter("endDate", thisEvent.EndTime.ToString()));
                }
                catch (Exception e)
                {
                    String ret = e.Message;
                    return;
                }
                    
                if (images != "")
                {
                    //We found them!
                    //Thread loadImages = new Thread(new ParameterizedThreadStart(loadImagesFromStream));
                    loadImagesFromStream(images);
                    m_videoId.zone = thisEvent.zone;
                    m_videoId.time = thisEvent.time;
                    break;
                }
            }
                //tmrVideoFrame.Interval = 1;
            
            
        }

        //private bool loadingImages = false;

        ///////////////////////////////////////////////////////////////
        private void loadImagesFromStream(object noxImageData)
        {
            //loadingImages = true;
            string[] splitImages = ((string)noxImageData).Split(Environment.NewLine.ToCharArray());
            DateTime imageTimer = DateTime.Now;
            string imageUri = "";
            Int32 tCount = 0;

            DateTime dtBegin = new DateTime();
            DateTime dtEnd = new DateTime();
            dtBegin = Convert.ToDateTime("1/1/2030");
            dtEnd = Convert.ToDateTime("1/1/1990");
            DateTime tDateTime = new DateTime();

            eventImageImageList.Clear();
            foreach (string imageLine in splitImages)
            {
                if (imageLine != "")
                {
                    tCount++;
                    tDateTime = getDateTimeFromFilename(imageLine);
                    if (tDateTime < dtBegin)
                    {
                        dtBegin = tDateTime;
                    }
                    if (tDateTime > dtEnd)
                    {
                        dtEnd = tDateTime;
                    }
                    imageUri = "http://" + imageLine.Substring(imageLine.IndexOf("|") +1 ).Replace("\\", "/");
                    try
                    {
                        Image thisImage = Image.FromStream(System.Net.HttpWebRequest.Create(imageUri).GetResponse().GetResponseStream());
                        eventImageImageList.Add(thisImage);
                        imageCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            //FPS
            try
            {
                TimeSpan ts = dtEnd.Subtract(dtBegin);
                Double seconds = ts.TotalSeconds;
                FPS = Convert.ToInt32(tCount / seconds);
            }
            catch (Exception)
            {
                FPS = 5;
            }
            //smoothingFPS = (3000 / eventImageImageList.Count) / 100;
            //FPS = (3000 / eventImageImageList.Count) / 100;

            //loadingImages = false;
        }

        ///////////////////////////////////////////////////////////////
        private DateTime getDateTimeFromFilename(String fileName)
        {
            String tS = "";
            String tS2 = "";
            try
            {
                tS = fileName.Substring(fileName.LastIndexOf('\\') + 1, 19);
                tS2 = tS.Substring(5, 2) + "/" + tS.Substring(8, 2) + "/" + tS.Substring(0, 4) + " ";
                tS2 += tS.Substring(11, 2) + ":" + tS.Substring(14, 2) + ":" + tS.Substring(17, 2);
            }
            catch (Exception)
            {
                tS2 = "1/1/2000";
            }

            return Convert.ToDateTime(tS2);
        }

        ///////////////////////////////////////////////////////////////
        private NoxHistoryItem getNextHistoryEventId(string id)
        {
            string history = "";
            string[] splitHistory;
            NoxHistoryItem priorityEvent = new NoxHistoryItem();
            NoxHistoryItem thisEvent = new NoxHistoryItem();

            try
            {
                history = NoxHelper.getNoxData(Host, "historyList", false, new NoxApiParameter("data", id), new NoxApiParameter("max", "30"), new NoxApiParameter("eventType", "present"), new NoxApiParameter("videoExistsOnly","true"));
                splitHistory = history.Split(Environment.NewLine.ToCharArray());
            }
            catch (Exception)
            {
                return thisEvent;
            }

            List<NoxHistoryItem> eventList = new List<NoxHistoryItem>();
            
            foreach (string historyLine in splitHistory)
            {
                if (historyLine != "")
                {
                    string value = historyLine.Substring(historyLine.IndexOf("|") + 1);
                    if (historyLine.StartsWith("id|"))
                    {
                        if (thisEvent.eventId != "")
                        {
                            if(thisEvent.thumbnail != "none" && thisEvent.thumbnail != "unknown" && thisEvent.thumbnail != "")
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

            //add to event list
            if (thisEvent.eventId != "" && thisEvent.thumbnail != "none" && thisEvent.thumbnail != "unknown" && thisEvent.thumbnail != "")
            {
                eventList.Add(thisEvent);
            }

            if (eventList.Count > 0)
                priorityEvent = eventList[0];
            foreach (NoxHistoryItem historyEvent in eventList)
            {
                if (historyEvent.zone != Zone)
                {
                    priorityEvent = historyEvent;
                    break;
                }

            }

            priorityEvent.secondsPre = secondsPre;
            priorityEvent.secondsPost = secondsPost;

            return priorityEvent;
        }

        ///////////////////////////////////////////////////////////////
        private void VideoDisplay_Load(object sender, EventArgs e)
        {
            
        }

        ///////////////////////////////////////////////////////////////
        private void pbDisplay_Paint(object sender, PaintEventArgs e)
        {
            if (EnableNames)
            {
                using (Font myFont = new Font("Arial", 30, FontStyle.Bold))
                {
                    e.Graphics.DrawString(name, myFont, Brushes.White, new Point(2, 2));
                }
            }

            using (Font secondFont = new Font("Arial", 13))
            {
                if (m_videoId.zone != "")
                    e.Graphics.DrawString(m_videoId.zone + " at " + m_videoId.time, secondFont, Brushes.White, new Point(2, pbDisplay.Height - 25));
            }
        }

        ///////////////////////////////////////////////////////////////
        private void VideoDisplay_Resize(object sender, EventArgs e)
        {
            //if (!resizeAdjust)
            //{
            //    return;
            //}
            int padding = 20;
            pbDisplay.Left = padding; //this.Left + padding;
            pbDisplay.Top = padding; //this.Top + padding;
            pbDisplay.Width = this.Width - (padding*2);
            pbDisplay.Height = this.Height - (padding * 2);

        }
    }

    ///////////////////////////////////////////////////////////////
    public class NoxQuickId
    {
        public string id = "";
        public string name = "";
        public string zone = "";
        public string time = "";
    }

    ///////////////////////////////////////////////////////////////
    public class NoxHistoryItem
    {
        public string eventId = "";
        public string name = "";
        public string zone = "";
        public string time = "";
        public string thumbnail = "";
        public Int32 secondsPre = 5;
        public Int32 secondsPost = 10;


        public DateTime BeginTime
        {
            get
            {
                DateTime begin = new DateTime();
                if (time != "")
                {
                    begin = DateTime.Parse(time);
                    begin = begin.Subtract(new TimeSpan(0, 0, secondsPre));
                }
                return begin;
            }
        }

        public DateTime EndTime
        {
            get
            {
                DateTime end = new DateTime();
                if (time != "")
                {
                    end = DateTime.Parse(time);
                    end = end.Add(new TimeSpan(0, 0, secondsPost));
                }
                return end;
            }
        }
    }
}
