using System;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;

namespace HiStaff.Util
{
    public delegate void SendCompletedCallback(object sender, AsyncCompletedEventArgs e);

    public class SEmail
    {
        public event SendCompletedCallback SendCompleted;
        private SmtpClient _smtpClient;
        public SEmail(string host, int port, string username, string password, bool enableSsl)
        {
            _smtpClient = new SmtpClient();
            _smtpClient.Host = host;
            _smtpClient.Port = port;
            _smtpClient.EnableSsl = enableSsl;
            _smtpClient.Credentials = new NetworkCredential(username, password);
            _smtpClient.SendCompleted += new SendCompletedEventHandler(_smtpClient_SendCompleted);
        }

        public SEmail(string encryptconfig)
        {
            string host = string.Empty;
            int port = 0;
            bool enableSsl = false;
            string username = string.Empty;
            string password = string.Empty;
            string decrypt = string.Empty;
            string[] decryptSplit;

            _smtpClient = new SmtpClient();
            //decrypt = CryptographyManager.Decrypt(encryptconfig);
            decrypt = encryptconfig;

            if (!string.IsNullOrEmpty(encryptconfig))
            {
                decryptSplit = encryptconfig.Split('|');
                if (decryptSplit.Length >= 5)
                {
                    host = decryptSplit[0];
                    port = Convert.ToInt32(decryptSplit[1]);
                    username = decryptSplit[2];
                    password = decryptSplit[3];
                    enableSsl = decryptSplit[4].ToUpper() == "TRUE" ? true : false;
                }
            }

            _smtpClient.Credentials = new NetworkCredential(username, password);
            _smtpClient.Host = host;
            _smtpClient.Port = port;
            _smtpClient.EnableSsl = enableSsl;
            _smtpClient.SendCompleted += new SendCompletedEventHandler(_smtpClient_SendCompleted);
        }

        void _smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SendCompleted(sender, e);
        }

        public bool SendMail(string from, string fromname, string cc,
                             string to, string toname,
                             string subject, string body, 
                             bool isBodyHtml, decimal pkey)
        {

            string[] arrCC = null;

            if (cc != string.Empty)
            {
                arrCC = cc.Split(',');
            }

            MailAddress _cc;

            MailAddress _from = new MailAddress(from, fromname);

            MailAddress _to = new MailAddress(to, toname);

            MailMessage message = new MailMessage(_from, _to);

            if (arrCC!= null && arrCC.Length > 0)
            {
                foreach (string ccItem in arrCC)
                {
                    _cc = new MailAddress(ccItem);

                    message.CC.Add(_cc);
                }
            }

            message.Body = body;

            message.IsBodyHtml = isBodyHtml;

            message.Subject = subject;

            message.BodyEncoding = System.Text.Encoding.UTF8;

            try
            {

                _smtpClient.SendAsync(message, pkey);

            }

            catch (Exception)
            {

                //Response.Write("Exception is:" + ex.ToString());

                return false;

            }

            return true;
        }
    }
}
