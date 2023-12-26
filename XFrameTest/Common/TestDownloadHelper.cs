
using System.Net;
using XFrame.Modules.Download;

namespace XFrameTest
{
    public class TestDownloadHelper : DownloadHelperBase
    {
        private bool m_IsDone;
        private DownloadResult m_Result;
        private string m_Url;
        private HttpClient m_Request;
        private HttpResponseMessage m_Response;

        protected override void OnDispose()
        {
            m_Request?.Dispose();
            m_Response?.Dispose();
            m_Request = null;
            m_Response = null;
        }

        protected override void OnInit()
        {
            m_IsDone = false;
        }


        protected override void OnUpdate()
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

        protected override void Request()
        {
            m_Request = new HttpClient();
            m_Response = m_Request.Send(new HttpRequestMessage(HttpMethod.Get, m_Url));
        }
    }
}
