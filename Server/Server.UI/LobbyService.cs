using SharedLibrary.LobbyDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http.Json;

namespace Server.UI
{
    internal class LobbyService
    {
        private readonly HttpClient _http;

        public LobbyService()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            _http = new HttpClient(handler)
            {
                //TODO: change to config
                BaseAddress = new Uri("https://localhost:5001/")
            };
        }

        public async Task<List<LobbyDto>> GetLobbiesAsync()
        {
            try
            {
                var lobbies = await _http.GetFromJsonAsync<List<LobbyDto>>("api/lobby");
                return lobbies ?? new List<LobbyDto>();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return new List<LobbyDto>();
            }
        }

    }
}
