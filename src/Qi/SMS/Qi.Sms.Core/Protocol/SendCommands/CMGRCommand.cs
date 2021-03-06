﻿using System;
using Qi.Sms.Protocol.Encodes;

namespace Qi.Sms.Protocol.SendCommands
{
    public class CmgrCommand : AtCommand
    {
        public override string Command
        {
            get { return "CMGR"; }
        }

        public int MessageIndex
        {
            set
            {
                if (Arguments.Count == 0)
                    Arguments.Add(value.ToString());
                else
                {
                    Arguments[0] = value.ToString();
                }
            }
        }

        public string Content { get; set; }
        public string SendMobile { get; set; }

        /// <summary>
        ///短信服务中心
        /// </summary>
        public string SCAAddr { get; set; }

        public DateTime ReceiveTime { get; set; }

        public override bool Init(string content)
        {
            var result = base.Init(content);
            if (Success)
            {
                if (!content.Contains("CMGR:"))
                    return false;

                var a = new SmsInfo();
                int pos = content.IndexOf("CMGR:");
                pos = content.IndexOf("\r\n", pos) + 2;
                int pos2 = content.IndexOf("\r\n", pos);
                if (pos2 == -1)
                {
                    pos2 = content.Length;
                }
                string smsContent = content.Substring(pos, pos2 - pos);
                SmsInfo ra = a.DecodingSMS(smsContent);
                Content = ra.UD.Replace("\\0", "");
                SendMobile = ra.OAAddr;
                SCAAddr = ra.SCAAddr;
                ReceiveTime = DateTime.Parse(ra.SCTS);
            }

            return result;
        }
    }
}