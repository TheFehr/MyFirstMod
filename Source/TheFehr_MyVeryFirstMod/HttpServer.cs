using System;
using System.Collections.Concurrent;
using System.Net;
using System.Text;
using System.Threading;
using Verse;

namespace TheFehr_MyVeryFirstMod
{
    class HttpServer : IDisposable
    {
        private readonly HttpListener _httpListener;
        private Thread _listenerLoop;
        private readonly Thread[] _requestProcessors;
        private readonly BlockingCollection<HttpListenerContext> _messages;

        protected HttpServer(int threadCount)
        {
            _requestProcessors = new Thread[threadCount];
            _messages = new BlockingCollection<HttpListenerContext>();
            _httpListener = new HttpListener();
        }

        protected internal virtual int Port { get; set; } = 80;

        protected virtual string[] Prefixes
        {
            get { return new[] { $"http://127.0.0.1:{Port}/" }; }
        }

        public void Start()
        {
            _listenerLoop = new Thread(HandleRequests);

            foreach (var prefix in Prefixes)
            {
                Log.Message($"Adding '{prefix}' as prefix");
                _httpListener.Prefixes.Add(prefix);
            }

            _listenerLoop.Start();

            for (int i = 0; i < _requestProcessors.Length; i++)
            {
                _requestProcessors[i] = StartProcessor(i, _messages);
            }
        }

        public void Dispose()
        {
            Stop();
        }

        protected void Stop()
        {
            _messages.CompleteAdding();

            foreach (Thread worker in _requestProcessors) worker.Join();

            _httpListener.Stop();
            _listenerLoop.Join();
        }

        private void HandleRequests()
        {
            _httpListener.Start();
            try
            {
                while (_httpListener.IsListening)
                {
                    Log.Message("The Listener Is Listening!");
                    HttpListenerContext context = _httpListener.GetContext();

                    _messages.Add(context);
                    Log.Message("The Listener has added a message!");
                }
            }
            catch (Exception e)
            {
                Log.Message(e.Message);
            }
        }

        private Thread StartProcessor(int number, BlockingCollection<HttpListenerContext> messages)
        {
            Thread thread = new Thread(() => Processor(number, messages));
            thread.Start();
            return thread;
        }

        private void Processor(int number, BlockingCollection<HttpListenerContext> messages)
        {
            Log.Message($"Processor {number} started.");
            try
            {
                for (;;)
                {
                    Log.Message($"Processor {number} awoken.");
                    HttpListenerContext context = messages.Take();
                    Log.Message($"Processor {number} dequeued message.");
                    Response(context);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }

            Log.Message($"Processor {number} terminated.");
        }

        protected virtual void Response(HttpListenerContext context)
        {
            SendReply(context,
                new StringBuilder(
                    "<html><head><title>NULL</title></head><body>This site is not yet implemented.</body></html>"));
        }

        protected static void SendReply(HttpListenerContext context, StringBuilder responseString)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseString.ToString());
            context.Response.ContentLength64 = buffer.Length;
            var output = context.Response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}