#nullable disable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace num1_Project
{
    public static class DatabaseHelper
    {
        private static HttpClient _http;
        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        private static bool _initialized;

        public static string ServerBaseUrl { get; private set; } = "http://192.168.0.10:5078";
        public static string ConnStr => "";
        public static string MusicFolder => "";
        public static string CacheFolder =>
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SongCache");

        public static UserDto CurrentUser { get; private set; }

        // [추가] 관리자 체크박스 + AdminRole 둘 다 충족했을 때 true
        public static bool IsAdminLogin { get; set; } = false;

        // [추가] 다운로드 기록 저장 파일
        private static readonly string DownloadHistoryPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "download_history.json");

        public static void InitDatabase()
        {
            InitApi();
        }

        public static void InitApi(string serverBaseUrl = "http://192.168.0.10:5078")
        {
            if (string.IsNullOrWhiteSpace(serverBaseUrl))
                serverBaseUrl = "http://192.168.0.10:5078";

            serverBaseUrl = serverBaseUrl.TrimEnd('/');

            if (_initialized)
            {
                if (string.Equals(ServerBaseUrl, serverBaseUrl, StringComparison.OrdinalIgnoreCase))
                    return;

                _http?.Dispose();
                _http = null;
                _initialized = false;
            }

            ServerBaseUrl = serverBaseUrl;

            _http = new HttpClient();
            _http.BaseAddress = new Uri(ServerBaseUrl);
            _http.Timeout = TimeSpan.FromSeconds(30);

            if (!Directory.Exists(CacheFolder))
                Directory.CreateDirectory(CacheFolder);

            _initialized = true;
        }

        private static void EnsureInitialized()
        {
            if (!_initialized || _http == null)
                InitApi();
        }

        public static void Logout()
        {
            CurrentUser = null;
            IsAdminLogin = false; // [추가] 로그아웃 시 관리자 상태 초기화
        }

        public static (int added, int skipped) ScanMusicFolder()
        {
            return (0, 0);
        }

        public static List<string> GetAllGenres()
        {
            return GetAllGenresAsync().GetAwaiter().GetResult();
        }

        public static async Task<List<string>> GetAllGenresAsync()
        {
            EnsureInitialized();

            try
            {
                var list = await _http.GetFromJsonAsync<List<string>>(
                    "/api/songs/genres", _jsonOptions);

                return list ?? new List<string>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("장르 목록을 불러오지 못했습니다. " + ex.Message, ex);
            }
        }

        public static List<SongInfo> GetSongsByGenre(string genreName)
        {
            return GetSongsByGenreAsync(genreName).GetAwaiter().GetResult();
        }

        public static async Task<List<SongInfo>> GetSongsByGenreAsync(string genreName)
        {
            EnsureInitialized();

            try
            {
                string url = "/api/songs";
                if (!string.IsNullOrWhiteSpace(genreName) && genreName != "전체")
                    url += "?genre=" + Uri.EscapeDataString(genreName);

                var list = await _http.GetFromJsonAsync<List<SongInfo>>(url, _jsonOptions);
                return list ?? new List<SongInfo>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("음악 목록을 불러오지 못했습니다. " + ex.Message, ex);
            }
        }

        public static void IncrementPlayCount(int songId)
        {
            IncrementPlayCountAsync(songId).GetAwaiter().GetResult();
        }

        public static async Task IncrementPlayCountAsync(int songId)
        {
            EnsureInitialized();

            try
            {
                using var response = await _http.PostAsync($"/api/songs/{songId}/play", null);
            }
            catch
            {
            }
        }

        public static async Task<List<SongInfo>> GetRecentPlayedAsync(int limit = 50)
        {
            EnsureInitialized();

            try
            {
                var list = await _http.GetFromJsonAsync<List<SongInfo>>(
                    $"/api/songs/recent?limit={limit}", _jsonOptions);
                return list ?? new List<SongInfo>();
            }
            catch
            {
                return new List<SongInfo>();
            }
        }

        public static async Task<List<SongInfo>> GetMostPlayedAsync(int limit = 50)
        {
            EnsureInitialized();

            try
            {
                var list = await _http.GetFromJsonAsync<List<SongInfo>>(
                    $"/api/songs/top?limit={limit}", _jsonOptions);
                return list ?? new List<SongInfo>();
            }
            catch
            {
                return new List<SongInfo>();
            }
        }

        public static async Task<List<NewsItem>> GetNewsByYearAsync(int year)
        {
            EnsureInitialized();

            try
            {
                var list = await _http.GetFromJsonAsync<List<NewsApiResponse>>(
                    $"/api/news/{year}", _jsonOptions);

                if (list == null)
                    return new List<NewsItem>();

                return list.Select(n => new NewsItem
                {
                    Headline = n.Headline,
                    Date = n.Month.HasValue ? $"{n.Year}년 {n.Month}월" : $"{n.Year}년",
                    Body = n.Description,
                    ImageUrl = n.ImageUrl,
                }).ToList();
            }
            catch
            {
                return new List<NewsItem>();
            }
        }

        public static async Task<bool> DeleteSongAsync(int songId)
        {
            EnsureInitialized();

            using var response = await _http.DeleteAsync($"/api/songs/{songId}");
            if (!response.IsSuccessStatusCode)
                return false;

            try
            {
                foreach (var file in Directory.GetFiles(CacheFolder, $"{songId}.*"))
                    File.Delete(file);
            }
            catch
            {
            }

            return true;
        }

        public static async Task<ApiResult<UserDto>> LoginAsync(string username, string password)
        {
            EnsureInitialized();

            try
            {
                var request = new LoginRequest
                {
                    Username = username,
                    Password = password
                };

                using var response = await _http.PostAsJsonAsync("/api/auth/login", request);
                var body = await ReadJsonSafeAsync<AuthResponseDto>(response);

                if (response.IsSuccessStatusCode && body != null && body.Success && body.User != null)
                {
                    CurrentUser = body.User;
                    return ApiResult<UserDto>.Ok(body.User, body.Message);
                }

                return ApiResult<UserDto>.Fail(
                    body?.Message ?? $"로그인 실패 ({(int)response.StatusCode})");
            }
            catch (Exception ex)
            {
                return ApiResult<UserDto>.Fail($"서버 연결 오류: {ex.Message}");
            }
        }

        public static async Task<ApiResult> RegisterAsync(RegisterRequest request)
        {
            EnsureInitialized();

            try
            {
                using var response = await _http.PostAsJsonAsync("/api/auth/register", request);
                var body = await ReadJsonSafeAsync<AuthResponseDto>(response);

                if (response.IsSuccessStatusCode && body != null && body.Success)
                    return ApiResult.Ok(body.Message);

                return ApiResult.Fail(
                    body?.Message ?? $"회원가입 실패 ({(int)response.StatusCode})");
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"서버 연결 오류: {ex.Message}");
            }
        }

        private static string GetSongExtension(SongInfo song)
        {
            string ext = "";

            try
            {
                if (!string.IsNullOrWhiteSpace(song?.FileUrl))
                {
                    string cleanUrl = song.FileUrl;
                    int q = cleanUrl.IndexOf('?');
                    if (q >= 0)
                        cleanUrl = cleanUrl.Substring(0, q);

                    ext = Path.GetExtension(cleanUrl);
                }
            }
            catch
            {
            }

            if (string.IsNullOrWhiteSpace(ext))
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(song?.FilePath))
                        ext = Path.GetExtension(song.FilePath);
                }
                catch
                {
                }
            }

            if (string.IsNullOrWhiteSpace(ext))
                ext = ".mp3";

            return ext;
        }

        public static async Task<string> DownloadSongToCacheAsync(SongInfo song)
        {
            EnsureInitialized();

            if (song == null || string.IsNullOrWhiteSpace(song.FileUrl))
                return "";

            string ext = GetSongExtension(song);
            string localPath = Path.Combine(CacheFolder, $"{song.SongId}{ext}");
            string tempPath = localPath + ".tmp";

            try
            {
                foreach (var oldFile in Directory.GetFiles(CacheFolder, $"{song.SongId}.*"))
                {
                    try
                    {
                        File.Delete(oldFile);
                    }
                    catch
                    {
                    }
                }

                if (File.Exists(tempPath))
                {
                    try
                    {
                        File.Delete(tempPath);
                    }
                    catch
                    {
                    }
                }

                using var response = await _http.GetAsync(
                    song.FileUrl,
                    HttpCompletionOption.ResponseHeadersRead);

                response.EnsureSuccessStatusCode();

                await using (var src = await response.Content.ReadAsStreamAsync())
                await using (var dst = new FileStream(
                    tempPath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None))
                {
                    await src.CopyToAsync(dst);
                }

                if (File.Exists(localPath))
                {
                    try
                    {
                        File.Delete(localPath);
                    }
                    catch
                    {
                    }
                }

                File.Move(tempPath, localPath);
                return localPath;
            }
            catch (Exception ex)
            {
                try
                {
                    if (File.Exists(tempPath))
                        File.Delete(tempPath);
                }
                catch
                {
                }

                throw new InvalidOperationException("음원 다운로드 실패: " + ex.Message, ex);
            }
        }

        public static async Task<ApiResult<CapsuleInfo>> CreateCapsuleAsync(CapsuleInfo capsule)
        {
            EnsureInitialized();

            if (CurrentUser == null)
                return ApiResult<CapsuleInfo>.Fail("로그인 후 이용하세요.");

            if (capsule == null)
                return ApiResult<CapsuleInfo>.Fail("캡슐 정보가 없습니다.");

            var request = new CapsuleCreateRequestDto
            {
                UserId = CurrentUser.UserId,
                Title = capsule.Title,
                OpenDate = capsule.OpenDate.Date,
                SongIds = capsule.Songs
                    .Where(x => x != null && x.SongId > 0)
                    .Select(x => x.SongId)
                    .Distinct()
                    .ToList()
            };

            try
            {
                using var response = await _http.PostAsJsonAsync("/api/capsules", request);
                var body = await ReadJsonSafeAsync<CapsuleInfo>(response);
                var error = await ReadJsonSafeAsync<ErrorResponseDto>(response);

                if (response.IsSuccessStatusCode && body != null)
                    return ApiResult<CapsuleInfo>.Ok(body, "캡슐 저장 완료");

                return ApiResult<CapsuleInfo>.Fail(
                    error?.Message ?? $"캡슐 저장 실패 ({(int)response.StatusCode})");
            }
            catch (Exception ex)
            {
                return ApiResult<CapsuleInfo>.Fail("캡슐 저장 실패: " + ex.Message);
            }
        }

        public static async Task<ApiResult<CapsuleInfo>> UpdateCapsuleAsync(CapsuleInfo capsule)
        {
            EnsureInitialized();

            if (CurrentUser == null)
                return ApiResult<CapsuleInfo>.Fail("로그인 후 이용하세요.");

            if (capsule == null)
                return ApiResult<CapsuleInfo>.Fail("캡슐 정보가 없습니다.");

            if (capsule.CapsuleId <= 0)
                return ApiResult<CapsuleInfo>.Fail("수정할 캡슐 정보가 올바르지 않습니다.");

            var request = new CapsuleCreateRequestDto
            {
                UserId = CurrentUser.UserId,
                Title = capsule.Title,
                OpenDate = capsule.OpenDate.Date,
                SongIds = capsule.Songs
                    .Where(x => x != null && x.SongId > 0)
                    .Select(x => x.SongId)
                    .Distinct()
                    .ToList()
            };

            try
            {
                using var response = await _http.PutAsJsonAsync($"/api/capsules/{capsule.CapsuleId}", request);
                var body = await ReadJsonSafeAsync<CapsuleInfo>(response);
                var error = await ReadJsonSafeAsync<ErrorResponseDto>(response);

                if (response.IsSuccessStatusCode && body != null)
                    return ApiResult<CapsuleInfo>.Ok(body, "캡슐 수정 완료");

                return ApiResult<CapsuleInfo>.Fail(
                    error?.Message ?? $"캡슐 수정 실패 ({(int)response.StatusCode})");
            }
            catch (Exception ex)
            {
                return ApiResult<CapsuleInfo>.Fail("캡슐 수정 실패: " + ex.Message);
            }
        }

        public static async Task<bool> DeleteCapsuleAsync(int capsuleId)
        {
            EnsureInitialized();

            try
            {
                using var response = await _http.DeleteAsync($"/api/capsules/{capsuleId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<List<CapsuleInfo>> GetMyCapsulesAsync()
        {
            EnsureInitialized();

            if (CurrentUser == null)
                return new List<CapsuleInfo>();

            try
            {
                var list = await _http.GetFromJsonAsync<List<CapsuleInfo>>(
                    $"/api/capsules/user/{CurrentUser.UserId}", _jsonOptions);

                return list ?? new List<CapsuleInfo>();
            }
            catch
            {
                return new List<CapsuleInfo>();
            }
        }

        public static async Task<List<CapsuleOpenNoticeDto>> GetOpenedCapsulesToNotifyAsync()
        {
            EnsureInitialized();

            if (CurrentUser == null)
                return new List<CapsuleOpenNoticeDto>();

            try
            {
                var list = await _http.GetFromJsonAsync<List<CapsuleOpenNoticeDto>>(
                    $"/api/capsules/user/{CurrentUser.UserId}/opened-pending", _jsonOptions);

                return list ?? new List<CapsuleOpenNoticeDto>();
            }
            catch
            {
                return new List<CapsuleOpenNoticeDto>();
            }
        }

        public static async Task<bool> AcknowledgeCapsuleOpenedAsync(int capsuleId)
        {
            EnsureInitialized();

            try
            {
                using var response = await _http.PostAsync($"/api/capsules/{capsuleId}/ack-open", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public static async Task CheckOpenedCapsulesAndNotifyAsync(IWin32Window owner = null)
        {
            if (CurrentUser == null)
                return;

            var list = await GetOpenedCapsulesToNotifyAsync();
            foreach (var item in list)
            {
                MessageBox.Show(owner, $"{item.Title} 캡슐이 열렸어요!!", "타임캡슐");
                await AcknowledgeCapsuleOpenedAsync(item.CapsuleId);
            }
        }

        public static async Task<ApiResult> UpdatePaymentAsync(string planType)
        {
            EnsureInitialized();

            if (CurrentUser == null)
                return ApiResult.Fail("로그인 정보가 없습니다.");

            try
            {
                var request = new UpdatePaymentRequest
                {
                    UserId = CurrentUser.UserId,
                    PlanType = planType
                };

                using var response = await _http.PostAsJsonAsync("/api/auth/payment", request);
                var body = await ReadJsonSafeAsync<AuthResponseDto>(response);

                if (response.IsSuccessStatusCode && body != null && body.Success)
                {
                    // [추가] 결제 성공 시 현재 로그인 사용자 요금제 반영
                    if (CurrentUser != null)
                        CurrentUser.PlanType = planType;

                    return ApiResult.Ok(body.Message);
                }

                return ApiResult.Fail(body?.Message ?? $"결제 업데이트 실패 ({(int)response.StatusCode})");
            }
            catch (Exception ex)
            {
                return ApiResult.Fail($"서버 연결 오류: {ex.Message}");
            }
        }

        // [추가] 요금제 문자열 정리
        private static string NormalizePlanType(string planType)
        {
            return string.IsNullOrWhiteSpace(planType) ? "" : planType.Trim();
        }

        // [추가] 현재 로그인한 유저의 요금제 반환
        public static string GetCurrentPlanType()
        {
            return NormalizePlanType(CurrentUser?.PlanType);
        }

        // [추가] 미가입 유저인지 여부
        public static bool IsPreviewOnlyUser()
        {
            string plan = GetCurrentPlanType();
            return plan != "일반" && plan != "VIP";
        }

        // [추가] 요금제별 주간 다운로드 제한
        private static int GetWeeklyDownloadLimitByPlan(string planType)
        {
            planType = NormalizePlanType(planType);

            if (planType == "일반")
                return 3;

            if (planType == "VIP")
                return 10;

            return 0;
        }

        // [추가] 현재 유저의 주간 다운로드 제한
        public static int GetWeeklyDownloadLimit()
        {
            return GetWeeklyDownloadLimitByPlan(GetCurrentPlanType());
        }

        // [추가] 다운로드 기록 모델
        private sealed class DownloadHistoryItem
        {
            public int UserId { get; set; }
            public int SongId { get; set; }
            public DateTime DownloadedAt { get; set; }
        }

        // [추가] 다운로드 기록 파일 로드
        private static List<DownloadHistoryItem> LoadDownloadHistory()
        {
            try
            {
                if (!File.Exists(DownloadHistoryPath))
                    return new List<DownloadHistoryItem>();

                string json = File.ReadAllText(DownloadHistoryPath);
                if (string.IsNullOrWhiteSpace(json))
                    return new List<DownloadHistoryItem>();

                var list = JsonSerializer.Deserialize<List<DownloadHistoryItem>>(json, _jsonOptions);
                return list ?? new List<DownloadHistoryItem>();
            }
            catch
            {
                return new List<DownloadHistoryItem>();
            }
        }

        // [추가] 다운로드 기록 저장
        private static void SaveDownloadHistory(List<DownloadHistoryItem> list)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(list ?? new List<DownloadHistoryItem>(), options);
                File.WriteAllText(DownloadHistoryPath, json);
            }
            catch
            {
            }
        }

        // [추가] 최근 7일 다운로드 횟수
        public static int GetWeeklyDownloadUsedCount()
        {
            if (CurrentUser == null)
                return 0;

            DateTime from = DateTime.Now.AddDays(-7);
            var list = LoadDownloadHistory();

            return list.Count(x =>
                x.UserId == CurrentUser.UserId &&
                x.DownloadedAt >= from);
        }

        // [추가] 남은 다운로드 횟수
        public static int GetWeeklyDownloadRemainingCount()
        {
            int limit = GetWeeklyDownloadLimit();
            int used = GetWeeklyDownloadUsedCount();
            int remain = limit - used;
            return remain < 0 ? 0 : remain;
        }

        // [추가] 다운로드 가능 여부 확인
        private static (bool allowed, int used, int limit, string message) CheckDownloadPermission()
        {
            if (CurrentUser == null)
                return (false, 0, 0, "로그인 후 다운로드할 수 있습니다.");

            string planType = GetCurrentPlanType();
            int limit = GetWeeklyDownloadLimitByPlan(planType);

            if (limit <= 0)
                return (false, 0, 0, "미가입 유저는 다운로드할 수 없습니다.");

            int used = GetWeeklyDownloadUsedCount();

            if (used >= limit)
                return (false, used, limit, $"이번 주 다운로드 가능 횟수를 모두 사용했습니다. ({used}/{limit})");

            return (true, used, limit, "");
        }

        // [추가] 다운로드 기록 1건 추가
        private static void AddDownloadHistory(int songId)
        {
            if (CurrentUser == null)
                return;

            var list = LoadDownloadHistory();

            list = list
                .Where(x => x.DownloadedAt >= DateTime.Now.AddDays(-30))
                .ToList();

            list.Add(new DownloadHistoryItem
            {
                UserId = CurrentUser.UserId,
                SongId = songId,
                DownloadedAt = DateTime.Now
            });

            SaveDownloadHistory(list);
        }

        // [추가] 저장 대화상자에 보여줄 추천 파일명
        public static string GetSuggestedSongFileName(SongInfo song)
        {
            if (song == null)
                return "music.mp3";

            string ext = GetSongExtension(song);
            string baseName;

            if (!string.IsNullOrWhiteSpace(song.Artist))
                baseName = $"{song.Artist} - {song.Title}";
            else
                baseName = song.Title;

            if (string.IsNullOrWhiteSpace(baseName))
                baseName = "music";

            foreach (char ch in Path.GetInvalidFileNameChars())
                baseName = baseName.Replace(ch, '_');

            return baseName + ext;
        }

        // [추가] 현재 곡을 사용자가 지정한 경로로 다운로드
        public static async Task<ApiResult<string>> DownloadSongForOfflineAsync(SongInfo song, string savePath)
        {
            EnsureInitialized();

            if (song == null)
                return ApiResult<string>.Fail("다운로드할 곡 정보가 없습니다.");

            if (string.IsNullOrWhiteSpace(song.FileUrl))
                return ApiResult<string>.Fail("다운로드할 파일 주소가 없습니다.");

            if (string.IsNullOrWhiteSpace(savePath))
                return ApiResult<string>.Fail("저장 경로가 올바르지 않습니다.");

            var check = CheckDownloadPermission();
            if (!check.allowed)
                return ApiResult<string>.Fail(check.message);

            try
            {
                string dir = Path.GetDirectoryName(savePath);
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using var response = await _http.GetAsync(song.FileUrl, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                await using (var src = await response.Content.ReadAsStreamAsync())
                await using (var dst = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await src.CopyToAsync(dst);
                }

                AddDownloadHistory(song.SongId);

                int afterUsed = check.used + 1;
                return ApiResult<string>.Ok(savePath, $"다운로드 완료 ({afterUsed}/{check.limit})");
            }
            catch (Exception ex)
            {
                return ApiResult<string>.Fail("다운로드 실패: " + ex.Message);
            }
        }

        private static async Task<T> ReadJsonSafeAsync<T>(HttpResponseMessage response)
            where T : class
        {
            try
            {
                return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
            }
            catch
            {
                return null;
            }
        }
    }

    public sealed class ApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static ApiResult Ok(string message = "OK")
        {
            return new ApiResult
            {
                Success = true,
                Message = message
            };
        }

        public static ApiResult Fail(string message)
        {
            return new ApiResult
            {
                Success = false,
                Message = message
            };
        }
    }

    public sealed class ApiResult<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResult<T> Ok(T data, string message = "OK")
        {
            return new ApiResult<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResult<T> Fail(string message)
        {
            return new ApiResult<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }
    }

    public sealed class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public sealed class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string DetailAddr { get; set; }
    }

    public sealed class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserDto User { get; set; }
    }

    public sealed class UserDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        // [추가] 관리자 권한 (1 = 관리자, null = 일반 고객)
        public int? AdminRole { get; set; }

        // [추가] 요금제
        public string PlanType { get; set; }
    }

    public sealed class ErrorResponseDto
    {
        public string Message { get; set; }
    }

    public sealed class CapsuleCreateRequestDto
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public DateTime OpenDate { get; set; }
        public List<int> SongIds { get; set; } = new List<int>();
    }

    public sealed class CapsuleOpenNoticeDto
    {
        public int CapsuleId { get; set; }
        public string Title { get; set; }
        public DateTime OpenDate { get; set; }
    }

    public class SongInfo
    {
        public int SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public string FilePath { get; set; }
        public int PlayCount { get; set; }
        public string FileUrl { get; set; }
        public string AlbumArtUrl { get; set; }

        // [추가] 가사 (.lrc 형식으로 저장, null = 가사 없음)
        public string Lyrics { get; set; }

        public string DurationText =>
            Duration > 0 ? $"{Duration / 60}:{Duration % 60:D2}" : "--:--";

        public string DisplayText =>
            string.IsNullOrWhiteSpace(Artist) ? Title : $"{Title} - {Artist}";

        public override string ToString()
        {
            return DisplayText;
        }
    }

    public sealed class NewsApiResponse
    {
        public int NewsId { get; set; }
        public int Year { get; set; }
        public int? Month { get; set; }
        public string Headline { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }

    public sealed class UpdatePaymentRequest
    {
        public int UserId { get; set; }
        public string PlanType { get; set; }
    }
}