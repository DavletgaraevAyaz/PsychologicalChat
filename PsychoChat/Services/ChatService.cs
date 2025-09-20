namespace PsychoChat.Services
{
    using PsychoChat.Models;
    using System.Collections.Concurrent;

    public class ChatService
    {
        private readonly ConcurrentDictionary<Guid, List<ChatMessage>> _chats = new();
        private readonly List<ChatSession> _availablePsychologists = new()
    {
        new ChatSession
        {
            Id = Guid.NewGuid(),
            PsychologistName = "Мария Иванова",
            PsychologistSpecialization = "Кризисная психология",
            IsOnline = true
        },
        new ChatSession
        {
            Id = Guid.NewGuid(),
            PsychologistName = "Алексей Петров",
            PsychologistSpecialization = "Семейная терапия",
            IsOnline = true
        },
        new ChatSession
        {
            Id = Guid.NewGuid(),
            PsychologistName = "Елена Сидорова",
            PsychologistSpecialization = "Когнитивно-поведенческая терапия",
            IsOnline = false
        }
    };

        public List<ChatSession> GetAvailablePsychologists()
        {
            return _availablePsychologists;
        }

        public List<ChatMessage> GetChatHistory(Guid chatId)
        {
            if (_chats.TryGetValue(chatId, out var messages))
            {
                return messages;
            }

            // Создаем новый чат с приветственным сообщением
            var welcomeMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Sender = "psychologist",
                Message = "Добро пожаловать! Я готов вас выслушать. Расскажите, что вас беспокоит?",
                Timestamp = DateTime.Now,
                IsRead = true
            };

            var newChat = new List<ChatMessage> { welcomeMessage };
            _chats[chatId] = newChat;

            return newChat;
        }

        public void SendMessage(Guid chatId, string message, string sender = "user")
        {
            if (!_chats.TryGetValue(chatId, out var messages))
            {
                messages = new List<ChatMessage>();
                _chats[chatId] = messages;
            }

            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatId = chatId,
                Sender = sender,
                Message = message,
                Timestamp = DateTime.Now,
                IsRead = sender == "user" // Сообщения пользователя сразу прочитаны
            };

            messages.Add(chatMessage);

            // Имитация ответа психолога
            if (sender == "user")
            {
                Task.Delay(2000).ContinueWith(_ =>
                {
                    var response = new ChatMessage
                    {
                        Id = Guid.NewGuid(),
                        ChatId = chatId,
                        Sender = "psychologist",
                        Message = GetPsychologistResponse(message),
                        Timestamp = DateTime.Now,
                        IsRead = false
                    };

                    messages.Add(response);
                    OnNewMessage?.Invoke(response);
                });
            }
        }

        public string GetPsychologistResponse(string userMessage)
        {
            // Простая имитация ответов психолога
            var responses = new[]
            {
            "Я понимаю ваши чувства. Расскажите подробнее?",
            "Это должно быть тяжело для вас. Хотите об этом поговорить?",
            "Спасибо, что делитесь этим. Я здесь, чтобы помочь.",
            "Я слушаю вас внимательно. Что вы чувствуете в этой ситуации?",
            "Это важное наблюдение. Давайте обсудим это глубже."
        };

            return responses[new Random().Next(responses.Length)];
        }

        public event Action<ChatMessage> OnNewMessage;
    }
}
