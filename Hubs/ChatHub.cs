using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Luna.Services;
using Luna.Dtos;
using System.Security.Claims;

namespace Luna.Hubs
{
    [Authorize] // Только авторизованные пользователи могут подключаться к хабу
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IMessageService messageService, ILogger<ChatHub> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        // Когда пользователь подключается
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier; // Это UserId из токена
            _logger.LogInformation("Пользователь {UserId} подключился", userId);

            // Добавляем пользователя в группы всех его чатов
            // (нужно внедрить IChatService, но для простоты пока опустим)

            await base.OnConnectedAsync();
        }

        // Когда пользователь отключается
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            _logger.LogInformation("Пользователь {UserId} отключился", userId);
            await base.OnDisconnectedAsync(exception);
        }

        // Метод, который вызывает клиент, чтобы отправить сообщение
        public async Task SendMessage(SendMessageRequestDto request)
        {
            var userId = Context.UserIdentifier!;

            // Сохраняем сообщение через сервис
            var messageDto = await _messageService.SendMessageAsync(userId, request);

            // Отправляем сообщение всем участникам чата (включая отправителя)
            await Clients.Group(request.ChatId.ToString()).SendAsync("ReceiveMessage", messageDto);
        }

        // Присоединение к группе чата (клиент должен вызвать этот метод после подключения)
        public async Task JoinChatGroup(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
            _logger.LogInformation("Пользователь {UserId} вошёл в чат {ChatId}", Context.UserIdentifier, chatId);
        }

        // Выход из группы чата
        public async Task LeaveChatGroup(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        // Индикатор набора текста
        public async Task Typing(string chatId, bool isTyping)
        {
            var userId = Context.UserIdentifier;
            await Clients.OthersInGroup(chatId).SendAsync("UserTyping", userId, isTyping);
        }
    }
}