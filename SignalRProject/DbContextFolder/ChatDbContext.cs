using Microsoft.EntityFrameworkCore;
using SignalRProject.Models;

namespace SignalRProject.DbContextFolder
{
	public class ChatDbContext: DbContext
	{
        public ChatDbContext(DbContextOptions<ChatDbContext> options): base(options)
        {
        }
        public DbSet<Message> Message { get; set; }
    }
}
