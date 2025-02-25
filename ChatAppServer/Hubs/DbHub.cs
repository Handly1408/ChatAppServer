using ChatAppServer.Util;
using DAL.Entities;
using DAL.Enum;
using DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace ChatAppServer.Hubs
{
    [Authorize]
    public class DbHub : Hub
    {
        private ChatContactsEntityRepository ChatContactsRepository { get; set; } = new ();
        private ChatUserProfileDataEntityRepository ChatUserProfileEntityRepository { get; set; } = new ();
        private ChatUserNotificationTokenEntityRepository chatContactNotificationTokenEntityRepository { get; set; } = new ();

        public override Task OnConnectedAsync()
        {
            TimeUtil.GetCurrentTimeMilliseconds();
            TimeUtil.GetRegionTime();
            ClientsUtil.AddDbClient(Context, GetType().Name);

            Console.WriteLine(ClientsUtil.AuthorizedDbClients.Count);
           

            return base.OnConnectedAsync();
        }
        /// <summary>
        /// Save in db registered user
        /// </summary>
        /// <param name="chatUserEntity"></param>
        /// <returns></returns>
        public async Task<ChatUserProfileDataEntity> OnSaveMyUserProfileData(ChatUserProfileDataEntity? chatUserEntity)
        {

            // Здесь может быть ошибка (например, при сохранении в БД)

            if (chatUserEntity is null) return null;
            // Если всё хорошо, отправляем сообщение клиенту   
            ChatUserProfileEntityRepository.Insert(chatUserEntity);
            return ChatUserProfileEntityRepository.GetUserProfileDataById(chatUserEntity.ContactId)!;

        }
 
        public async Task<ChatUserProfileDataEntity> OnGetUserProfileData(string userProfileId)
        {

            // Здесь может быть ошибка (например, при сохранении в БД)

          return ChatUserProfileEntityRepository.GetUserProfileDataById(userProfileId)!;
            
              

        }
        public async Task<string> OnContactSave(ChatContactEntity chatGroupEntity)
        {
             
             // Здесь может быть ошибка (например, при сохранении в БД)
               
             ChatContactsRepository.Insert(chatGroupEntity);
            // Если всё хорошо, отправляем сообщение клиенту   
            return await Task.FromResult("Group save success");
   
        }
        public Task OnAddGroupMember(string groupNetworkId,string memberNetworkId)
        {
            var group = ChatContactsRepository.GetByContactId(groupNetworkId);

            if(CheckContactForGroup(group))
            {
                group!.Members?.Add(memberNetworkId);
                ChatContactsRepository.Update(group);
            }
            return Task.CompletedTask;
        }

        private bool CheckContactForGroup(ChatContactEntity? group)
        {
            return group != null && EnumUtil.GetEnumFromString<ContactType>(group.ContactType!).Equals(ContactType.GROUP);
            
        }

        /// <summary>
        /// Get client by its cnetwork client id
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        public ChatContactEntity OnGetContactById(string contactId, string? contactOwnerId)
        {

            // Здесь может быть ошибка (например, при сохранении в БД)
            ChatContactEntity? contactEntity;
           
            contactEntity = ChatContactsRepository.GetByContactId(contactId,contactOwnerId);
            Console.Write($"{nameof(OnGetContactById)} success -> contact id-> {contactEntity?.ContactId}, contact owner id -> {contactOwnerId} " );
      
            
           
            // Если всё хорошо, отправляем сообщение клиенту   
            return contactEntity!;

        }
        public Task OnSaveMyNotificationToken(string userNotificationtokenOwnerId, string notificationTokenId)
        {

            // Здесь может быть ошибка (например, при сохранении в БД)
            this.chatContactNotificationTokenEntityRepository.Insert(new ChatUserNotificationTokenEntity() { TokenUserOwnerId = userNotificationtokenOwnerId, NotificationToken = notificationTokenId });
            string notificationToken = chatContactNotificationTokenEntityRepository.GetUserNotificationTokenById(userNotificationtokenOwnerId)!;
            Console.Write($"{nameof(OnSaveMyNotificationToken)} success -> contact id-> {userNotificationtokenOwnerId} saved notification token {notificationToken}");



            // Если всё хорошо, отправляем сообщение клиенту   
            return Task.CompletedTask;

        }
        public Task OnRemoveGroupMember(string groupNetworkId, string memberNetworkId)
        {
            var group = ChatContactsRepository.GetByContactId(groupNetworkId);

            if (CheckContactForGroup(group))
            {
                group!.Members?.Remove(memberNetworkId);
                ChatContactsRepository.Update(group);
            }
            return Task.CompletedTask;
        }


        public override async Task<Task> OnDisconnectedAsync(Exception? exception)
        {
            Console.Write($"\nOn Disconnected");
            // Удаляем клиента из всех групп (если используешь группы)

            ClientsUtil.RemoveDbClient(Context, GetType().Name);
            return base.OnDisconnectedAsync(exception);
        }

        

     
        
    }
}
