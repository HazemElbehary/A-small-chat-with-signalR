using Microsoft.AspNetCore.SignalR;
using SignalRProject.DbContextFolder;
using SignalRProject.Models;

namespace SignalRProject.HubFolder
{
    public class ChatHub(ChatDbContext dbContext): Hub
    {
        public async void Send(string userName, string message)
        {
            // 1. Save Message In DB
            Message msm = new Message() 
            {
                UserName = userName,
                Text = message 
            };
            await dbContext.Message.AddAsync(msm);
            dbContext.SaveChanges();

            // 2. Call JS Function That Fire Action in JS
            //    to Call All Receive MSM Fucntion "that Send For ALl Users This MSM"
            await Clients.Others.SendAsync("ReceiveMessage", userName, message);
        }
    
        public async void JoinGroup(string groupName, string userName)
        {
            // 1. Add All Users With ConnectionId to the Same Group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // 2. Call Method Send In Group "New Memeber Join"
            await Clients.OthersInGroup(groupName).SendAsync("NewMemberJoin", userName, groupName);
        }
    
        public async void SendToGroup(string groupName, string userName, string groupMessage)
        {
            // 1. Save Message In DB
            Message msm = new Message()
            {
                UserName = userName,
                Text = groupMessage
            };
            await dbContext.Message.AddAsync(msm);
            dbContext.SaveChanges();


            // 2. Call JS Function That Fire Action in JS
            //    to Call All Receive MSM Fucntion "that Send For ALl Users This MSM"
            await Clients.OthersInGroup(groupName).SendAsync("ReciveMessageFromGroup", userName, groupMessage);
        }
    }
}
