using Microsoft.AspNetCore.Mvc;
using WorldBeat.Api.Configuration;
using WorldBeat.Api.Contracts;
using WorldBeat.Api.Filters;
using WorldBeat.Api.Infrastructure;
using WorldBeat.Api.Repositories;
using WorldBeat.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("App"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AdminKeyHeaderOperationFilter>();
});
builder.Services.AddControllers();

builder.Services.AddSingleton<ISqliteConnectionFactory, SqliteConnectionFactory>();
builder.Services.AddSingleton<DbInitializer>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IYearNewsRepository, YearNewsRepository>();

builder.Services.AddScoped<AdminKeyFilter>();

builder.Services.AddScoped<ICapsuleRepository, CapsuleRepository>();
builder.Services.AddScoped<ICapsuleService, CapsuleService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();
    await dbInitializer.InitializeAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();

var auth = app.MapGroup("/api/auth");

auth.MapPost("/register", async (
    RegisterRequest request,
    IAuthService authService,
    CancellationToken cancellationToken) =>
{
    var result = await authService.RegisterAsync(request, cancellationToken);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

auth.MapPost("/login", async (
    LoginRequest request,
    IAuthService authService,
    CancellationToken cancellationToken) =>
{
    var result = await authService.LoginAsync(request, cancellationToken);
    return result.Success ? Results.Ok(result) : Results.BadRequest(result);
});

auth.MapPost("/payment", async (
    UpdatePaymentRequest request,
    IUserRepository userRepo,
    CancellationToken cancellationToken) =>
{
    if (request == null || request.UserId <= 0)
        return Results.BadRequest(new AuthResponse { Success = false, Message = "잘못된 요청입니다." });

    if (string.IsNullOrWhiteSpace(request.PlanType))
        return Results.BadRequest(new AuthResponse { Success = false, Message = "요금제 정보가 없습니다." });

    await userRepo.UpdatePaymentAsync(request.UserId, request.PlanType, cancellationToken);

    return Results.Ok(new AuthResponse { Success = true, Message = "결제가 완료되었습니다." });
});

var songs = app.MapGroup("/api/songs");

songs.MapGet("/genres", async (
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var genres = await songService.GetGenresAsync(cancellationToken);
    return Results.Ok(genres);
});

songs.MapGet("/recent", async (
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken,
    [FromQuery] int limit = 50) =>
{
    if (limit <= 0) limit = 50;
    var items = await songService.GetRecentAsync(limit, httpContext, cancellationToken);
    return Results.Ok(items);
});

songs.MapGet("/top", async (
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken,
    [FromQuery] int limit = 50) =>
{
    if (limit <= 0) limit = 50;
    var items = await songService.GetTopAsync(limit, httpContext, cancellationToken);
    return Results.Ok(items);
});

songs.MapGet("/", async (
    [FromQuery] string genre,
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var items = await songService.GetSongsAsync(genre, httpContext, cancellationToken);
    return Results.Ok(items);
});

songs.MapGet("/{songId:int}", async (
    int songId,
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var song = await songService.GetSongAsync(songId, httpContext, cancellationToken);
    return song == null ? Results.NotFound() : Results.Ok(song);
});

songs.MapGet("/{songId:int}/stream", async (
    int songId,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var streamInfo = await songService.OpenSongStreamAsync(songId, cancellationToken);
    if (streamInfo == null)
        return Results.NotFound();

    return Results.File(
        streamInfo.Stream,
        streamInfo.ContentType,
        fileDownloadName: streamInfo.DownloadName,
        enableRangeProcessing: true);
});

songs.MapGet("/{songId:int}/art", async (
    int songId,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var streamInfo = await songService.OpenAlbumArtStreamAsync(songId, cancellationToken);
    if (streamInfo == null)
        return Results.NotFound();

    return Results.File(
        streamInfo.Stream,
        streamInfo.ContentType,
        fileDownloadName: streamInfo.DownloadName,
        enableRangeProcessing: true);
});

songs.MapPost("/{songId:int}/play", async (
    int songId,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    bool ok = await songService.IncrementPlayCountAsync(songId, cancellationToken);
    return ok ? Results.Ok() : Results.NotFound();
});

var adminSongs = app.MapGroup("/api/admin/songs")
    .AddEndpointFilter<AdminKeyFilter>();

adminSongs.MapGet("/", async (
    [FromQuery] string genre,
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var items = await songService.GetSongsAsync(genre, httpContext, cancellationToken);
    return Results.Ok(items);
});

adminSongs.MapGet("/{songId:int}", async (
    int songId,
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    var song = await songService.GetSongAsync(songId, httpContext, cancellationToken);
    return song == null
        ? Results.NotFound(new { success = false, message = "곡을 찾을 수 없습니다." })
        : Results.Ok(song);
});

adminSongs.MapPut("/{songId:int}", async (
    int songId,
    SongUpdateRequest request,
    HttpContext httpContext,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    bool updated = await songService.UpdateSongAsync(songId, request, httpContext, cancellationToken);
    if (!updated)
        return Results.NotFound(new { success = false, message = "곡을 찾을 수 없습니다." });

    var song = await songService.GetSongAsync(songId, httpContext, cancellationToken);
    return Results.Ok(song);
});

adminSongs.MapDelete("/{songId:int}", async (
    int songId,
    ISongService songService,
    CancellationToken cancellationToken) =>
{
    bool deleted = await songService.DeleteSongAsync(songId, cancellationToken);
    return deleted
        ? Results.Ok(new { success = true, message = "삭제 완료" })
        : Results.NotFound(new { success = false, message = "곡을 찾을 수 없습니다." });
});

// ──────────────────────────────────────────────────────────────
// [추가] AdminManager: 유저 관리자 권한 부여/해제 API
//
// AdminKeyFilter 로 보호되어 있어서 X-Admin-Key 헤더가 있어야 호출 가능해.
// Swagger 에서 AdminManager 섹션으로 표시돼.
//
// POST /api/admin/users/{userId}/grant  → 관리자로 지정 (AdminRole = 1)
// POST /api/admin/users/{userId}/revoke → 일반 고객으로 해제 (AdminRole = null)
// GET  /api/admin/users                 → 전체 유저 목록 조회 (관리자 여부 포함)
// ──────────────────────────────────────────────────────────────
var adminUsers = app.MapGroup("/api/admin/users")
    .AddEndpointFilter<AdminKeyFilter>()
    .WithTags("AdminManager"); // [추가] Swagger 에서 AdminManager 섹션으로 표시

// 전체 유저 목록 조회 (AdminRole 포함)
adminUsers.MapGet("/", async (
    IUserRepository userRepo,
    CancellationToken cancellationToken) =>
{
    var users = await userRepo.GetAllAsync(cancellationToken);
    var response = users.Select(u => new
    {
        u.UserId,
        u.Username,
        u.Name,
        u.Phone,
        AdminRole = u.AdminRole.HasValue ? u.AdminRole.Value.ToString() : "일반 고객"
    });
    return Results.Ok(response);
});

// 특정 유저를 관리자로 지정 (AdminRole = 1)
adminUsers.MapPost("/{userId:int}/grant", async (
    int userId,
    IUserRepository userRepo,
    CancellationToken cancellationToken) =>
{
    // 유저 존재 여부 확인
    var user = await userRepo.GetByIdAsync(userId, cancellationToken);
    if (user == null)
        return Results.NotFound(new { success = false, message = $"UserId {userId} 를 찾을 수 없습니다." });

    // 이미 관리자인 경우
    if (user.AdminRole == 1)
        return Results.Ok(new { success = true, message = $"{user.Username} 은(는) 이미 관리자입니다." });

    await userRepo.UpdateAdminRoleAsync(userId, 1, cancellationToken);
    return Results.Ok(new { success = true, message = $"{user.Username} 을(를) 관리자로 지정했습니다." });
});

// 특정 유저의 관리자 권한 해제 (AdminRole = null)
adminUsers.MapPost("/{userId:int}/revoke", async (
    int userId,
    IUserRepository userRepo,
    CancellationToken cancellationToken) =>
{
    // 유저 존재 여부 확인
    var user = await userRepo.GetByIdAsync(userId, cancellationToken);
    if (user == null)
        return Results.NotFound(new { success = false, message = $"UserId {userId} 를 찾을 수 없습니다." });

    // 이미 일반 고객인 경우
    if (user.AdminRole == null)
        return Results.Ok(new { success = true, message = $"{user.Username} 은(는) 이미 일반 고객입니다." });

    await userRepo.UpdateAdminRoleAsync(userId, null, cancellationToken);
    return Results.Ok(new { success = true, message = $"{user.Username} 의 관리자 권한을 해제했습니다." });
});

var news = app.MapGroup("/api/news");

news.MapGet("/{year:int}", async (
    int year,
    HttpContext httpContext,
    IYearNewsRepository newsRepo,
    CancellationToken cancellationToken) =>
{
    var items = await newsRepo.GetByYearAsync(year, cancellationToken);
    var response = items.Select(n => new YearNewsResponse
    {
        NewsId = n.NewsId,
        Year = n.Year,
        Month = n.Month,
        Headline = n.Headline,
        Description = n.Description,
        Category = n.Category,
        ImageUrl = string.IsNullOrWhiteSpace(n.ImagePath)
            ? ""
            : $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/news/{n.NewsId}/image"
    });
    return Results.Ok(response);
});

news.MapGet("/{newsId:int}/image", async (
    int newsId,
    IYearNewsRepository newsRepo,
    IFileStorageService storage,
    CancellationToken cancellationToken) =>
{
    var item = await newsRepo.GetByIdAsync(newsId, cancellationToken);
    if (item == null || string.IsNullOrWhiteSpace(item.ImagePath))
        return Results.NotFound();

    var stream = await storage.OpenNewsImageReadAsync(item.ImagePath, cancellationToken);
    if (stream == null)
        return Results.NotFound();

    return Results.File(stream, storage.GetNewsImageContentType(item.ImagePath));
});

app.Run();
