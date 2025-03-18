using EmployeeManagementSystemLoginHr.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace EmployeeManagementSystemLoginHr.Context
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _httpContextAccessor;

        private  List<AuditLog>? auditLogs ;

        public AuditInterceptor(ICurrentUserService httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
           
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

            var entries = dbContext.ChangeTracker.Entries().Where(x=>x.Entity is not AuditLog && x.State != EntityState.Unchanged);

            if (!entries.Any()) { 
                return await base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            auditLogs = new List<AuditLog>();
            foreach (var entry in entries)
            {
                //if (!_tablesToAudit.Contains(entry.Metadata.ClrType.Name))
                //    continue; // Skip tables not in audit list

                if (entry.State == EntityState.Added)
                {
                    var auditLog = new AuditLog
                    {
                        TableName = entry.Metadata.ClrType.Name,
                        UserId = _httpContextAccessor.UserId,
                        UserName = _httpContextAccessor.UserName,
                        UserRoles = _httpContextAccessor.UserRole,
                    };
                    auditLog.Action = "INSERT";
                    Console.WriteLine(entry.CurrentValues.ToObject());
                    Console.WriteLine(entry.CurrentValues);

                    auditLog.ChangedData = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    auditLogs.Add(auditLog);

                }
                else if (entry.State == EntityState.Modified)
                {
                    var auditLog = new AuditLog
                    {
                        TableName = entry.Metadata.ClrType.Name,
                        UserId = _httpContextAccessor.UserId,
                        UserName = _httpContextAccessor.UserName,
                        UserRoles = _httpContextAccessor.UserRole,
                    };
                    auditLog.Action = "UPDATE";
                    auditLog.ChangedData = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    auditLogs.Add(auditLog);

                }
                else if (entry.State == EntityState.Deleted)
                {
                    var auditLog = new AuditLog
                    {
                        TableName = entry.Metadata.ClrType.Name,
                        UserId = _httpContextAccessor.UserId,
                        UserName = _httpContextAccessor.UserName,
                        UserRoles = _httpContextAccessor.UserRole,
                    };
                    auditLog.Action = "DELETE";
                    auditLog.ChangedData = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    auditLogs.Add(auditLog);

                }


            }



            return await base.SavingChangesAsync(eventData, result);

        }



        public override async ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,
            int result,
            CancellationToken cancellationToken = default)
        {
            var dbContext = eventData.Context;
            if (dbContext == null) return await base.SavedChangesAsync(eventData, result, cancellationToken);

            if (auditLogs is not null && auditLogs.Count > 0)
            {
                dbContext.Set<AuditLog>().AddRange(auditLogs);
                auditLogs = null;
                await dbContext.SaveChangesAsync();
            }


            return await base.SavedChangesAsync(eventData, result, cancellationToken);
        }
    }

}
