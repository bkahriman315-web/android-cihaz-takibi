using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;

namespace PhoneHub.Desktop.ViewModels
{
    public partial class SilentModeViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7001";

        [ObservableProperty]
        private string selectedDeviceId = "";

        [ObservableProperty]
        private bool isSilentModeEnabled = false;

        [ObservableProperty]
        private bool keepDisplayOff = true;

        [ObservableProperty]
        private bool muteAudio = true;

        [ObservableProperty]
        private bool disableVibration = true;

        [ObservableProperty]
        private bool disableNotificationLED = true;

        [ObservableProperty]
        private bool hideSystemNotifications = true;

        [ObservableProperty]
        private bool disableLogs = true;

        [ObservableProperty]
        private bool clearHistoryAfterSession = true;

        [ObservableProperty]
        private string pinCode = "";

        [ObservableProperty]
        private bool isLoadingStatus = false;

        [ObservableProperty]
        private string statusMessage = "";

        [ObservableProperty]
        private ObservableCollection<SilentModeSessionInfo> sessionHistory = new();

        public SilentModeViewModel()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {GetAuthToken()}");
        }

        [RelayCommand]
        public async Task EnableSilentMode()
        {
            try
            {
                IsLoadingStatus = true;
                StatusMessage = "Gizli mod etkinleştiriliyor...";

                var requestData = new
                {
                    keepDisplayOff = KeepDisplayOff,
                    muteAudio = MuteAudio,
                    disableVibration = DisableVibration,
                    disableNotificationLED = DisableNotificationLED,
                    hideSystemNotifications = HideSystemNotifications,
                    disableLogs = DisableLogs,
                    clearHistoryAfterSession = ClearHistoryAfterSession,
                    pin = string.IsNullOrEmpty(PinCode) ? null : PinCode
                };

                var json = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    $"{_baseUrl}/api/silentmode/enable/{SelectedDeviceId}",
                    content);

                if (response.IsSuccessStatusCode)
                {
                    IsSilentModeEnabled = true;
                    StatusMessage = "✅ Gizli mod başarıyla etkinleştirildi";
                }
                else
                {
                    StatusMessage = $"❌ Hata: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Hata: {ex.Message}";
            }
            finally
            {
                IsLoadingStatus = false;
            }
        }

        [RelayCommand]
        public async Task DisableSilentMode()
        {
            try
            {
                IsLoadingStatus = true;
                StatusMessage = "Gizli mod devre dışı bırakılıyor...";

                var url = $"{_baseUrl}/api/silentmode/disable/{SelectedDeviceId}";
                if (!string.IsNullOrEmpty(PinCode))
                {
                    url += $"?pin={PinCode}";
                }

                var response = await _httpClient.PostAsync(url, new StringContent("{}", System.Text.Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    IsSilentModeEnabled = false;
                    StatusMessage = "✅ Gizli mod başarıyla devre dışı bırakıldı";
                    PinCode = "";
                }
                else
                {
                    StatusMessage = $"❌ Hata: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Hata: {ex.Message}";
            }
            finally
            {
                IsLoadingStatus = false;
            }
        }

        [RelayCommand]
        public async Task GetStatus()
        {
            try
            {
                IsLoadingStatus = true;

                var response = await _httpClient.GetAsync($"{_baseUrl}/api/silentmode/status/{SelectedDeviceId}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<dynamic>(json);

                    IsSilentModeEnabled = data?.isEnabled ?? false;
                    KeepDisplayOff = data?.keepDisplayOff ?? true;
                    MuteAudio = data?.muteAudio ?? true;
                    DisableVibration = data?.disableVibration ?? true;
                    StatusMessage = "✅ Durum alındı";
                }
                else
                {
                    StatusMessage = $"❌ Durum alınamadı: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Hata: {ex.Message}";
            }
            finally
            {
                IsLoadingStatus = false;
            }
        }

        [RelayCommand]
        public async Task LoadSessionHistory()
        {
            try
            {
                IsLoadingStatus = true;

                var response = await _httpClient.GetAsync(
                    $"{_baseUrl}/api/silentmode/device/{SelectedDeviceId}/sessions?limit=50");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var sessions = JsonConvert.DeserializeObject<List<dynamic>>(json);

                    SessionHistory.Clear();
                    if (sessions != null)
                    {
                        foreach (var session in sessions)
                        {
                            SessionHistory.Add(new SilentModeSessionInfo
                            {
                                SessionId = session.sessionId,
                                StartedAt = DateTime.Parse(session.startedAt.ToString()),
                                EndedAt = session.endedAt != null ? DateTime.Parse(session.endedAt.ToString()) : null,
                                OperationCount = session.operationCount,
                                Duration = session.duration
                            });
                        }
                    }
                    StatusMessage = $"✅ {SessionHistory.Count} oturum yüklendi";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Hata: {ex.Message}";
            }
            finally
            {
                IsLoadingStatus = false;
            }
        }

        private string GetAuthToken()
        {
            // Gerçek uygulamada, bu token giriş sırasında kaydedilir
            return "your-jwt-token-here";
        }
    }

    public class SilentModeSessionInfo
    {
        public string SessionId { get; set; } = "";
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int OperationCount { get; set; }
        public string Duration { get; set; } = "";
    }
}
