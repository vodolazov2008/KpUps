using Scada.Comm.Channels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using ApcupsdLib;

namespace Scada.Comm.Devices
{
    public class KpUpsLogic : KPLogic
    {
        public IPEndPoint endPoint;
        public int DefaultPort = 3551;

        public KpUpsLogic(int number) : base(number)
        {
            CanSendCmd = false;
            ConnRequired = false;
            WorkState = WorkStates.Normal;
            List<TagGroup> tagGroups = new List<TagGroup>();
            TagGroup tagGroup = new TagGroup("UPS Statistics");
            tagGroup.KPTags.Add(new KPTag(0, "Status"));
            tagGroup.KPTags.Add(new KPTag(1, "InputLineVoltage"));
            tagGroup.KPTags.Add(new KPTag(2, "BatteryActualVoltage"));
            tagGroup.KPTags.Add(new KPTag(3, "BatteryCapacity"));
            tagGroup.KPTags.Add(new KPTag(4, "BatteryRunTimeRemaining"));
            tagGroup.KPTags.Add(new KPTag(5, "OutputLoad"));
            tagGroups.Add(tagGroup);
            InitKPTags(tagGroups);
        }
        public override void Session()
        {
            base.Session();
                WriteToLog(CommPhrases.KPCaption);
                CommUtils.ExtractAddrAndPort(CallNum, DefaultPort, out IPAddress addr, out int port);
                endPoint = new IPEndPoint(addr, port);
                var upsStatus = new ApcupsdClient(endPoint.Address.ToString(), endPoint.Port);
                var upsStatusResponse = upsStatus.GetStatus();
            //WriteToLog(string.Format("", CommUtils.GetNowDT()));
            WriteToLog(string.Format("Connecting to: ", endPoint.Address.ToString(), endPoint.Port.ToString()));
            if (upsStatusResponse != null)
                    {
                            WriteToLog(CommPhrases.ResponseOK);
                double stat = (double)upsStatusResponse.Status;
                SetCurData(0, stat, 13);
                double linev = Convert.ToDouble(upsStatusResponse.LineV);
                SetCurData(1, linev, 13);
                double battv = Convert.ToDouble(upsStatusResponse.BattV);
                SetCurData(2, battv, 13);
                double bcharge = Convert.ToDouble(upsStatusResponse.BCharge);
                SetCurData(3, bcharge, 13);
                double timeleft = Convert.ToDouble(upsStatusResponse.TimeLeft);
                SetCurData(4, timeleft, 13);
                double outl = Convert.ToDouble(upsStatusResponse.LoadPct);
                SetCurData(5, outl, 13);
                //----------------------------
                kpStats.ReqCnt++;
                    }
                    else
                    {
                SetCurData(0, 0, 0);
                SetCurData(1, 0, 0);
                SetCurData(2, 0, 0);
                SetCurData(3, 0, 0);
                SetCurData(4, 0, 0);
                SetCurData(5, 0, 0);
                kpStats.ReqErrCnt++;
                            WriteToLog(CommPhrases.ResponseError);
                            WorkState = WorkStates.Error;
                    }
                FinishRequest();
                Thread.Sleep(ReqParams.Delay);
                CalcSessStats();
        }
    }


}