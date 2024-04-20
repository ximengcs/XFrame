
using System.Net;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;

namespace XFrameTest
{
    public class TestDownloadHelper : IDownloadHelper
    {
        private bool m_IsDone;
        private DownloadResult m_Result;
        private string m_Url;
        private HttpClient m_Request;
        private HttpResponseMessage m_Response;

        public string[] ReserveUrl { get; set; }

        bool IDownloadHelper.IsDone => m_IsDone;

        DownloadResult IDownloadHelper.Result => m_Result;

        string IDownloadHelper.Url
        {
            get => m_Url;
            set => m_Url = value;
        }

        void IDownloadHelper.OnDispose()
        {
            m_Request?.Dispose();
            m_Response?.Dispose();
            m_Request = null;
            m_Response = null;
        }

        void IDownloadHelper.OnInit()
        {
            m_IsDone = false;
        }

        void IDownloadHelper.OnUpdate()
        {
            if (m_Request == null)
                return;
            if (m_Response == null)
                return;
            if (m_Response.StatusCode == HttpStatusCode.Processing)
                return;
            if (m_Response.IsSuccessStatusCode)
            {
                Stream stream = m_Response.Content.ReadAsStream();
                StreamReader reader = new StreamReader(stream);
                m_Result = new DownloadResult(true, reader.ReadToEnd(), null, null);
            }
            else
            {
                m_Result = new DownloadResult(false, null, null, m_Response.StatusCode.ToString());
            }
            m_IsDone = true;
        }

        void IDownloadHelper.Request()
        {
            m_Request = new HttpClient();
            m_Response = m_Request.Send(new HttpRequestMessage(HttpMethod.Get, m_Url));
        }
    }
}
