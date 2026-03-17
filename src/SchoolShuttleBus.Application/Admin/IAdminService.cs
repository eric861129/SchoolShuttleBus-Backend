using SchoolShuttleBus.Application.Common;
using SchoolShuttleBus.Contracts.Admin;

namespace SchoolShuttleBus.Application.Admin;

public interface IAdminService
{
    Task<Result<DispatchOverrideResponse>> CreateDispatchOverrideAsync(CreateDispatchOverrideRequest request, CancellationToken cancellationToken);

    Task<Result<ReportExportResponse>> CreateReportAsync(CreateReportRequest request, Guid actorUserId, CancellationToken cancellationToken);

    Task<Result<(byte[] Content, string ContentType, string FileName)>> GetReportContentAsync(Guid reportId, CancellationToken cancellationToken);

    Task<AdminLookupsResponse> GetLookupsAsync(CancellationToken cancellationToken);
}
