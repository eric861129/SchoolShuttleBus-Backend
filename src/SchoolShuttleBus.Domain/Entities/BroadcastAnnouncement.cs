using SchoolShuttleBus.Domain.Common;
using SchoolShuttleBus.Domain.Shared;

namespace SchoolShuttleBus.Domain.Entities;

/// <summary>
/// 表示發送給特定對象的廣播公告。
/// </summary>
public sealed class BroadcastAnnouncement : AuditableEntity
{
    /// <summary>
    /// 公告接收對象。
    /// </summary>
    public BroadcastAudience Audience { get; set; }

    /// <summary>
    /// 公告主旨。
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// 公告內容。
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// 建立公告的使用者識別碼。
    /// </summary>
    public Guid CreatedByUserId { get; set; }
}
