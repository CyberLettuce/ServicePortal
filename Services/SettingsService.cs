using System.Security.Claims;
using System.Text.Json;
using ServicePortal.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace ServicePortal.Services
{
    public class SettingsService
    {
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        private const string StorageKeyPrefix = "sgsHelpdeskSettings";

        public AppSettings Current { get; private set; } = new();

        public SettingsService(
            IJSRuntime js,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _js = js;
            _authenticationStateProvider = authenticationStateProvider;
        }

        private async Task<string> GetStorageKeyAsync()
        {
            var authState =
                await _authenticationStateProvider.GetAuthenticationStateAsync();

            var user = authState.User;

            var userId =
                user.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = user.Identity?.Name;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new InvalidOperationException(
                    "Unable to determine the currently signed-in user.");
            }

            return $"{StorageKeyPrefix}_{userId}";
        }

        public async Task LoadAsync()
        {
            try
            {
                var storageKey = await GetStorageKeyAsync();

                var json = await _js.InvokeAsync<string?>(
                    "localStorage.getItem",
                    storageKey);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var settings =
                        JsonSerializer.Deserialize<AppSettings>(json);

                    if (settings != null)
                    {
                        Current = settings;
                        return;
                    }
                }

                Current = new AppSettings();
            }
            catch
            {
                Current = new AppSettings();
            }
        }

        public async Task SaveAsync()
        {
            var storageKey = await GetStorageKeyAsync();

            var json =
                JsonSerializer.Serialize(Current);

            await _js.InvokeVoidAsync(
                "localStorage.setItem",
                storageKey,
                json);
        }

        public async Task ResetAsync()
        {
            var storageKey = await GetStorageKeyAsync();

            Current = new AppSettings();

            await _js.InvokeVoidAsync(
                "localStorage.removeItem",
                storageKey);
        }

        public void Update(AppSettings settings)
        {
            Current = settings;
        }

        public async Task ApplyAppearanceAsync()
        {
            try
            {
                await _js.InvokeVoidAsync("settingsHelper.setTheme", Current.Theme, Current.DarkMode);
            }
            catch (JSDisconnectedException)
            {
                // The browser has navigated away or refreshed. The next circuit applies the saved setting.
            }
        }
    }
}
