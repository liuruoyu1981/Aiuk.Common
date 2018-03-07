using System;
using System.Net;
using System.Net.Mail;

namespace Aiuk.Common.Utility
{
    /// <summary>
    /// 邮件工具。
    /// </summary>
    public static class AiukMailUtility
    {
        /// <summary>
        /// 同步发送邮件。
        /// </summary>
        /// <param name="mailMessage">邮件消息对象。</param>
        /// <param name="formAccount">发送方邮箱账号。</param>
        /// <param name="formMailPassword">发送方邮箱密码。</param>
        /// <param name="formSmtp">发送方smtp服务器地址。</param>
        public static void SyncSend(MailMessage mailMessage, string formAccount, string formMailPassword,
            string formSmtp)
        {
            try
            {
                if (mailMessage == null) return;
                var smtpClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(formAccount, formMailPassword) as ICredentialsByHost,
                    Host = formSmtp
                };
                smtpClient.Send(mailMessage);
            }
            catch (Exception exception)
            {
                AiukDebugUtility.LogError(string.Format("邮件发送出现异常，异常为{0}", exception.Message));
            }
        }

        /// <summary>
        /// 异步发送邮件。
        /// </summary>
        /// <param name="mailMessage">邮件消息对象。</param>
        /// <param name="formAccount">发送方邮箱账号。</param>
        /// <param name="formMailPassword">发送方邮箱密码。</param>
        /// <param name="formSmtp">发送方smtp服务器地址。</param>
        public static void AsyncSend(MailMessage mailMessage, string formAccount, string formMailPassword,
            string formSmtp)
        {
            try
            {
                if (mailMessage == null) return;
                var smtpClient = new SmtpClient
                {
                    Credentials = new NetworkCredential(formAccount, formMailPassword) as ICredentialsByHost,
                    Host = formSmtp
                };
                smtpClient.SendAsync(mailMessage, smtpClient);
            }
            catch (Exception exception)
            {
                AiukDebugUtility.LogError(string.Format("邮件发送出现异常，异常为{0}", exception.Message));
            }
        }

    }
}