using _01_Framework.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using S3Object = _01_Framework.Application.AwsServices.AwsDto.S3Object;

namespace _01_Framework.Application.TusServices;

public class StorageServiceTus : IStorageServiceTus
{
    private readonly IHubContext<UploadHub> _hubContext;
    private readonly HttpClient _http;
    private const string ChannelId = "94528717-ca77-4b8a-9611-db8027a4291a";

    private const string ApiBaseUrl = "https://napi.arvancloud.ir/vod/2.0";
    public StorageServiceTus(IHubContext<UploadHub> hubContext)
    {
        _hubContext = hubContext;
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("apikey", "a4db47ac-9a25-595a-b444-02765bf5b253");
    }

    public async Task<string> InitiateUploadAsync(S3Object obj, string metadata)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, ApiBaseUrl + "/channels/" + ChannelId + "/files");
        req.Headers.Add("Tus-Resumable", "1.0.0");
        req.Headers.Add("Upload-Length", obj.InputStream.Length.ToString());
        req.Headers.Add("Upload-Metadata", metadata);

        var res = await _http.SendAsync(req);
        res.EnsureSuccessStatusCode();

        return res.Headers.Location.ToString();
    }


    public async Task UploadPartsAsync(S3Object obj, string uploadUrl, CancellationToken ct)
    {
        obj.InputStream.Position = 0;

        long totalBytes = obj.InputStream.Length;
        long uploadedBytes = 0;

        var buffer = new byte[500 * 1024];
        int bytesRead;

        while ((bytesRead = await obj.InputStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
            ct.ThrowIfCancellationRequested();

            var chunk = new ByteArrayContent(buffer.Take(bytesRead).ToArray());
            chunk.Headers.ContentType = new MediaTypeHeaderValue("application/offset+octet-stream");

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), uploadUrl);
            req.Headers.Add("Tus-Resumable", "1.0.0");
            req.Headers.Add("Upload-Offset", uploadedBytes.ToString());
            req.Content = chunk;

            HttpResponseMessage res;
            try
            {
                res = await _http.SendAsync(req, HttpCompletionOption.ResponseHeadersRead, ct);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (IOException) when (ct.IsCancellationRequested)
            {
                throw new OperationCanceledException(ct);
            }
            catch (SocketException) when (ct.IsCancellationRequested)
            {
                throw new OperationCanceledException(ct);
            }

            if (!res.IsSuccessStatusCode)
            {
                if (ct.IsCancellationRequested)
                    return; 

                res.EnsureSuccessStatusCode();
            }

            uploadedBytes += bytesRead;

            int percent = (int)((uploadedBytes * 100) / totalBytes);

            await _hubContext.Clients.All.SendAsync("ReceiveVideoProgress", percent);
        }

        await _hubContext.Clients.All.SendAsync("ReceiveVideoProgress", 100);
    }


    public async Task<string> FinalizeUploadAsync(string fileName, string fileId)
    {
        var body = new
        {
            title = fileName,
            file_id = fileId,
            convert_mode = "auto"
        };

        var json = JsonConvert.SerializeObject(body);

        var req = new HttpRequestMessage(HttpMethod.Post,
            $"{ApiBaseUrl}/channels/{ChannelId}/videos");

        req.Content = new StringContent(json, Encoding.UTF8, "application/json");

        var res = await _http.SendAsync(req);

        if (!res.IsSuccessStatusCode)
        {
            var error = await res.Content.ReadAsStringAsync();
            throw new Exception($"Finalize error {res.StatusCode}: {error}");
        }
        var responseJson = await res.Content.ReadAsStringAsync();

        dynamic obj = JsonConvert.DeserializeObject(responseJson);

        string videoId = obj.data.id;

        return videoId;
    }

    public async Task<string> GetPlayerUrlAsync(string videoId)
    {
        int retries = 60;
        int delay = 1000;

        for (int i = 0; i < retries; i++)
        {
            var req = new HttpRequestMessage(
                HttpMethod.Get,
                $"{ApiBaseUrl}/videos/{videoId}"
            );

            var res = await _http.SendAsync(req);
            var json = await res.Content.ReadAsStringAsync();

            dynamic obj = JsonConvert.DeserializeObject(json);
            string status = obj.data.status;

            if (status == "complete")
            {
                return obj.data.player_url;
                
            }
               

            await Task.Delay(delay);
        }

        return null;
    }

    public async Task DeleteFileAsync(string fileId)
    {
        try
        {
            var req = new HttpRequestMessage(HttpMethod.Delete,
                $"{ApiBaseUrl}/files/{fileId}");
            await _http.SendAsync(req);
        }
        catch
        {
            
        }
    }

    public async Task DeleteVideoAsync(string videoId)
    {
        try
        {
            var req = new HttpRequestMessage(HttpMethod.Delete,
                $"{ApiBaseUrl}/videos/{videoId}");
            await _http.SendAsync(req);
        }
        catch
        {

        }
    }
}
