using graph.chat.server.entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace graph.chat.server.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<Message> Messages { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<TopicMember> TopicMembers { get; set; }
}