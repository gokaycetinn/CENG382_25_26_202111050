using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Week5.Models;
namespace Week5.Data
{
 public class SchoolDbContext : IdentityDbContext
 {
 public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
 : base(options)
 {
 }
 public DbSet<Class> Classes { get; set; }
 }
}