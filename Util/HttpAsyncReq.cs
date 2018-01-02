using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OkexTrader.Util
{
    class HttpAsyncReq
    {
        public delegate void ResponseCallback(String content);
        public ResponseCallback m_callback;

        public void requestHttpPostAsync(String url_prex, String url, Dictionary<String, String> paras, 
                    ResponseCallback cb)
        {
            m_callback = cb;
            try
            {  //组装访问路径
                url = url_prex + url;
                //根据url创建HttpWebRequest对象
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                //设置请求方式和头信息
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                //遍历参数集合
                if (!(paras == null || paras.Count == 0))
                {
                    StringBuilder buffer = new StringBuilder();
                    int i = 0;
                    foreach (string key in paras.Keys)
                    {
                        if (i > 0)
                        {
                            buffer.AppendFormat("&{0}={1}", key, paras[key]);
                        }
                        else
                        {
                            buffer.AppendFormat("{0}={1}", key, paras[key]);
                        }
                        i++;
                    }
                    byte[] btBodys = Encoding.UTF8.GetBytes(buffer.ToString());
                    httpWebRequest.ContentLength = btBodys.Length;
                    //将请求内容封装在请求体中
                    httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);
                    httpWebRequest.BeginGetResponse(readCallback, httpWebRequest);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void readCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest request = (HttpWebRequest)asynchronousResult.AsyncState;

            //Stream postStream = request.EndGetResponse(asynchronousResult);

            //异步回调方法使用 EndGetResponse 方法返回实际的 WebResponse。 
            HttpWebResponse httpWebResponse = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            //读取响应内容
            String responseContent = streamReader.ReadToEnd();
            if(m_callback != null)
            {
                m_callback(responseContent);
            }

            if (httpWebResponse != null)
            {
                httpWebResponse.Close();

            }
            if (streamReader != null)
            {
                streamReader.Close();
            }
        }
    }
}
