using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using HiStaff.Domain;

namespace HiStaff.Util
{
    public class SXml
    {
        private const string XMLTEMPLATE_TAG = "XMLTemplateList";
        private const string TEMPLATE_TAG = "Template";
        private const string ID_ATR = "Id";
        private const string ID_SUBJECT = "Subject";

        public static string ReadEmailTemplate(string urlSource, string idTemplate, out string eSubject)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string rtnValue = string.Empty;
            eSubject = string.Empty;

            if (!File.Exists(urlSource))
                return rtnValue;

            xmlDoc.Load(urlSource);

            // Read Report Note by Id
            XmlNode rptNode = xmlDoc.SelectSingleNode(XMLTEMPLATE_TAG + "/" +
                                                   TEMPLATE_TAG + "[@" + ID_ATR + "='" + idTemplate + "']");
            if (rptNode != null)
            {
                rtnValue = rptNode.InnerText;
                eSubject = rptNode.Attributes[ID_SUBJECT].Value;
            }

            return rtnValue;
        }
        private static void SetInOut(CO_SWIPE_DATA item, DateTime outval)
        {
            TimeSpan Limit = new TimeSpan(0, 30, 0);

            if (!item.VALIN1.HasValue)
                item.VALIN1 = outval;
            else if (!item.VALOUT1.HasValue)
            {
                if (outval - item.VALIN1.Value > Limit)
                    item.VALOUT1 = outval;
            }
            else if (!item.VALIN2.HasValue)
            {
                if (outval - item.VALOUT1.Value > Limit)
                    item.VALIN2 = outval;
            }
            else if (!item.VALOUT2.HasValue)
            {
                if (outval - item.VALIN2.Value > Limit)
                    item.VALOUT2 = outval;
            }
            else if (!item.VALIN3.HasValue)
            {
                if (outval - item.VALOUT2.Value > Limit)
                    item.VALIN3 = outval;
            }
            else if (!item.VALOUT3.HasValue)
            {
                if (outval - item.VALIN3.Value > Limit)
                    item.VALOUT3 = outval;
            }
            else if (!item.VALIN4.HasValue)
            {
                if (outval - item.VALOUT3.Value > Limit)
                    item.VALIN4 = outval;
            }
            else
            {
                if (outval - item.VALIN4.Value > Limit)
                {
                    item.VALOUT4 = outval;
                }
            }
        }
        public static //List<CO_SWIPE_DATA> 
            void ReadDataInOut(string urlSource,
                               DateTime workingDay,
                                                    List<CO_SWIPE_DATA> data,
                                                    System.ComponentModel.BackgroundWorker bgWorker,
                                                    int currentFile = 1,
                                                    int numberFile = 1,
                                                    string strRoot = "ROOT",
                                                    string strID = "MãNhânViên",
                                                    string strDate = "Ngày",
                                                    string strIN = "GVào",
                                                    string strIOUT = "GRa",
                                                    string strFormat = "dd/MM/yyyy HH:mm:ss",
                                                    int numberAtb = 6)
        {
            DateTime outval;
            if (data == null)
                data = new List<CO_SWIPE_DATA>();

            CO_SWIPE_DATA item;
            XmlDocument xmlDoc = new XmlDocument();

            if (!File.Exists(urlSource))
            {
                return; //data;
            }
            xmlDoc.Load(urlSource);

            XmlNode rptNode = xmlDoc.SelectSingleNode(strRoot);
            int currentNode = 0;
            int currentPercent = 0;
            string EMPLOYEEID = string.Empty;

            foreach (XmlElement node in rptNode.ChildNodes)
            {
                currentNode++;

                if (node.Attributes.Count == numberAtb)
                {
                    EMPLOYEEID = node.Attributes[strID].Value;
                    if (DateTime.TryParseExact(node.Attributes[strDate].Value,
                             strFormat, null, System.Globalization.DateTimeStyles.None, out outval))
                    {
                        var index = data.Find(obj => obj.EMPLOYEEID == EMPLOYEEID && obj.WORKINGDAY == workingDay);

                        if (index != null)
                        {
                            if (DateTime.TryParseExact(string.Format("{0:dd/MM/yyyy} {1}", workingDay, node.Attributes[strIN].Value),
                                 strFormat, null, System.Globalization.DateTimeStyles.None, out outval))
                            {
                                if (outval.Hour < 9) outval = outval.AddDays(1);
                                SetInOut(index, outval);
                            }
                            if (DateTime.TryParseExact(string.Format("{0:dd/MM/yyyy} {1}", workingDay, node.Attributes[strIOUT].Value),
                                 strFormat, null, System.Globalization.DateTimeStyles.None, out outval))
                            {
                                if (outval.Hour < 9) outval = outval.AddDays(1);
                                SetInOut(index, outval);
                            }
                        }
                        else
                        {
                            item = new CO_SWIPE_DATA();
                            item.USERNAME = "AUTO";
                            item.EMPLOYEEID = EMPLOYEEID;
                            item.WORKINGDAY = workingDay;

                            if (DateTime.TryParseExact(string.Format("{0:dd/MM/yyyy} {1}", workingDay, node.Attributes[strIN].Value),
                                 strFormat, null, System.Globalization.DateTimeStyles.None, out outval))
                            {
                                //if (outval.Hour < 6) outval = outval.AddDays(1);
                                item.VALIN1 = outval;
                            }

                            if (DateTime.TryParseExact(string.Format("{0:dd/MM/yyyy} {1}", workingDay, node.Attributes[strIOUT].Value),
                                 strFormat, null, System.Globalization.DateTimeStyles.None, out outval))
                            {
                                if (item.VALIN1.HasValue)
                                {
                                    if (outval.Hour < 9) outval = outval.AddDays(1);

                                    //if (outval.Hour > 12 || item.VALIN1.HasValue)
                                    item.VALOUT1 = outval;
                                    //else
                                    //item.VALIN1 = outval;
                                }
                                else
                                {
                                    item.VALIN1 = outval;
                                }

                            }
                            data.Add(item);
                        }
                    }
                }
                double x = currentNode * 100 / rptNode.ChildNodes.Count;
                currentPercent = ((currentFile - 1) * 100 + (int)Math.Round(x)) / numberFile;

                bgWorker.ReportProgress(currentPercent, "Đọc dữ liệu vào ra {0}%...");
            }

            //return data;
        }
    }
}

