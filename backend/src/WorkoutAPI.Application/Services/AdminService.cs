// Services/IAdminService.cs
using System.Security.Claims;
using System.Text;
using WorkoutAPI.Application.DTOs;

namespace WorkoutAPI.Application.Services {
    public interface IAdminService {
        Task<AdminResponse> CreateAdminAsync(CreateAdminRequest request);
        Task<LoginResponse> LoginAsync(string email, string password);
        Task<IEnumerable<AdminResponse>> GetAllAdminsAsync();
        Task<AdminResponse> GetAdminByIdAsync(Guid id);
        Task<AdminResponse> UpdateAdminAsync(Guid id, UpdateAdminRequest request);
        Task DeleteAdminAsync(Guid id);
        Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword);
    }
    public class AdminService : IAdminService {
        private readonly WorkoutDbContext _context;
        private readonly IConfiguration _configuration;

        public AdminService(WorkoutDbContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AdminResponse> CreateAdminAsync(CreateAdminRequest request) {
            // Check if email already exists
            var existingAdmin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == request.Email);

            if (existingAdmin != null)
                throw new InvalidOperationException("Admin with this email already exists");

            var admin = new Admin {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return MapToResponse(admin);
        }

        public async Task<LoginResponse> LoginAsync(string email, string password) {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == email && a.IsActive);

            if (admin == null || !VerifyPassword(password, admin.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            if (admin.IsLocked)
                throw new UnauthorizedAccessException("Account is locked");

            // Reset failed login attempts on successful login
            admin.FailedLoginAttempts = 0;
            admin.LastLoginAt = DateTime.UtcNow;
            admin.LockedUntil = null;

            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(admin);

            return new LoginResponse(
                Token: token,
                Admin: MapToResponse(admin),
                ExpiresAt: DateTime.UtcNow.AddHours(24)
            );
        }

        public async Task<IEnumerable<AdminResponse>> GetAllAdminsAsync() {
            var admins = await _context.Admins
                .Where(a => a.IsActive)
                .OrderBy(a => a.FirstName)
                .ToListAsync();

            return admins.Select(MapToResponse);
        }

        public async Task<AdminResponse> GetAdminByIdAsync(Guid id) {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                throw new KeyNotFoundException("Admin not found");

            return MapToResponse(admin);
        }

        public async Task<AdminResponse> UpdateAdminAsync(Guid id, UpdateAdminRequest request) {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                throw new KeyNotFoundException("Admin not found");

            admin.FirstName = request.FirstName;
            admin.LastName = request.LastName;
            admin.Email = request.Email;
            admin.Role = request.Role;
            admin.IsActive = request.IsActive;
            admin.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return MapToResponse(admin);
        }

        public async Task DeleteAdminAsync(Guid id) {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                throw new KeyNotFoundException("Admin not found");

            admin.IsActive = false;
            admin.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(Guid id, string currentPassword, string newPassword) {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Id == id);

            if (admin == null)
                throw new KeyNotFoundException("Admin not found");

            if (!VerifyPassword(currentPassword, admin.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect");

            admin.PasswordHash = HashPassword(newPassword);
            admin.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        private static string HashPassword(string password) {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private static bool VerifyPassword(string password, string hash) {
            return HashPassword(password) == hash;
        }

        private string GenerateJwtToken(Admin admin) {
            var jwtKey = _configuration["Jwt:Key"] ?? "DefaultSecretKeyForJWTTokenGeneration";
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "WorkoutAPI";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "WorkoutAPIUsers";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtKey);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.Email, admin.Email),
                    new Claim(ClaimTypes.Role, admin.Role.ToString()),
                    new Claim(ClaimTypes.Name, admin.FullName)
                }),
                Expires = DateTime.UtcNow.AddHours(24),
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static AdminResponse MapToResponse(Admin admin) {
            return new AdminResponse(
                Id: admin.Id,
                FirstName: admin.FirstName,
                LastName: admin.LastName,
                Email: admin.Email,
                Role: admin.Role,
                IsActive: admin.IsActive,
                CreatedAt: admin.CreatedAt,
                LastLoginAt: admin.LastLoginAt
            );
        }
    }
}
