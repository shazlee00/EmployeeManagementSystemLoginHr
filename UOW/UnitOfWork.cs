using EmployeeManagementSystemLoginHr.Context;
using EmployeeManagementSystemLoginHr.Models;
using EmployeeManagementSystemLoginHr.repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EmployeeManagementSystemLoginHr.UOW
{
    public class UnitOfWork : IDisposable
    {

        private readonly ApplicationDbContext context;

        private IEmployeeRepository employeeRepository;
        private IDbContextTransaction _transaction;

        private IGenericRepository<AuditLog> _auditLogRepository;

        public UnitOfWork(ApplicationDbContext context )
        {
            this.context = context;
            
        }

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                employeeRepository ??= new EmployeeRepository(context);
                return employeeRepository;
            }
        }

        public IGenericRepository<AuditLog> AuditLogRepository
        {
            get
            {
                _auditLogRepository ??= new GenericRepository<AuditLog>(context);
                return _auditLogRepository;
            }
        }


        #region transaction support
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await context.SaveChangesAsync(cancellationToken);
                await _transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken); // Ensure this is awaited
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync(); // Ensure proper asynchronous disposal
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("No transaction in progress.");
            }

            try
            {
                await _transaction.RollbackAsync(cancellationToken); // Ensure this is awaited
            }
            finally
            {
                await _transaction.DisposeAsync(); // Ensure proper asynchronous disposal
                _transaction = null;
            }
        }
        #endregion


        public async Task<int> CompleteAsync()
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                int result = await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        public async Task<int> SaveAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
           
            
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
