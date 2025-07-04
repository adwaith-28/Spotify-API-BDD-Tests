using System;
using System.Net;
using System.Threading.Tasks;

public class SpotifyOAuthServer
{
    private readonly HttpListener _listener;
    public string AuthCode { get; private set; }

    public SpotifyOAuthServer(string redirectUri)
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://127.0.0.1:8888/callback/");  
    }

    public async Task StartAsync()
    {
        _listener.Start();
        var context = await _listener.GetContextAsync();
        var request = context.Request;
        var response = context.Response;

        AuthCode = request.QueryString["code"];

        var responseString = "<html><body><h1>You can close this window.</h1></body></html>";
        var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
        response.Close();

        _listener.Stop();
    }
}