using System.Text.Json;
using System.Text.Json.Serialization;

namespace Supermaket.Helpers
{
	public static class SessionExtensions
	{
        public static void Set<T>(this ISession session, string key, T value)
        {
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,  // Sử dụng Preserve để xử lý vòng lặp tham chiếu
                MaxDepth = 64 // Tùy chọn này giúp giới hạn độ sâu của đối tượng (nếu cần)
            };

            session.SetString(key, JsonSerializer.Serialize(value, options));
        }

        public static T? Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve // Giải quyết vòng lặp tham chiếu khi deserialize
            });
        }

    }
}
